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

namespace msfs_server.Components
{
    public partial class MovingMap
    {
        //[Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        private double _latitude;

        private double _longitude;

        private double _heading;

        [Parameter] public AircraftStatusModel AircraftStatus { get; set; }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/util.js").AsTask();

        protected override void OnInitialized()
        {
            _longitude = AircraftStatus.Longitude;
            _latitude = AircraftStatus.Latitude;
            _heading = AircraftStatus.TrueHeading;
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

        protected override async Task OnParametersSetAsync()
        {
            /* null island ???
             0.00040748246254171134 0.01397450300629543 
             0.000407520306147189   0.01397450300629543 
             */


            if (AircraftStatus.Latitude > 0 && AircraftStatus.Longitude > 0  && (_latitude != AircraftStatus.Latitude || _longitude != AircraftStatus.Longitude || _heading != AircraftStatus.TrueHeading))
            {
                _latitude = AircraftStatus.Latitude;

                _longitude = AircraftStatus.Longitude;

                _heading = AircraftStatus.TrueHeading;

                await SetMapCoordinates(_latitude, _longitude, _heading);

            }
        }

        async Task SetMapCoordinates(double latitude, double longitude, double heading)
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetMapCoordinates", latitude, longitude, heading);
        }

        async Task InitMap()
        {
            var module = await ModuleReference;
            await module.InvokeVoidAsync("InitMap");
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleReference != null)
            {
                var module = await _moduleReference;
                await module.DisposeAsync();
            }
        }
    }
}
