using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Constants;
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
            var state = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        public async Task LogOutAndUpdateAuthenticationState()
        {
            await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.AuthToken);
            await GetAuthenticationStateAsync();
        }

        public async Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt)
        {
            await _protectedLocalStorage.SetAsync(AppSecrets.LocalStorage.AuthToken, jwt);
            await GetAuthenticationStateAsync();
        }
    }
}
