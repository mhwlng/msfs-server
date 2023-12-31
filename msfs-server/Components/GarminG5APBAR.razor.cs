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
    public partial class GarminG5APBar
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public AircraftStatusFastModel AircraftStatusFast { get; set; }

        private bool? _autopilotMaster;
        private bool? _autopilotAltitudeLock;
        private bool? _autopilotHeadingLock;
        private bool? _autopilotNav1Lock;
        private bool? _autopilotFlightDirectorActive;
        private bool? _autopilotBackcourseHold;
        private bool? _autopilotVerticalHold;
        private bool? _autopilotYawDamper;
        private bool? _autopilotApproachHold;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5APBAR.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                if (_autopilotMaster != AircraftStatusFast.StatusFast.AutopilotMaster ||
                    _autopilotAltitudeLock != AircraftStatusFast.StatusFast.AutopilotAltitudeLock ||
                    _autopilotHeadingLock != AircraftStatusFast.StatusFast.AutopilotHeadingLock ||
                    _autopilotNav1Lock != AircraftStatusFast.StatusFast.AutopilotNav1Lock ||
                    _autopilotFlightDirectorActive != AircraftStatusFast.StatusFast.AutopilotFlightDirectorActive ||
                    _autopilotBackcourseHold != AircraftStatusFast.StatusFast.AutopilotBackcourseHold ||
                    _autopilotVerticalHold != AircraftStatusFast.StatusFast.AutopilotVerticalHold ||
                    _autopilotYawDamper != AircraftStatusFast.StatusFast.AutopilotYawDamper ||
                    _autopilotApproachHold != AircraftStatusFast.StatusFast.AutopilotApproachHold
                    )
                {
                
                    _autopilotMaster = AircraftStatusFast.StatusFast.AutopilotMaster;
                    _autopilotAltitudeLock = AircraftStatusFast.StatusFast.AutopilotAltitudeLock;
                    _autopilotHeadingLock = AircraftStatusFast.StatusFast.AutopilotHeadingLock;
                    _autopilotNav1Lock = AircraftStatusFast.StatusFast.AutopilotNav1Lock;
                    _autopilotFlightDirectorActive = AircraftStatusFast.StatusFast.AutopilotFlightDirectorActive;
                    _autopilotBackcourseHold = AircraftStatusFast.StatusFast.AutopilotBackcourseHold;
                    _autopilotVerticalHold = AircraftStatusFast.StatusFast.AutopilotVerticalHold;
                    _autopilotYawDamper = AircraftStatusFast.StatusFast.AutopilotYawDamper;
                    _autopilotApproachHold = AircraftStatusFast.StatusFast.AutopilotApproachHold;

                    await SetValues(

                        AircraftStatusFast.StatusFast.AutopilotMaster,
                        AircraftStatusFast.StatusFast.AutopilotAltitudeLock,
                        AircraftStatusFast.StatusFast.AutopilotHeadingLock,
                        AircraftStatusFast.StatusFast.AutopilotNav1Lock,
                        AircraftStatusFast.StatusFast.AutopilotFlightDirectorActive,
                        AircraftStatusFast.StatusFast.AutopilotBackcourseHold,
                        AircraftStatusFast.StatusFast.AutopilotVerticalHold,
                        AircraftStatusFast.StatusFast.AutopilotYawDamper,
                        AircraftStatusFast.StatusFast.AutopilotApproachHold

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

            bool autopilotMaster,
            bool autopilotAltitudeLock,
            bool autopilotHeadingLock,
            bool autopilotNav1Lock,
            bool autopilotFlightDirectorActive,
            bool autopilotBackcourseHold,
            bool autopilotVerticalHold,
            bool autopilotYawDamper,
            bool autopilotApproachHold

            )
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues",
                
                autopilotMaster,
                autopilotAltitudeLock,
                autopilotHeadingLock,
                autopilotNav1Lock,
                autopilotFlightDirectorActive,
                autopilotBackcourseHold,
                autopilotVerticalHold,
                autopilotYawDamper,
                autopilotApproachHold

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
