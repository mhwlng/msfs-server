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
    public partial class Dial
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] public GarminG5Model DialData { get; set; }

        [Inject] private IJSRuntime MyJsRuntime { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public string DialRangesJson { get; set; }
        
        [Parameter] public double RangeMin { get; set; }

        [Parameter] public double RangeMax { get; set; }

        [Parameter] public double MinAngle { get; set; }

        [Parameter] public double MaxAngle { get; set; }

        [Parameter] public double InnerRadius { get; set; }

        [Parameter] public double OuterRadius { get; set; }

        [Parameter] public System.Drawing.Color Color { get; set; }



        private Task<IJSObjectReference> _moduleReference;
        private Task<IJSObjectReference> ModuleReference => _moduleReference ??= MyJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Dial.razor.js").AsTask();

      
        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/myhub"))
                .Build();

            hubConnection.On("MsFsGarminG5Refresh", async () =>
            {
                var id = $"dial_{Value}";

                var val = (double)(DialData.GetType().GetProperty(Value)?.GetValue(DialData, null) ?? 0);

                var module = await ModuleReference;
                await module.InvokeVoidAsync("SetValues",
                    id,
                    RangeMin,
                    RangeMax,
                    MinAngle,
                    MaxAngle,
                    InnerRadius,
                    OuterRadius,
                    ColorTranslator.ToHtml(Color),
                    val
                );

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
                    MinAngle,
                    MaxAngle,
                    InnerRadius,
                    OuterRadius,
                    Color,
                    DialRangesJson
                );

                //StateHasChanged();

            }
        }


        async Task Init(string value, 
                        string label,
                        double rangeMin,
                        double rangeMax,
                        double minAngle,
                        double maxAngle,
                        double innerRadius,
                        double outerRadius,
                        System.Drawing.Color color,
                        string dialRangesFileName)
        {
            var id = $"dial_{value}";

            var module = await ModuleReference;
            await module.InvokeVoidAsync("Init",
                id,
                label,
                rangeMin,
                rangeMax,
                minAngle,
                maxAngle,
                innerRadius,
                outerRadius,
                ColorTranslator.ToHtml(color),
                dialRangesFileName);
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
