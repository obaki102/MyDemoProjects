using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Constants;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MyDemoProjects.UI.Services.Authentication.Implementation
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly HttpClient _httpClient;
        private readonly ISender _mediator;
        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage,
                HttpClient httpClient,
                ISender mediator)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _httpClient = httpClient;
            _mediator = mediator;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authToken = string.Empty;
            try
            {
                var authTokenFromLocalStorage = await _protectedLocalStorage.GetAsync<string>(AppSecrets.LocalStorage.AuthToken);
                authToken = authTokenFromLocalStorage.Value;
            }
            catch (Exception)
            {
                await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.AuthToken);
            }

            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(AppSecrets.Bearer, authToken.Replace("\"", ""));
                principal = _mediator.Send(new ValidateToken(authToken)).Result.Data;
            }

            if (principal == null || string.IsNullOrEmpty(authToken))
            {
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
                return new AuthenticationState(new ClaimsPrincipal());
            }

            var state = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        //TODO : Creata a separate token provider.
        public async Task<string> GetAccessToken()
        {
            try
            {
                var authTokenFromLocalStorage = await _protectedLocalStorage.GetAsync<string>(AppSecrets.LocalStorage.AuthToken);
                return authTokenFromLocalStorage.Value ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
