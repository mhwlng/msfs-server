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

        [Inject] public GarminG5ApbarModel GarminG5ApbarData { get; set; }

        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GarminG5APBAR.razor.js").AsTask();

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            _hubConnection.On("MsFsGarminG5APBARRefresh", async () =>
            {
                //InvokeAsync(StateHasChanged);
                var module = await ModuleReference;
                await module.InvokeVoidAsync("SetValues",

                    GarminG5ApbarData.Data.AutopilotMaster,
                    GarminG5ApbarData.Data.AutopilotAltitudeLock,
                    GarminG5ApbarData.Data.AutopilotHeadingLock,
                    GarminG5ApbarData.Data.AutopilotNav1Lock,
                    GarminG5ApbarData.Data.AutopilotFlightDirectorActive,
                    GarminG5ApbarData.Data.AutopilotBackcourseHold,
                    GarminG5ApbarData.Data.AutopilotVerticalHold,
                    GarminG5ApbarData.Data.AutopilotYawDamper,
                    GarminG5ApbarData.Data.AutopilotApproachHold

                );
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
