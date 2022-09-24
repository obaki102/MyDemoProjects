namespace MyDemoProjects.UI.Services.ChatRoom
{
    public interface IChatRoom
    {
        Task InitiateConnection();
        void BroadcastMessage(string name, string message);
        Task SendAsync(string message);
        Task DisconnectAsync();
        // List<MessagesDto> Messages { get; set; }

        event Action BroadcastChanged;
    }
}
