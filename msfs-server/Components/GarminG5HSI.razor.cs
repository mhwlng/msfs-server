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
    public partial class GarminG5HSI
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public AircraftStatusFastModel AircraftStatusFast { get; set; }

        private double? _gpsGroundSpeed;
        private double? _planeHeadingMagnetic;
        private double? _nav1OBS;
        private double? _nav1CDI;
        private double? _nav1GSI;
        private double? _autoPilotHeadingLockDir;
        private bool? _autopilotHeadingLock;
        private bool? _autopilotMaster;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5HSI.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                if (_gpsGroundSpeed != AircraftStatusFast.GpsGroundSpeed ||
                    _planeHeadingMagnetic != AircraftStatusFast.PlaneHeadingMagnetic ||

                    _nav1OBS != AircraftStatusFast.Nav1OBS ||
                    _nav1CDI != AircraftStatusFast.Nav1CDI ||
                    _nav1GSI != AircraftStatusFast.Nav1GSI ||

                    _autoPilotHeadingLockDir != AircraftStatusFast.AutoPilotHeadingLockDir ||
                    _autopilotHeadingLock != AircraftStatusFast.AutopilotHeadingLock ||

                    _autopilotMaster != AircraftStatusFast.AutopilotMaster
                    )
                {
                    _gpsGroundSpeed = AircraftStatusFast.GpsGroundSpeed;
                    _planeHeadingMagnetic = AircraftStatusFast.PlaneHeadingMagnetic;

                    _nav1OBS = AircraftStatusFast.Nav1OBS;
                    _nav1CDI = AircraftStatusFast.Nav1CDI;
                    _nav1GSI = AircraftStatusFast.Nav1GSI;

                    _autoPilotHeadingLockDir = AircraftStatusFast.AutoPilotHeadingLockDir;
                    _autopilotHeadingLock = AircraftStatusFast.AutopilotHeadingLock;

                    _autopilotMaster = AircraftStatusFast.AutopilotMaster;

                    await SetValues(

                        AircraftStatusFast.GpsGroundSpeed,
                        AircraftStatusFast.PlaneHeadingMagnetic,
                        AircraftStatusFast.Nav1OBS,
                        AircraftStatusFast.Nav1CDI,
                        AircraftStatusFast.Nav1GSI,
                        AircraftStatusFast.AutoPilotHeadingLockDir,
                        AircraftStatusFast.AutopilotHeadingLock,
                        AircraftStatusFast.AutopilotMaster

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
                await Init();

                //StateHasChanged();
            }
        }

        async Task SetValues(

            double gpsGroundSpeed,
            double planeHeadingMagnetic,
            double nav1OBS,
            double nav1CDI,
            double nav1GSI,
            double autoPilotHeadingLockDir,
            bool autopilotHeadingLock,
            bool autopilotMaster

            )
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues",

                gpsGroundSpeed,
                planeHeadingMagnetic,
                nav1OBS,
                nav1CDI,
                nav1GSI,
                autoPilotHeadingLockDir,
                autopilotHeadingLock,
                autopilotMaster

                );
        }

        async Task Init()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init");
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
