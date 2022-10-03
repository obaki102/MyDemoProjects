namespace MyDemoProjects.Application.Features.Shared.Service;
/// <summary>
/// Contract for HttpService.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Send an asynchronous GET request.  
    /// </summary>
    /// <param name="options">Options contains the settings on how the GET request will be send.</param>
    /// <returns>A  asynchronous task of HttpResponseMessage.</returns>
    Task<HttpResponseMessage> GetResponse(HttpServiceOption options);
    /// <summary>
    /// Send an asynchronous POST  request. 
    /// </summary>
    /// <param name="options">Options contains the settings on how the POST request will be send</param>
    /// <returns>>A  asynchronous task of HttpResponseMessage.</returns>
    Task<HttpResponseMessage> PostRequest(HttpServiceOption options);
}
