using MediatR;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.DTOs;
using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IMediator _mediator;
        private readonly CustomAuthStateProvider _customAuthStateProvider;
        private readonly IConfiguration _config;
        public Authentication(IMediator mediator, CustomAuthStateProvider customAuthStateProvider, IConfiguration config)
        {
            _mediator = mediator;
            _customAuthStateProvider = customAuthStateProvider;
            _config = config;
        }

        public async Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser)
        {
            return await _mediator.Send(new CreateAccount(newUser));
        }

        public async Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser)
        {
            var loginResponse = await _mediator.Send(new LoginWithToken(loginUser));
            if(loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await _customAuthStateProvider.SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }

        public async Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUserRequset externalLoginUser)
        {
            var loginResponse = await _mediator.Send(new LoginWithExternalAuthService(externalLoginUser));
            if (loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await _customAuthStateProvider.SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }

        public  Task<GoogleAuth2Config> GetGoogleExternalAuthConfig()
        {
            var buildConfig = new GoogleAuth2Config {
                AccessToken = "access_token",
                ClientId = _config.GetSection("google_client_id").Value,
                Scope = "https://www.googleapis.com/auth/userinfo.email",
                DiscoveryDocs = "https://people.googleapis.com/$discovery/rest?version=v1"
            };
            return Task.FromResult(buildConfig);
        }
    }
}
