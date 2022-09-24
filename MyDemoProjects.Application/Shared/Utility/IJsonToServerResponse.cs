namespace MyDemoProjects.Server.Shared.Utility
{
    public interface IJsonToServerResponse<T>
    {
        Task<ApplicationResponse<T>> Convert(HttpResponseMessage response);
    }
}
