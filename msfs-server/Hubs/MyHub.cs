using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace msfs_server.Hubs
{
    public static class UserHandler
    {
        public static readonly HashSet<string> ConnectedIds = [];
    }


    public class MyHub : Hub
    {
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);

            await base.OnConnectedAsync();
        }

    }
}
