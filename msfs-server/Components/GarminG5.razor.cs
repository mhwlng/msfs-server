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
    public partial class GarminG5
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Inject] public AircraftStatusFastModel AircraftStatusFast { get; set; }

        private double _bankdegrees;

        private double _pitchdegrees;
        
        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/garming5.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _bankdegrees = AircraftStatusFast.BankDegrees;
            _pitchdegrees = AircraftStatusFast.PitchDegrees;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                if (_bankdegrees != AircraftStatusFast.BankDegrees ||
                    _pitchdegrees != AircraftStatusFast.PitchDegrees)
                {
                    _bankdegrees = AircraftStatusFast.BankDegrees;
                    _pitchdegrees = AircraftStatusFast.PitchDegrees;

                    await SetG5Values(_bankdegrees, _pitchdegrees);
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

        async Task SetG5Values(double bankdegrees, double pitchdegrees)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetG5Values", bankdegrees, pitchdegrees);
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
