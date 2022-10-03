namespace MyDemoProjects.Application.Features.Shared.Service;
/// <summary>
/// Contract for HttpService.
/// </summary>
public interface IHttpService
{
    Task<HttpResponseMessage> GetResponse(HttpServiceOption options);
    Task<HttpResponseMessage> PostRequest(HttpServiceOption options);
}
