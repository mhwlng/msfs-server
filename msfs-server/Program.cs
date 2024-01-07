using System;
using System.Globalization;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor.Services;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Threading;

namespace msfs_server
{
    class Program
    {
 
        [STAThread]
        public static int Main(string[] args)
        {

            Common.Startup(args);

            var app = new App
            {
                StartupUri = new Uri("MainWindow.xaml", UriKind.Relative)
            };
            app.Run();

            return 0;
        }

    }

}