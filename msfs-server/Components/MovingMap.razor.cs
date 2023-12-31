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

        private double? _latitude;
        private double? _longitude;
        private double? _heading;
        private bool? _gpsFlightPlanActive;
        private double? _gpsNextWpLatitude;
        private double? _gpsNextWpLongitude;
        private double? _gpsPrevWpLatitude;
        private double? _gpsPrevWpLongitude;

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/MovingMap.razor.js").AsTask();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
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


                if (AircraftStatusSlow.StatusSlow.Latitude != 0 && AircraftStatusSlow.StatusSlow.Longitude != 0 &&
                    (_latitude != AircraftStatusSlow.StatusSlow.Latitude ||
                     _longitude != AircraftStatusSlow.StatusSlow.Longitude ||
                     _heading != AircraftStatusSlow.StatusSlow.TrueHeading ||
                     _gpsFlightPlanActive != AircraftStatusSlow.StatusSlow.GPSFlightPlanActive ||
                     _gpsNextWpLatitude != AircraftStatusSlow.StatusSlow.GPSNextWPLatitude ||
                     _gpsNextWpLongitude != AircraftStatusSlow.StatusSlow.GPSNextWPLongitude ||
                     _gpsPrevWpLatitude != AircraftStatusSlow.StatusSlow.GPSPrevWPLatitude ||
                     _gpsPrevWpLongitude != AircraftStatusSlow.StatusSlow.GPSPrevWPLongitude))
                {
                    _latitude = AircraftStatusSlow.StatusSlow.Latitude;

                    _longitude = AircraftStatusSlow.StatusSlow.Longitude;

                    _heading = AircraftStatusSlow.StatusSlow.TrueHeading;

                    _gpsFlightPlanActive = AircraftStatusSlow.StatusSlow.GPSFlightPlanActive;
                    _gpsNextWpLatitude = AircraftStatusSlow.StatusSlow.GPSNextWPLatitude;
                    _gpsNextWpLongitude = AircraftStatusSlow.StatusSlow.GPSNextWPLongitude;
                    _gpsPrevWpLatitude = AircraftStatusSlow.StatusSlow.GPSPrevWPLatitude;
                    _gpsPrevWpLongitude = AircraftStatusSlow.StatusSlow.GPSPrevWPLongitude;

                    await SetValues(
                        AircraftStatusSlow.StatusSlow.Latitude,
                        AircraftStatusSlow.StatusSlow.Longitude,
                        AircraftStatusSlow.StatusSlow.TrueHeading,
                        AircraftStatusSlow.StatusSlow.GPSFlightPlanActive,
                        AircraftStatusSlow.StatusSlow.GPSNextWPLatitude,
                        AircraftStatusSlow.StatusSlow.GPSNextWPLongitude,
                        AircraftStatusSlow.StatusSlow.GPSPrevWPLatitude,
                        AircraftStatusSlow.StatusSlow.GPSPrevWPLongitude);
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
