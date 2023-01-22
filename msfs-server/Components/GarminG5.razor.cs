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

        private double _bankDegrees;

        private double _pitchDegrees;

        private double _indicatedAltitude;

        private int _verticalSpeed;

        private double _airspeedIndicated;


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

                    await SetG5Values(_bankDegrees, _pitchDegrees, _indicatedAltitude, _verticalSpeed, _airspeedIndicated);
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

        async Task SetG5Values(double bankDegrees, double pitchDegrees, double indicatedAltitude, int verticalSpeed, double airspeedIndicated)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetG5Values", bankDegrees, pitchDegrees, indicatedAltitude, verticalSpeed, airspeedIndicated);
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
