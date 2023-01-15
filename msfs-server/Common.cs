using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;

namespace msfs_server
{
    public static class Common
    {
        public static string ExePath = null!;
        public static IConfigurationRoot ConfigurationRoot = null!;
  
        public static string GetExePath()
        {
            var strExeFilePath = Assembly.GetEntryAssembly()?.Location;
            return Path.GetDirectoryName(strExeFilePath) ?? "";
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configurationRoot) =>

            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        if (Debugger.IsAttached)
                        {
                            serverOptions.Listen(IPAddress.Loopback, 0);
                        }

                        var externalPort = configurationRoot.GetValue<int>("ExternalPort");

                        if (externalPort > 0)
                        {
                            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                            {
                                if (!item.Description.Contains("virtual", StringComparison.CurrentCultureIgnoreCase) &&
                                    item.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                    item.OperationalStatus == OperationalStatus.Up)
                                {
                                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                                    {
                                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                        {
                                            serverOptions.Listen(ip.Address, externalPort);
                                        }
                                    }
                                }
                            }
                        }

                    });
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });

        public static void Startup(string[] args)
        {
            ExePath = GetExePath();

            Directory.SetCurrentDirectory(ExePath);

            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ConfigurationRoot)
                .CreateLogger();
            
            Log.Information("Starting");

            var host = Common.CreateHostBuilder(args, Common.ConfigurationRoot).Build();
            
            host.RunAsync();

        }
    }
}
