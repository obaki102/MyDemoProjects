using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MyDemoProjects.Application.Shared.Constants;

namespace MyDemoProjects.UI.Extensions
{
    public static class HubExtension
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(navigationManager.ToAbsoluteUri(HubConstants.ChatHubUrl))
                                  .Build();
            }
            return hubConnection;
        }
    }
}
