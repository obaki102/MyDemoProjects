using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.DTOs;
using MyDemoProjects.Application.Shared.Models.Response;
using MyDemoProjects.UI.Services.Authentication.Interface;

namespace MyDemoProjects.UI.Services.Authentication.Implementation
{
    public class Authentication : IAuthentication
    {
        private readonly IMediator _mediator;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly IConfiguration _config;
        public Authentication(IMediator mediator, AuthenticationStateProvider authenticationStateProvider, 
            IConfiguration config, ProtectedLocalStorage protectedLocalStorage)
        {
            _mediator = mediator;
            _authenticationStateProvider = authenticationStateProvider;
            _config = config;
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccount newUser)  
        {
            return await _mediator.Send(newUser);
        }

        public async Task<ApplicationResponse<bool>> LoginAsync(LoginUser loginUser)
        {
            var loginResponse = await _mediator.Send(loginUser);
            if (loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }

        public async Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUser externalLoginUser)
        {
            var loginResponse = await _mediator.Send(externalLoginUser);
            if (loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }
            await SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }

        public Task<GoogleAuth2Config> GetGoogleExternalAuthConfig()
        {
            var buildConfig = new GoogleAuth2Config
            {
                AccessToken = Auth2Config.AccessToken,
                ClientId = _config.GetSection(AppSecrets.GoogleClientId).Value,
                Scope = Auth2Config.Scope,
                DiscoveryDocs = Auth2Config.DiscoveryDocs
            };
            return Task.FromResult(buildConfig);
        }

        public async Task LogOutAndUpdateAuthenticationState()
        {
            await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.AuthToken);
            await _protectedLocalStorage.DeleteAsync(AppSecrets.LocalStorage.UserSettings);
            await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        public async Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt)
        {
            await _protectedLocalStorage.SetAsync(AppSecrets.LocalStorage.AuthToken, jwt);
            await _authenticationStateProvider.GetAuthenticationStateAsync();
        }
    }
}
