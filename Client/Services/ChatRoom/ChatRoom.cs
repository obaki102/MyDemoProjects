using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace MyDemoProjects.Client.Services.ChatRoom
{
    public class ChatRoom : IChatRoom
    {

        private HubConnection _hubConnection;
        private NavigationManager _navigationManager;

        public ChatRoom(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }
        public List<MessagesDto> Messages { get; set; } = new List<MessagesDto>();

        public event Action BroadcastChanged;
        public async Task InitiateConnection()
        {

            string baseUrl = _navigationManager.BaseUri;

            string _hubUrl = baseUrl.TrimEnd('/') + "/chathub";
            _hubConnection = new HubConnectionBuilder()
              .ConfigureLogging(logging => {
                  logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
              })
              .WithUrl(_hubUrl)
              .Build();

            _hubConnection.On<string, string>("Broadcast", BroadcastMessage);

            await _hubConnection.StartAsync();

            await SendAsync($"[Notice joined chat room.");
        }

        public void BroadcastMessage(string username, string message)
        {
            Messages.Add(new MessagesDto(username, message));
            BroadcastChanged.Invoke();
        }


        public async Task DisconnectAsync()
        {
            await SendAsync($"[Notice]  left chat room.");

            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();

            _hubConnection = null;
        }

        public async Task SendAsync(string message)
        {
            await _hubConnection.SendAsync("Broadcast", "test", message);
            

        }
    }

}
