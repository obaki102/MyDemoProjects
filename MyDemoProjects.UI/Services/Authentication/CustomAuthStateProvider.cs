using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Models.Request;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly HttpClient _httpClient;
        private readonly ISender _mediator;

        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage, HttpClient httpClient, ISender mediator)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _httpClient = httpClient;
            _mediator = mediator;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var authToken = await _protectedLocalStorage.GetAsync<string>("auth_Token");
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken.Value))
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken.Value.Replace("\"", ""));
                }
                catch
                {
                    await _protectedLocalStorage.DeleteAsync("auth_Token");

                }

                principal = _mediator.Send(new ValidateToken(authToken.Value)).Result.Data;
            }
            var state = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        public async Task LogOutAndUpdateAuthenticationState()
        {
            await _protectedLocalStorage.DeleteAsync("auth_Token");
            await GetAuthenticationStateAsync();
        }

        public async Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt)
        {
            await _protectedLocalStorage.SetAsync("auth_Token", jwt);
            await GetAuthenticationStateAsync();

        }
    }
}
