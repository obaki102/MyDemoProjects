using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyDemoProjects.Application.Infastructure.Hubs.Interface;
using MyDemoProjects.Application.Shared.Events;
using MyDemoProjects.Application.Shared.Models;
using Newtonsoft.Json;

namespace MyDemoProjects.Application.Infastructure.Hubs.Implementation
{
    public class ChatHubClient : IHubClient , IAsyncDisposable
    {
        private HubConnection? hubConnection;
        private readonly HubClientOptions _hubClientOptions;
        private bool isConnectionStarted = false;

        public ChatHubClient(IOptions<HubClientOptions>  hubClientOptions)
        {
            _hubClientOptions = hubClientOptions.Value;
        }

        public async Task ConnectAsync()
        {
            if (!isConnectionStarted)
            {
                hubConnection = new HubConnectionBuilder()
                               .WithUrl(_hubClientOptions.HubUrl)
                                .ConfigureLogging(logging =>
                                {
                                    logging.ClearProviders();
                                    logging.AddConsole();
                                })
                                     .AddMessagePackProtocol()
                                     .Build();


                hubConnection.On<object>(HubHandler.ReceivedMessage, (receivedMessage) =>
                {
                    var json = JsonConvert.SerializeObject(receivedMessage);
                    var chatMessage = JsonConvert.DeserializeObject<ChatMessage>(json);
                    ReceivedMessageHandler?.Invoke(this, new ChatMessageEventArgs { ChatMessage = chatMessage?? new() });
                });

                await hubConnection.StartAsync();
                isConnectionStarted = true;
            }
        }

        public async Task DisconnectAsync()
        {
            if (isConnectionStarted && hubConnection is not null)
            {
                await hubConnection.StopAsync();
                await hubConnection.DisposeAsync();
                hubConnection = null;
                isConnectionStarted = false;
            }
        }

        public event  EventHandler<ChatMessageEventArgs>? ReceivedMessageHandler;

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
        }
    }
}
