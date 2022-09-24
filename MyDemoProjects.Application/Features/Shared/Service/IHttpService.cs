namespace MyDemoProjects.Application.Features.Shared.Service;

public interface IHttpService
{
    Task<HttpResponseMessage> GetResponse(HttpServiceOption options);
    Task<HttpResponseMessage> PostRequest(HttpServiceOption options);
}
