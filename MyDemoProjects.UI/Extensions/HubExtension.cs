﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.UI.Services.Authentication.Implementation;

namespace MyDemoProjects.UI.Extensions
{
    public static class HubExtension
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager, CustomAuthStateProvider tokenProvider)
        {
            if (hubConnection == null)
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
