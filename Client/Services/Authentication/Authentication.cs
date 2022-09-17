
using static System.Net.WebRequestMethods;
using System.Net;
using System.Net.Http.Json;

namespace MyDemoProjects.Client.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly HttpClient _httpClient;
        public Authentication(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServerResponse<string>> AuthenticateAsync(LoginUserRequest loginUserRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/Authentication/api/loginUser", loginUserRequest);
            return response.Content
                .ReadFromJsonAsync<ServerResponse<string>>().Result;
        }
    }
}
