using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PW.Hubs
{
    public class LoopyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("Send", $"{Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }

        public Task Send(string data)
        {
            return Clients.All.SendAsync("Send", data);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Clients.All.SendAsync("Send", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
