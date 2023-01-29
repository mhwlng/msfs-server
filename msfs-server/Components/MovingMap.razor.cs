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

        private double _latitude;

        private double _longitude;

        private double _heading;

        private bool _gpsFlightPlanActive;
        private double _gpsNextWpLatitude;
        private double _gpsNextWpLongitude;
        private double _gpsPrevWpLatitude;
        private double _gpsPrevWpLongitude;


        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MovingMap.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _longitude = AircraftStatusSlow.Longitude;
            _latitude = AircraftStatusSlow.Latitude;
            _heading = AircraftStatusSlow.TrueHeading;

            _gpsFlightPlanActive = AircraftStatusSlow.GPSFlightPlanActive;
            _gpsNextWpLatitude = AircraftStatusSlow.GPSNextWPLatitude;
            _gpsNextWpLongitude = AircraftStatusSlow.GPSNextWPLongitude;
            _gpsPrevWpLatitude = AircraftStatusSlow.GPSPrevWPLatitude;
            _gpsPrevWpLongitude = AircraftStatusSlow.GPSPrevWPLongitude;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsSlowRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);

                /* null island ???
                 0.00040748246254171134 0.01397450300629543 
                 0.000407520306147189   0.01397450300629543 
                 */


                if (AircraftStatusSlow.Latitude != 0 && AircraftStatusSlow.Longitude != 0 &&
                    (_latitude != AircraftStatusSlow.Latitude || _longitude != AircraftStatusSlow.Longitude ||
                     _heading != AircraftStatusSlow.TrueHeading))
                {
                    _latitude = AircraftStatusSlow.Latitude;

                    _longitude = AircraftStatusSlow.Longitude;

                    _heading = AircraftStatusSlow.TrueHeading;

                    _gpsFlightPlanActive = AircraftStatusSlow.GPSFlightPlanActive;
                    _gpsNextWpLatitude = AircraftStatusSlow.GPSNextWPLatitude;
                    _gpsNextWpLongitude = AircraftStatusSlow.GPSNextWPLongitude;
                    _gpsPrevWpLatitude = AircraftStatusSlow.GPSPrevWPLatitude;
                    _gpsPrevWpLongitude = AircraftStatusSlow.GPSPrevWPLongitude;

                    await SetMapValues(
                        _latitude, 
                        _longitude, 
                        _heading,
                        _gpsFlightPlanActive,
                        _gpsNextWpLatitude,
                        _gpsNextWpLongitude,
                        _gpsPrevWpLatitude,
                        _gpsPrevWpLongitude);
                }

            });

            await hubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await InitMap();

                StateHasChanged();
            }
        }

        async Task SetMapValues(
            double latitude, 
            double longitude, 
            double heading, 
            bool gpsFlightPlanActive, 
            double gpsNextWpLatitude, 
            double gpsNextWpLongitude, 
            double gpsPrevWpLatitude, 
            double gpsPrevWpLongitude)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues", 
                latitude, 
                longitude,
                heading, 
                gpsFlightPlanActive, 
                gpsNextWpLatitude, 
                gpsNextWpLongitude, 
                gpsPrevWpLatitude, 
                gpsPrevWpLongitude);
        }

        async Task InitMap()
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
