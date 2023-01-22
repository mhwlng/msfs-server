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
    public partial class GarminG5
    {
        //[Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        private double _bankdegrees;

        private double _pitchdegrees;

        [Parameter] public AircraftStatusModel AircraftStatus { get; set; }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/garming5.js").AsTask();

        protected override void OnInitialized()
        {
            _bankdegrees = AircraftStatus.BankDegrees;
            _pitchdegrees = AircraftStatus.PitchDegrees;
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

        protected override async Task OnParametersSetAsync()
        {

            if (_bankdegrees != AircraftStatus.BankDegrees ||
                _pitchdegrees != AircraftStatus.PitchDegrees)
            {
                _bankdegrees = AircraftStatus.BankDegrees;
                _pitchdegrees = AircraftStatus.PitchDegrees;

                await SetG5Values(_bankdegrees, _pitchdegrees);
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
