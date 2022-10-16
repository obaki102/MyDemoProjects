using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace MyDemoProjects.UI.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatHub>
    {
        public async override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            Console.WriteLine($"{Context.User.Identity.Name} Hub connected");
            await Clients.All.UserOnline(Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await Clients.All.UserOffline(Context.User.Identity.Name);
            await base.OnDisconnectedAsync(e);
        }
        public async Task ReceiveMessage(string from, string to, string message)
        {
            Console.WriteLine($"Message from {from} to {to} : {message}");
            await Clients.Users(from, to).ReceiveMessage(from, to, message);  
        }


    }
}
