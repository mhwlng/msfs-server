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

namespace msfs_server.Components
{
    public partial class MovingMap
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public AircraftStatusSlowModel AircraftStatusSlow { get; set; }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MovingMap.razor.js").AsTask();

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            _hubConnection.On("MsFsSlowRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                var module = await ModuleReference;
                await module.InvokeVoidAsync("SetValues",
                    AircraftStatusSlow.Data.Latitude,
                    AircraftStatusSlow.Data.Longitude,
                    AircraftStatusSlow.Data.TrueHeading,
                    AircraftStatusSlow.Data.GPSFlightPlanActive,
                    AircraftStatusSlow.Data.GPSNextWPLatitude,
                    AircraftStatusSlow.Data.GPSNextWPLongitude,
                    AircraftStatusSlow.Data.GPSPrevWPLatitude,
                    AircraftStatusSlow.Data.GPSPrevWPLongitude);


            });

            await _hubConnection.StartAsync();
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

        async Task Init()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init");
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
