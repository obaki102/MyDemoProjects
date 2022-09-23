namespace MyDemoProjects.Server.Shared.Utility
{
    public interface IJsonToServerResponse<T>
    {
        Task<ServerResponse<T>> Convert(HttpResponseMessage response);
    }
}
