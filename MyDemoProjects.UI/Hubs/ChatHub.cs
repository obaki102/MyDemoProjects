using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MyDemoProjects.Application.Shared.Models;
using System.Security.Claims;

namespace MyDemoProjects.UI.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatHub>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
            Console.WriteLine($"{Context.User.Identity.Name} connected");
            Console.WriteLine($"{Context.User?.FindFirst(ClaimTypes.Email)?.Value!} * *** connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
        public async Task ReceiveMessage(string from, string to, string message)
        {
            Console.WriteLine($"Message from {from} to {to} : {message}");
            await Clients.Users(from, to).ReceiveMessage(from, to, message);  
        }

        public async Task OnlineUsers(UserSettings onlineUser)
        {
            Console.WriteLine($"{onlineUser.Email} is online");
            await Clients.All.OnlineUsers(onlineUser);
        }

    }
}
