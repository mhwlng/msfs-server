using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using msfs_server.msfs;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;
using MudBlazor.Extensions;
using static MudBlazor.Colors;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.FlightSimulator.SimConnect;

namespace msfs_server.Components
{
    public partial class GarminG5
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public AircraftStatusFastModel AircraftStatusFast { get; set; }

        private double _bankDegrees;

        private double _pitchDegrees;

        private double _indicatedAltitude;

        private double _verticalSpeed;

        private double _airspeedIndicated;
        
        private bool _autopilotMaster;

        private double _autoPilotAltitudeLockVar;

        private bool _autopilotAltitudeLock;

        private double _gpsGroundSpeed;

        private double _kohlsmanSetting;

        private double _planeHeadingMagnetic;

        private double _autoPilotHeadingLockDir;

        private bool _autopilotHeadingLock;

        private double _turnCoordinatorBall;

        private double _navCDI;

        private double _navGSI;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/garming5.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _bankDegrees = AircraftStatusFast.BankDegrees;
            _pitchDegrees = AircraftStatusFast.PitchDegrees;
            _indicatedAltitude = AircraftStatusFast.IndicatedAltitude;
            _verticalSpeed = AircraftStatusFast.VerticalSpeed;
            _airspeedIndicated = AircraftStatusFast.AirspeedIndicated;

            _autopilotMaster = AircraftStatusFast.AutopilotMaster;
            _autoPilotAltitudeLockVar = AircraftStatusFast.AutoPilotAltitudeLockVar;
            _autopilotAltitudeLock = AircraftStatusFast.AutopilotAltitudeLock;
            _gpsGroundSpeed = AircraftStatusFast.GpsGroundSpeed;
            _kohlsmanSetting = AircraftStatusFast.KohlsmanSetting;
            _planeHeadingMagnetic = AircraftStatusFast.PlaneHeadingMagnetic;
            _autoPilotHeadingLockDir = AircraftStatusFast.AutoPilotHeadingLockDir;
            _autopilotHeadingLock = AircraftStatusFast.AutopilotHeadingLock;
            _turnCoordinatorBall = AircraftStatusFast.TurnCoordinatorBall;
            _navCDI = AircraftStatusFast.NavCDI;
            _navGSI = AircraftStatusFast.NavGSI;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                if (_bankDegrees != AircraftStatusFast.BankDegrees ||
                    _pitchDegrees != AircraftStatusFast.PitchDegrees ||
                    _indicatedAltitude != AircraftStatusFast.IndicatedAltitude ||
                    _verticalSpeed != AircraftStatusFast.VerticalSpeed ||
                    _airspeedIndicated != AircraftStatusFast.AirspeedIndicated)
                {
                    _bankDegrees = AircraftStatusFast.BankDegrees;
                    _pitchDegrees = AircraftStatusFast.PitchDegrees;
                    _indicatedAltitude = AircraftStatusFast.IndicatedAltitude;
                    _verticalSpeed = AircraftStatusFast.VerticalSpeed;
                    _airspeedIndicated = AircraftStatusFast.AirspeedIndicated;

                    _autopilotMaster = AircraftStatusFast.AutopilotMaster;
                    _autoPilotAltitudeLockVar = AircraftStatusFast.AutoPilotAltitudeLockVar;
                    _autopilotAltitudeLock = AircraftStatusFast.AutopilotAltitudeLock;
                    _gpsGroundSpeed = AircraftStatusFast.GpsGroundSpeed;
                    _kohlsmanSetting = AircraftStatusFast.KohlsmanSetting;
                    _planeHeadingMagnetic = AircraftStatusFast.PlaneHeadingMagnetic;
                    _autoPilotHeadingLockDir = AircraftStatusFast.AutoPilotHeadingLockDir;
                    _autopilotHeadingLock = AircraftStatusFast.AutopilotHeadingLock;
                    _turnCoordinatorBall = AircraftStatusFast.TurnCoordinatorBall;
                    _navCDI = AircraftStatusFast.NavCDI;
                    _navGSI = AircraftStatusFast.NavGSI;

                    await SetG5Values(
                        _bankDegrees, 
                        _pitchDegrees, 
                        _indicatedAltitude,
                        _verticalSpeed, 
                        _airspeedIndicated,

                        _autopilotMaster,
                        _autoPilotAltitudeLockVar,
                        _autopilotAltitudeLock,
                        _gpsGroundSpeed,
                        _kohlsmanSetting,
                        _planeHeadingMagnetic,
                        _autoPilotHeadingLockDir,
                        _autopilotHeadingLock,
                        _turnCoordinatorBall,
                        _navCDI,
                        _navGSI


                        );
                }
            });

            await hubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await InitG5();

                StateHasChanged();
            }
        }

        async Task SetG5Values(
            double bankDegrees,
            double pitchDegrees, 
            double indicatedAltitude,
            double verticalSpeed,
            double airspeedIndicated,

            bool autopilotMaster,
            double autoPilotAltitudeLockVar,
            bool autopilotAltitudeLock,
            double gpsGroundSpeed,
            double kohlsmanSetting,
            double planeHeadingMagnetic,
            double autoPilotHeadingLockDir,
            bool autopilotHeadingLock,
            double turnCoordinatorBall,
            double navCDI,
            double navGSI
                
            )
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetG5Values", 
                bankDegrees, 
                pitchDegrees, 
                indicatedAltitude,
                verticalSpeed, 
                airspeedIndicated,

                autopilotMaster,
                autoPilotAltitudeLockVar,
                autopilotAltitudeLock,
                gpsGroundSpeed,
                kohlsmanSetting,
                planeHeadingMagnetic,
                autoPilotHeadingLockDir,
                autopilotHeadingLock,
                turnCoordinatorBall,
                navCDI,
                navGSI

                );
        }

        async Task InitG5()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("InitG5");
        }

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }

            if (_moduleReference != null)
            {
                var module = await _moduleReference;
                await module.DisposeAsync();
            }
        }
    }
}
