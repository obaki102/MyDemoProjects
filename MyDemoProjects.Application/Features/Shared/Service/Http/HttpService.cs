using System.Net.Http.Headers;
using System.Text;

namespace MyDemoProjects.Application.Features.Shared.Service;
/// <summary>
/// A service that sends a customized HTTP request based on the defined  HttpServiceOptions
/// </summary>
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
      
    }
    /// <summary>
    /// Send an asynchronous GET request.  
    /// </summary>
    /// <param name="options">Options contains the settings on how the GET request will be send.</param>
    /// <returns>A  asynchronous task of HttpResponseMessage.</returns>
    public async Task<HttpResponseMessage> GetResponse(HttpServiceOption options)
    {
        if (options.IsTokenRequired)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
        }
        if(options.IsAnimeList)
        {
            _httpClient.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", _configuration.GetSection("client_id").Value);
        }

        return await _httpClient.GetAsync(options.Endpoint);

    }

    /// <summary>
    /// Send an asynchronous POST  request. 
    /// </summary>
    /// <param name="options">Options contains the settings on how the POST request will be send</param>
    /// <returns>>A  asynchronous task of HttpResponseMessage.</returns>
    public async Task<HttpResponseMessage> PostRequest(HttpServiceOption options)
    {
        {
            var properties = new Dictionary<string, string>
             {
                {"grant_type" ,options.IsRefreshToken ? "refresh_token": "authorization_code"},
                {options.IsRefreshToken ? "refresh_token": "code" , options.IsRefreshToken ? options.Token : options.Code},

             };

           if(!options.IsRefreshToken)
            {
                properties.Add("client_id", _configuration.GetSection("client_id").Value);
                properties.Add("client_secret", _configuration.GetSection("client_secret").Value);
                properties.Add("code_verifier", _configuration.GetSection("code_verifier").Value);
                properties.Add("code", _configuration.GetSection("code").Value);
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetSection("client_id").Value}:{_configuration.GetSection("client_secret").Value}")));

            return await _httpClient.PostAsync(options.Endpoint, new FormUrlEncodedContent(properties));

        }
    }
}
