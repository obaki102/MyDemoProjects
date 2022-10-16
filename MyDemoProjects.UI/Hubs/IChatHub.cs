using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string to, string from, string message);
        Task UserOnline(string onlineUser);
        Task UserOffline(string offlineUser);

    }
}
