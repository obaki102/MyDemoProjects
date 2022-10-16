using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string from, ChatMessage message);
        Task UserOnline(string onlineUser);
        Task UserOffline(string offlineUser);

    }
}
