using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Features.Authentication.Commands;
using System.Net.Http.Headers;
using System.Security.Claims;
using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly HttpClient _httpClient;
        private readonly ISender _mediator;

        private ClaimsPrincipal Principal { get; set; } = new ClaimsPrincipal();

        public UserSettings UserSettings { get; private set; } = new UserSettings();

        public string Status { get; private set; } = string.Empty;
        public bool IsAuthenticated { get; private set; } = false;
        public string NameIdentifier { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string ProfilePictureDataUrl { get; private set; } = string.Empty;
        public string Expiration { get; private set; } = string.Empty;

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

            if (principal == null)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
                return new AuthenticationState(new ClaimsPrincipal());
            }

            Principal = principal;
            UpdateProperties();
            await SetUserSettings();
            var state = new AuthenticationState(principal);
            IsAuthenticated = state.User.Identity.IsAuthenticated;
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        
        public async Task LogOutAndUpdateAuthenticationState()
        {
            await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.AuthToken);
            await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.UserSettings);
            await GetAuthenticationStateAsync();
        }

        public async Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt)
        {
            await _protectedLocalStorage.SetAsync(AppSecrets.LocalStorage.AuthToken, jwt);
            await GetAuthenticationStateAsync();
        }

        public async Task<string> GetAccessToken()
        {
            var authToken = string.Empty;
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
                var userSettings = new UserSettings
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

        public async Task<UserSettings> GetUserSettings()
        {
            try
            {
                var result = await _protectedLocalStorage.GetAsync<UserSettings>(AppSecrets.LocalStorage.UserSettings);
                return result.Value??new UserSettings();
            }
            catch (Exception)
            {
                await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.UserSettings);
                return new UserSettings();
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
