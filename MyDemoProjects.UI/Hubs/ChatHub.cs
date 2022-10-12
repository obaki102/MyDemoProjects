using Microsoft.AspNetCore.SignalR;

namespace MyDemoProjects.UI.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
        public async Task SendMessage(string from, string to, string message)
        {
            Console.WriteLine($"Mesasge from {from} to{to} : {message}");
            await Clients.All.SendAsync("SendMessage", from, to, message);
        }

    }
}
