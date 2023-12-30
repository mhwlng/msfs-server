using System.IO;
using System.Linq;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using System.Reflection;
using System.Windows.Controls;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using msfs_server.Hubs;
using System.Diagnostics;
using msfs_server.msfs;
using Microsoft.FlightSimulator.SimConnect;
using msfs_server.MQTT;
using msfs_server.Services;

namespace msfs_server
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices();

            services.AddSingleton<AircraftStatusSlowModel>();
            services.AddSingleton<AircraftStatusFastModel>();
            services.AddSingleton<Mqtt>();


            if (!Debugger.IsAttached)
            {
                var configureOptions = new[]{"application/octet-stream"};

                services.AddResponseCompression(opts =>
                {
                    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        configureOptions);
                });
            }

            services.AddHostedService<Worker>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!Debugger.IsAttached)
            {
                app.UseResponseCompression();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Common.ExePath, @"wwwroot")),
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<MyHub>("/myhub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
