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

        private double _gpsGroundSpeed;

        private double _planeHeadingMagnetic;

        private double _navOBS;

        private double _navCDI;

        private double _navGSI;

        private double _autoPilotHeadingLockDir;

        private bool _autopilotHeadingLock;

        private bool _autopilotMaster;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5HSI.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _gpsGroundSpeed = AircraftStatusFast.GpsGroundSpeed;

            _planeHeadingMagnetic = AircraftStatusFast.PlaneHeadingMagnetic;

            _navOBS = AircraftStatusFast.NavOBS;

            _navCDI = AircraftStatusFast.NavCDI;
            _navGSI = AircraftStatusFast.NavGSI;

            _autoPilotHeadingLockDir = AircraftStatusFast.AutoPilotHeadingLockDir;
            _autopilotHeadingLock = AircraftStatusFast.AutopilotHeadingLock;

            _autopilotMaster = AircraftStatusFast.AutopilotMaster;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                if (_gpsGroundSpeed != AircraftStatusFast.GpsGroundSpeed ||
                    _planeHeadingMagnetic != AircraftStatusFast.PlaneHeadingMagnetic)
                {
                    _gpsGroundSpeed = AircraftStatusFast.GpsGroundSpeed;
                    _planeHeadingMagnetic = AircraftStatusFast.PlaneHeadingMagnetic;

                    _navOBS = AircraftStatusFast.NavOBS;
                    _navCDI = AircraftStatusFast.NavCDI;
                    _navGSI = AircraftStatusFast.NavGSI;

                    _autoPilotHeadingLockDir = AircraftStatusFast.AutoPilotHeadingLockDir;
                    _autopilotHeadingLock = AircraftStatusFast.AutopilotHeadingLock;

                    _autopilotMaster = AircraftStatusFast.AutopilotMaster;

                    await SetValues(

                        _gpsGroundSpeed,
                        _planeHeadingMagnetic,
                        _navOBS,
                        _navCDI,
                        _navGSI,
                        _autoPilotHeadingLockDir,
                        _autopilotHeadingLock,
                        _autopilotMaster

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

                StateHasChanged();
            }
        }

        async Task SetValues(

            double gpsGroundSpeed,
            double planeHeadingMagnetic,
            double navOBS,
            double navCDI,
            double navGSI,
            double autoPilotHeadingLockDir,
            bool autopilotHeadingLock,
            bool autopilotMaster

            )
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues",

                gpsGroundSpeed,
                planeHeadingMagnetic,
                navOBS,
                navCDI,
                navGSI,
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
