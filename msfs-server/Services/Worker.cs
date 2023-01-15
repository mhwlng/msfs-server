using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using msfs_server.Hubs;
using msfs_server.msfs;
using System.Threading.Tasks;
using System.Threading;
using Serilog;
using Newtonsoft.Json;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Xml.Linq;

namespace msfs_server.Services
{

    // thanks to https://github.com/kurt1288/msfs-flight-following

 
    public class Worker : BackgroundService
    {
        private AircraftStatusModel _aircraftStatus;

        private static Task _simTask;
        private static CancellationTokenSource _simTokenSource = new();

        private IntPtr WindowHandle { get; }

        private SimConnect simconnect = null;

        const uint WmUserSimconnect = 0x0402;

        private bool _simConnected = false;
        private bool _simDisconnected = false;

        public Worker(AircraftStatusModel aircraftStatus)
        {

            var win = MessageWindow.GetWindow();
            WindowHandle = win.Hwnd;
            win.WndProcHandle += W_WndProcHandle;

            _aircraftStatus = aircraftStatus;
        }

        private IntPtr W_WndProcHandle(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (msg == WmUserSimconnect)
                {
                    if (simconnect != null && !_simDisconnected)
                    {
                        simconnect.ReceiveMessage();
                    }
                }
                
            }
            catch
            {
                SimDisconnect();
            }

            return IntPtr.Zero;
        }

        private void SetFlightDataDefinitions()
        {
            foreach (var fieldInfo in typeof(SimConnectStructs.AircraftStatusStruct).GetFields())
            {
                foreach (DataDefinition dd in fieldInfo.GetCustomAttributes(true))
                {
                    simconnect.AddToDataDefinition(SimConnectStructs.DEFINITIONS.AircraftStatus, dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            simconnect.RegisterDataDefineStruct<SimConnectStructs.AircraftStatusStruct>(SimConnectStructs.DEFINITIONS.AircraftStatus);
        }

        private void SimDisconnect()
        {
            _simConnected = false;
            _simDisconnected = true;
        }

        private void RecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            switch (data.dwRequestID)
            {
                case (uint)SimConnectStructs.DATA_REQUEST.AircraftStatus:

                    _aircraftStatus.SetData((SimConnectStructs.AircraftStatusStruct)data.dwData[0]);

                    break;
            }
        }

        private void OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            _simConnected = true;

            Log.Information("Simconnect has connected to the flight sim.");
        }

        private void OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            SimDisconnect();

            Log.Information("Simconnect has disconnected to the flight sim.");
        }

        private void RecvExceptionHandler(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SimDisconnect();

            Log.Error("SimConnect exception: {0}", data.dwException);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (UserHandler.ConnectedIds.Count == 0)
            {
                await Task.Delay(1000, stoppingToken);
            }

            var simToken = _simTokenSource.Token;

            _simTask = Task.Run(async () =>
            {
                Log.Information("Sim task started");

                while (true)
                {
                    if (simToken.IsCancellationRequested)
                    {
                        simToken.ThrowIfCancellationRequested();
                    }

                    if (simconnect == null)
                    {
                        try
                        {
                            simconnect = new SimConnect("MSFS Flight Following", WindowHandle, WmUserSimconnect, null, 0);

                            if (simconnect != null)
                            {
                                SetFlightDataDefinitions();
                                simconnect.OnRecvOpen += OnRecvOpen;
                                simconnect.OnRecvQuit += OnRecvQuit;
                                simconnect.OnRecvException += RecvExceptionHandler;
                                
                                simconnect.OnRecvSimobjectDataBytype += RecvSimobjectDataBytype;

                            }
                        }
                        catch //(COMException ex)
                        {
                           // Log.Error("Unable to create new SimConnect instance: {0}", ex.Message);
                            simconnect = null;
                        }
                    }

                    if (simconnect != null)
                    {
                        if (_simDisconnected)
                        {
                            simconnect.Dispose();
                            simconnect = null;
                            _simDisconnected = false;
                        }
                        else if (simconnect != null && _simConnected)
                        {
                            try
                            {
                                simconnect.RequestDataOnSimObjectType(SimConnectStructs.DATA_REQUEST.AircraftStatus,
                                SimConnectStructs.DEFINITIONS.AircraftStatus, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
                            }
                            catch (COMException ex)
                            {
                                 Log.Error("RequestDataOnSimObjectType error: {0}", ex.Message);
                                //simconnect = null;
                            }
                        }

                    }

                    await Task.Delay(1000, _simTokenSource.Token); // repeat every 5 seconds
                }


            }, simToken);

            Log.Information("Init Worker");

            stoppingToken.WaitHandle.WaitOne();

            if (simconnect != null)
            {
                simconnect.Dispose();
                simconnect = null;
            }

            _simTokenSource.Cancel();

            try
            {
                _simTask?.Wait(simToken);
            }
            catch (OperationCanceledException)
            {
                Log.Information("Sim background task ended");
            }
            finally
            {
                _simTokenSource.Dispose();
            }

            Log.Information("Shutdown Worker");
        }
    }
}
