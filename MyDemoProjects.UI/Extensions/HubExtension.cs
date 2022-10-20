using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.UI.Services.Authentication.Implementation;

namespace MyDemoProjects.UI.Extensions
{
    public static class HubExtension
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager, CustomAuthStateProvider tokenProvider, bool isAzureHub)
        {
            if (hubConnection == null)
            {
                if(isAzureHub)
                hubConnection = new HubConnectionBuilder()
                                  .WithUrl("https://mydemoprojectsfunction.azurewebsites.net/api/?Code=eo8KJcUhAJqIjNjhAaFStXRKfJSDi3AxYRT_F530PKlZAzFuEZBPcQ==")
                                 //.WithUrl("http://localhost:7153/api/")
                                  .ConfigureLogging(logging =>
                                  {
                                      logging.ClearProviders();
                                      logging.AddConsole();
                                  })
                                  .WithAutomaticReconnect()
                                  .Build();
            }
            else
            {
                hubConnection = new HubConnectionBuilder()
                                .WithUrl(navigationManager.ToAbsoluteUri(HubConstants.ChatHubUrl), options =>
                                {
                                    options.AccessTokenProvider = async () =>
                                    {
                                        Console.WriteLine(navigationManager.ToAbsoluteUri(HubConstants.ChatHubUrl));
                                        var accessTokenResult = await tokenProvider.GetAccessToken();
                                        return accessTokenResult;
                                    };
                                })
                                .ConfigureLogging(logging =>
                                {
                                    logging.ClearProviders();
                                    logging.AddConsole();
                                })
                                .AddMessagePackProtocol()
                                .WithAutomaticReconnect()
                                .Build();
            }
            return hubConnection;
        }
    }
}
