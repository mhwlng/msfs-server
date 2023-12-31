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

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5.razor.js").AsTask();

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            _hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                var module = await ModuleReference;
                await module.InvokeVoidAsync("SetValues",
                    AircraftStatusFast.Data.BankDegrees,
                    AircraftStatusFast.Data.PitchDegrees,
                    AircraftStatusFast.Data.IndicatedAltitude,
                    AircraftStatusFast.Data.VerticalSpeed,
                    AircraftStatusFast.Data.AirspeedIndicated,

                    AircraftStatusFast.Data.AutopilotMaster,
                    AircraftStatusFast.Data.AutoPilotAltitudeLockVar,
                    AircraftStatusFast.Data.AutopilotAltitudeLock,
                    AircraftStatusFast.Data.GpsGroundSpeed,
                    AircraftStatusFast.Data.KohlsmanSetting,
                    AircraftStatusFast.Data.PlaneHeadingMagnetic,
                    AircraftStatusFast.Data.AutoPilotHeadingLockDir,
                    AircraftStatusFast.Data.AutopilotHeadingLock,
                    AircraftStatusFast.Data.TurnCoordinatorBall,
                    AircraftStatusFast.Data.Nav1CDI,
                    AircraftStatusFast.Data.Nav1GSI
                    
                );

            });

            await _hubConnection.StartAsync();
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

        async Task Init(string rangesJson)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init",
                rangesJson);
        }

        public bool IsConnected =>
            _hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }

            if (_moduleReference != null)
            {
                var module = await _moduleReference;
                await module.DisposeAsync();
            }
        }
    }
}
