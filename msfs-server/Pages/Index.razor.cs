using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using msfs_server.msfs;


namespace msfs_server.Pages
{
    public partial class Index : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private AircraftStatusModel AircraftStatus { get; set; }

        private HubConnection hubConnection;
        
        protected override async Task OnInitializedAsync()
        {
            
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();
           
            hubConnection.On("MsFsRefresh", () =>
            { 
              InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();
            
        }


        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }




    }
}
