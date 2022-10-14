namespace MyDemoProjects.UI.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string to, string from, string message);
    }
}
