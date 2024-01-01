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
        private readonly MovingMapModel _movingMapData;
        private readonly GarminG5Model _garminG5Data;
        private readonly GarminG5ApbarModel _garminG5ApbarData;
        private readonly GarminG5HsiModel _garminG5HsiData;

        private static Task _simTask;
        private static readonly CancellationTokenSource SimTokenSource = new();

        private IntPtr WindowHandle { get; }

        public static SimConnect Simconnect { get; private set; } = null;

        const uint WmUserSimconnect = 0x0402;

        private bool _simConnected = false;
        private bool _simDisconnected = false;

        private static int _fastCounter = 0;

        public Worker(MovingMapModel movingMapData, 
                      GarminG5Model garminG5Data, 
                      GarminG5ApbarModel garminG5ApbarData,
                      GarminG5HsiModel garminG5HsiData)
        {

            var win = MessageWindow.GetWindow();
            WindowHandle = win.Hwnd;
            win.WndProcHandle += W_WndProcHandle;

            _movingMapData = movingMapData;
            _garminG5Data = garminG5Data;
            _garminG5ApbarData = garminG5ApbarData;
            _garminG5HsiData = garminG5HsiData;
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
            foreach (var fieldInfo in typeof(SimConnectStructs.MovingMapStruct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.Definitions.AIRCRAFT_STATUS_MOVINGMAP,
                        dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.MovingMapStruct>(SimConnectStructs.Definitions
                .AIRCRAFT_STATUS_MOVINGMAP);

            //------------------------

            foreach (var fieldInfo in typeof(SimConnectStructs.GarminG5Struct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5, dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.GarminG5Struct>(SimConnectStructs.Definitions
                .AIRCRAFT_STATUS_GARMING5);

            //------------------------

            foreach (var fieldInfo in typeof(SimConnectStructs.GarminG5ApbarStruct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5_APBAR,
                        dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.GarminG5ApbarStruct>(SimConnectStructs.Definitions
                .AIRCRAFT_STATUS_GARMING5_APBAR);

            //------------------------

            foreach (var fieldInfo in typeof(SimConnectStructs.GarminG5HsiStruct).GetFields())
            {
                for (var index = 0; index < fieldInfo.GetCustomAttributes(true).Length; index++)
                {
                    var dd = (DataDefinition)fieldInfo.GetCustomAttributes(true)[index];
                    Simconnect.AddToDataDefinition(SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5_HSI,
                        dd.DatumName,
                        dd.UnitsName, dd.DatumType, dd.fEpsilon, SimConnect.SIMCONNECT_UNUSED);
                }
            }

            Simconnect.RegisterDataDefineStruct<SimConnectStructs.GarminG5HsiStruct>(SimConnectStructs.Definitions
                .AIRCRAFT_STATUS_GARMING5_HSI);
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
                case (uint)SimConnectStructs.DataRequest.AIRCRAFT_STATUS_MOVINGMAP:

                    var simobjectDataMovingMap = (SimConnectStructs.MovingMapStruct)data.dwData[0];

                    _movingMapData.SetData(simobjectDataMovingMap);

                    break;
                case (uint)SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5:

                    var simobjectDataGarminG5 = (SimConnectStructs.GarminG5Struct)data.dwData[0];

                    _garminG5Data.SetData(simobjectDataGarminG5);

                    break;

                case (uint)SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5_APBAR:

                    var simobjectDataGarminG5Apbar = (SimConnectStructs.GarminG5ApbarStruct)data.dwData[0];

                    _garminG5ApbarData.SetData(simobjectDataGarminG5Apbar);

                    break;

                case (uint)SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5_HSI:

                    var simobjectDataGarminG5Hsi = (SimConnectStructs.GarminG5HsiStruct)data.dwData[0];

                    _garminG5HsiData.SetData(simobjectDataGarminG5Hsi);

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

        public enum GroupId
        {
            GROUP0
        };

        public enum Events
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
            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_MASTER, "AP_MASTER");

            Simconnect.MapClientEventToSimEvent(Events.KEY_YAW_DAMPER_TOGGLE, "YAW_DAMPER_TOGGLE");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_HDG_HOLD, "AP_HDG_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_ALT_HOLD, "AP_ALT_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_NAV1_HOLD, "AP_NAV1_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_BC_HOLD, "AP_BC_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_APR_HOLD, "AP_APR_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_AP_VS_HOLD, "AP_VS_HOLD");

            Simconnect.MapClientEventToSimEvent(Events.KEY_TOGGLE_FLIGHT_DIRECTOR, "TOGGLE_FLIGHT_DIRECTOR");

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

            var simToken = SimTokenSource.Token;

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

                                Simconnect.RequestDataOnSimObjectType(SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5,
                                    SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

                                Simconnect.RequestDataOnSimObjectType(SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5_APBAR,
                                    SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5_APBAR, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

                                Simconnect.RequestDataOnSimObjectType(SimConnectStructs.DataRequest.AIRCRAFT_STATUS_GARMING5_HSI,
                                    SimConnectStructs.Definitions.AIRCRAFT_STATUS_GARMING5_HSI, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

                                _fastCounter++;

                                //--------------------

                                if (_fastCounter >= 10) // 1 per second 
                                {

                                    Simconnect.RequestDataOnSimObjectType(
                                        SimConnectStructs.DataRequest.AIRCRAFT_STATUS_MOVINGMAP,
                                        SimConnectStructs.Definitions.AIRCRAFT_STATUS_MOVINGMAP, 0,
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

                    await Task.Delay(100, SimTokenSource.Token);
                }


            }, simToken);

            Log.Information("Init Worker");

            stoppingToken.WaitHandle.WaitOne();

            if (Simconnect != null)
            {
                Simconnect.Dispose();
                Simconnect = null;
            }

            await SimTokenSource.CancelAsync();

            try
            {
                await _simTask?.WaitAsync(simToken)!;
            }
            catch (OperationCanceledException)
            {
                Log.Information("Sim background task ended");
            }
            finally
            {
                SimTokenSource.Dispose();
            }

            Log.Information("Shutdown Worker");
        }
    }
}
