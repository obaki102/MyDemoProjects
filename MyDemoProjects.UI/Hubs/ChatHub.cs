using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatHub>
    {
        public async override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.User.Identity.Name} Hub connected");
            await Clients.All.UserOnline(Context.User.Identity.Name);
        }
        public override async Task OnDisconnectedAsync(Exception? e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await Clients.All.UserOffline(Context.User.Identity.Name);
        }
        public async Task ReceiveMessage(ChatMessage chatMessage)
        {
            await Clients.All.ReceiveMessage(chatMessage);  
        }


    }
}
