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

        [Inject] public GarminG5Model GarminG5Data { get; set; }

        [Parameter] public string RangesJson { get; set; }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5.razor.js").AsTask();

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            _hubConnection.On("MsFsGarminG5Refresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                var module = await ModuleReference;
                await module.InvokeVoidAsync("SetValues",
                    GarminG5Data.Data.BankDegrees,
                    GarminG5Data.Data.PitchDegrees,
                    GarminG5Data.Data.IndicatedAltitude,
                    GarminG5Data.Data.VerticalSpeed,
                    GarminG5Data.Data.AirspeedIndicated,

                    GarminG5Data.Data.AutopilotMaster,
                    GarminG5Data.Data.AutoPilotAltitudeLockVar,
                    GarminG5Data.Data.AutopilotAltitudeLock,
                    GarminG5Data.Data.GpsGroundSpeed,
                    GarminG5Data.Data.KohlsmanSetting,
                    GarminG5Data.Data.PlaneHeadingMagnetic,
                    GarminG5Data.Data.AutoPilotHeadingLockDir,
                    GarminG5Data.Data.AutopilotHeadingLock,
                    GarminG5Data.Data.TurnCoordinatorBall,
                    GarminG5Data.Data.Nav1CDI,
                    GarminG5Data.Data.Nav1GSI
                    
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
