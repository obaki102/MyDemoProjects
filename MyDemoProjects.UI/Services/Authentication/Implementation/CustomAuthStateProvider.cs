using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Features.Authentication.Commands;
using System.Net.Http.Headers;
using System.Security.Claims;
using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Services.Authentication.Implementation
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly HttpClient _httpClient;
        private readonly ISender _mediator;

        private ClaimsPrincipal Principal { get; set; } = new ClaimsPrincipal();

        public User UserSettings { get; private set; } = new User();
        public string Status { get; private set; } = string.Empty;
        public bool IsAuthenticated { get; private set; } = false;
        public string NameIdentifier { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string ProfilePictureDataUrl { get; private set; } = string.Empty;
        public string Expiration { get; private set; } = string.Empty;

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

            if (principal == null)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
                return new AuthenticationState(new ClaimsPrincipal());
            }

            var state = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

      

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

        public async Task SetUserSettings()
        {
            try
            {
                var userSettings = new User
                {
                    Email = Email,
                    Name = Name,
                    NameIdentifier = NameIdentifier,
                    ProfileUrl = ProfilePictureDataUrl
                };

                UserSettings = userSettings;
                //  await _protectedLocalStorage.SetAsync(AppSecrets.LocalStorage.UserSettings, userSettings);
            }
            catch (Exception)
            {
                await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.UserSettings);
            }
        }

        public async Task<User> GetUserSettings()
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<User>(AppSecrets.LocalStorage.UserSettings);
                return result.Value ?? new User();
            }
            catch (Exception)
            {
                await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.UserSettings);
                return new User();
            }
        }

        public void UpdateProperties()
        {
            foreach (var claims in Principal.Claims)
            {
                switch (claims.Type)
                {
                    case ApplicationClaimTypes.Status:
                        Status = claims.Value;
                        break;
                    case ClaimTypes.NameIdentifier:
                        NameIdentifier = claims.Value;
                        break;
                    case ClaimTypes.Name:
                        Name = claims.Value;
                        break;
                    case ClaimTypes.Email:
                        Email = claims.Value;
                        break;
                    case ApplicationClaimTypes.ProfilePictureDataUrl:
                        ProfilePictureDataUrl = claims.Value;
                        break;
                    case ApplicationClaimTypes.Expiration:
                        Expiration = claims.Value;
                        break;
                }
            }
        }
    }
}
