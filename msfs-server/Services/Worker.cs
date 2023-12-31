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
        private readonly AircraftStatusSlowModel _aircraftStatusSlow;

        private readonly AircraftStatusFastModel _aircraftStatusFast;

        private static Task _simTask;
        private static readonly CancellationTokenSource _simTokenSource = new();

        private IntPtr WindowHandle { get; }

        public static SimConnect Simconnect { get; private set; } = null;

        const uint WmUserSimconnect = 0x0402;

        private bool _simConnected = false;
        private bool _simDisconnected = false;

        private static int _fastCounter = 0;

        public Worker(AircraftStatusSlowModel aircraftStatusSlow, AircraftStatusFastModel aircraftStatusFast)
        {

            var win = MessageWindow.GetWindow();
            WindowHandle = win.Hwnd;
            win.WndProcHandle += W_WndProcHandle;

            _aircraftStatusSlow = aircraftStatusSlow;
            _aircraftStatusFast = aircraftStatusFast;
        }

        private IntPtr W_WndProcHandle(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (msg == WmUserSimconnect)
                {
                    if (Simconnect != null && !_simDisconnected)
                    {
                        Simconnect.ReceiveMessage();
                    }
                }
                
            }
            catch
            {
                SimDisconnect();
            }

            return IntPtr.Zero;
        }

        private static void SetFlightDataDefinitions()
        {
            foreach (var fieldInfo in typeof(SimConnectStructs.AircraftStatusSlowStruct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.DEFINITIONS.AircraftStatusSlow, dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.AircraftStatusSlowStruct>(SimConnectStructs.DEFINITIONS.AircraftStatusSlow);

            //------------------------
            
            foreach (var fieldInfo in typeof(SimConnectStructs.AircraftStatusFastStruct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.DEFINITIONS.AircraftStatusFast, dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.AircraftStatusFastStruct>(SimConnectStructs.DEFINITIONS.AircraftStatusFast);
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
                case (uint)SimConnectStructs.DATA_REQUEST.AircraftStatusSlow:

                    var simobjectDataSlow = (SimConnectStructs.AircraftStatusSlowStruct)data.dwData[0];

                    _aircraftStatusSlow.SetData(simobjectDataSlow);

                    break;
                case (uint)SimConnectStructs.DATA_REQUEST.AircraftStatusFast:

                    var simobjectDataFast = (SimConnectStructs.AircraftStatusFastStruct)data.dwData[0];

                    simobjectDataFast.GeneralEngineOilTemperature = ((5.0 / 9.0) * simobjectDataFast.GeneralEngineOilTemperature) - 273.15; // convert to celcius

                    simobjectDataFast.GeneralEngineOilPressure /= 144.0; // convert to psi

                    _aircraftStatusFast.SetData(simobjectDataFast);

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

        public enum GROUP_ID
        {
            GROUP0
        };

        public enum EVENTS
        {
            KEY_AP_MASTER,
            KEY_YAW_DAMPER_TOGGLE,
            KEY_AP_HDG_HOLD,
            KEY_AP_ALT_HOLD,
            KEY_AP_NAV1_HOLD,
            KEY_AP_BC_HOLD,
            KEY_AP_APR_HOLD,
            KEY_AP_VS_HOLD,
            KEY_TOGGLE_FLIGHT_DIRECTOR
        };
        
        [DataDefinition("AUTOPILOT FLIGHT DIRECTOR ACTIVE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f)]
        public bool AutopilotFlightDirectorActive;

      private static void MapClientEventToSimEvent()
        {
            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_MASTER, "AP_MASTER");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_YAW_DAMPER_TOGGLE, "YAW_DAMPER_TOGGLE");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_HDG_HOLD, "AP_HDG_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_ALT_HOLD, "AP_ALT_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_NAV1_HOLD, "AP_NAV1_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_BC_HOLD, "AP_BC_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_APR_HOLD, "AP_APR_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_AP_VS_HOLD, "AP_VS_HOLD");

            Simconnect.MapClientEventToSimEvent(EVENTS.KEY_TOGGLE_FLIGHT_DIRECTOR, "TOGGLE_FLIGHT_DIRECTOR");

            //Simconnect.AddClientEventToNotificationGroup(GROUP_ID.GROUP0, EVENTS.KEY_AP_MASTER, false);
            //Simconnect.SubscribeToSystemEvent(EVENTS.KEY_AP_MASTER, "AP_MASTER");
            // Simconnect.SetNotificationGroupPriority(GROUP_ID.GROUP0, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST);


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

                    if (Simconnect == null)
                    {
                        try
                        {
                            Simconnect = new SimConnect("MSFS Flight Following", WindowHandle, WmUserSimconnect, null, 0);

                            if (Simconnect != null)
                            {
                                SetFlightDataDefinitions();
                                Simconnect.OnRecvOpen += OnRecvOpen;
                                Simconnect.OnRecvQuit += OnRecvQuit;
                                Simconnect.OnRecvException += RecvExceptionHandler;
                                
                                Simconnect.OnRecvSimobjectDataBytype += RecvSimobjectDataBytype;

                                MapClientEventToSimEvent();

                            }
                        }
                        catch //(COMException ex)
                        {
                           // Log.Error("Unable to create new SimConnect instance: {0}", ex.Message);
                            Simconnect = null;
                        }
                    }

                    if (Simconnect != null)
                    {
                        if (_simDisconnected)
                        {
                            Simconnect.Dispose();
                            Simconnect = null;
                            _simDisconnected = false;
                        }
                        else if (Simconnect != null && _simConnected)
                        {
                            try
                            {
                                // 10 times per second

                                Simconnect.RequestDataOnSimObjectType(SimConnectStructs.DATA_REQUEST.AircraftStatusFast,
                                    SimConnectStructs.DEFINITIONS.AircraftStatusFast, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

                                _fastCounter++;

                                //--------------------

                                if (_fastCounter >= 10) // 1 per second 
                                {

                                    Simconnect.RequestDataOnSimObjectType(
                                        SimConnectStructs.DATA_REQUEST.AircraftStatusSlow,
                                        SimConnectStructs.DEFINITIONS.AircraftStatusSlow, 0,
                                        SIMCONNECT_SIMOBJECT_TYPE.USER);

                                    _fastCounter = 0;
                                }




                            }
                            catch (COMException ex)
                            {
                                 Log.Error("RequestDataOnSimObjectType error: {0}", ex.Message);
                                //simconnect = null;
                            }
                        }

                    }

                    await Task.Delay(100, _simTokenSource.Token);
                }


            }, simToken);

            Log.Information("Init Worker");

            stoppingToken.WaitHandle.WaitOne();

            if (Simconnect != null)
            {
                Simconnect.Dispose();
                Simconnect = null;
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
