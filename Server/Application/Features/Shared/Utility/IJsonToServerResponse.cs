namespace MyDemoProjects.Server.Application.Features.Shared.Utility
{
    public interface IJsonToServerResponse<T>
    {
        Task<ServerResponse<T>> Convert(HttpResponseMessage response);
    }
}
