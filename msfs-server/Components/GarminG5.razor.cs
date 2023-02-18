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

        [Parameter] public string RangesJson { get; set; }

        private double? _bankDegrees;
        private double? _pitchDegrees;
        private double? _indicatedAltitude;
        private double? _verticalSpeed;
        private double? _airspeedIndicated;
        private bool? _autopilotMaster;
        private double? _autoPilotAltitudeLockVar;
        private bool? _autopilotAltitudeLock;
        private double? _gpsGroundSpeed;
        private double? _kohlsmanSetting;
        private double? _planeHeadingMagnetic;
        private double? _autoPilotHeadingLockDir;
        private bool? _autopilotHeadingLock;
        private double? _turnCoordinatorBall;
        private double? _nav1CDI;
        private double? _nav1GSI;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
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
                    _airspeedIndicated != AircraftStatusFast.AirspeedIndicated ||

                    _autopilotMaster != AircraftStatusFast.AutopilotMaster ||
                    _autoPilotAltitudeLockVar != AircraftStatusFast.AutoPilotAltitudeLockVar ||
                    _autopilotAltitudeLock != AircraftStatusFast.AutopilotAltitudeLock ||
                    _gpsGroundSpeed != AircraftStatusFast.GpsGroundSpeed ||
                    _kohlsmanSetting != AircraftStatusFast.KohlsmanSetting ||
                    _planeHeadingMagnetic != AircraftStatusFast.PlaneHeadingMagnetic ||
                    _autoPilotHeadingLockDir != AircraftStatusFast.AutoPilotHeadingLockDir ||
                    _autopilotHeadingLock != AircraftStatusFast.AutopilotHeadingLock ||
                    _turnCoordinatorBall != AircraftStatusFast.TurnCoordinatorBall ||
                    _nav1CDI != AircraftStatusFast.Nav1CDI ||
                    _nav1GSI != AircraftStatusFast.Nav1GSI 

                    )
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
                    _nav1CDI = AircraftStatusFast.Nav1CDI;
                    _nav1GSI = AircraftStatusFast.Nav1GSI;

                    await SetValues(
                        AircraftStatusFast.BankDegrees,
                        AircraftStatusFast.PitchDegrees,
                        AircraftStatusFast.IndicatedAltitude,
                        AircraftStatusFast.VerticalSpeed,
                        AircraftStatusFast.AirspeedIndicated,

                        AircraftStatusFast.AutopilotMaster,
                        AircraftStatusFast.AutoPilotAltitudeLockVar,
                        AircraftStatusFast.AutopilotAltitudeLock,
                        AircraftStatusFast.GpsGroundSpeed,
                        AircraftStatusFast.KohlsmanSetting,
                        AircraftStatusFast.PlaneHeadingMagnetic,
                        AircraftStatusFast.AutoPilotHeadingLockDir,
                        AircraftStatusFast.AutopilotHeadingLock,
                        AircraftStatusFast.TurnCoordinatorBall,
                        AircraftStatusFast.Nav1CDI,
                        AircraftStatusFast.Nav1GSI


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
                await Init(RangesJson);

                //StateHasChanged();
            }
        }

        async Task SetValues(
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
            double nav1CDI,
            double nav1GSI
                
            )
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues", 
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
                nav1CDI,
                nav1GSI

                );
        }

        async Task Init(string rangesJson)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init",
                rangesJson);
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
