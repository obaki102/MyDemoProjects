﻿
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Text;

namespace MyDemoProjects.Server.Application.Features.Shared.Service
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

        }

        public async Task<HttpResponseMessage> GetResponse(HttpServiceOption options)
        {

            if (options.IsTokenRequired)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
            }

            return await _httpClient.GetAsync(options.Endpoint);

        }

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
                    properties.Add("client_id", _configuration.GetSection("MyDemoProjects:client_id").Value);
                    properties.Add("client_secret", _configuration.GetSection("MyDemoProjects:client_secret").Value);
                    properties.Add("code_verifier", _configuration.GetSection("MyDemoProjects:code_verifier").Value);
                    properties.Add("code", _configuration.GetSection("MyDemoProjects:code").Value);
                }
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.GetSection("MyDemoProjects:client_id").Value}:{_configuration.GetSection("MyDemoProjects:client_secret").Value}")));

                return await _httpClient.PostAsync(options.Endpoint, new FormUrlEncodedContent(properties));

            }
        }
    }
}
