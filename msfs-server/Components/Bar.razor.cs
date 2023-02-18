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
using msfs_server.Helpers;
using System.Xml.Linq;
using System.Reflection;
using MudBlazor;
using System.Drawing;

namespace msfs_server.Components
{
    public partial class Bar
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public AircraftStatusFastModel AircraftStatusFast { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public string BarRangesJson { get; set; }
        
        [Parameter] public double RangeMin { get; set; }

        [Parameter] public double RangeMax { get; set; }

        [Parameter] public double RulerWidth { get; set; }


        [Parameter] public System.Drawing.Color Color { get; set; }



        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Bar.razor.js").AsTask();

      
        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsFastRefresh", async () =>
            {
                await SetValues(
                    Value,
                    RangeMin,
                    RangeMax,
                    Color);

            });

            await hubConnection.StartAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Init(
                    Value,
                    Label,
                    RangeMin,
                    RangeMax,
                    RulerWidth,
                    Color,
                    BarRangesJson
                );

                //StateHasChanged();

            }
        }

        async Task SetValues(string value,
                             double rangeMin,
                             double rangeMax,
                             System.Drawing.Color color)
        {
            var id = $"bar_{value}";

            var val = (double)(AircraftStatusFast.GetType().GetProperty(Value)?.GetValue(AircraftStatusFast, null) ?? 0);

            var module = await ModuleReference;
            await module.InvokeVoidAsync("SetValues",
                id,
                rangeMin,
                rangeMax,
                ColorTranslator.ToHtml(color),
                val
                );

        }

        async Task Init(string value, 
                        string label,
                        double rangeMin,
                        double rangeMax,
                        double rulerWidth,
                        System.Drawing.Color color,
                        string barRangesFileName)
        {
            var id = $"bar_{value}";

            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init",
                id,
                label,
                rangeMin,
                rangeMax,
                rulerWidth,
                ColorTranslator.ToHtml(color),
                barRangesFileName);
        }

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
