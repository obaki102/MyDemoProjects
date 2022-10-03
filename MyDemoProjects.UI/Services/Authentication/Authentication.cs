using MediatR;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;
using MyDemoProjects.Application.Shared.Models.Security;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IMediator _mediator;
        private readonly CustomAuthStateProvider customAuthStateProvider;
        public Authentication(IMediator mediator, CustomAuthStateProvider customAuthStateProvider)
        {
            _mediator = mediator;
            this.customAuthStateProvider = customAuthStateProvider;
        }

        public async Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser)
        {
            return await _mediator.Send(new CreateAccount(newUser));
        }

        public async Task<ApplicationResponse<bool>> LoginAsync(LoginFormModel loginUser)
        {
            var loginResponse = await _mediator.Send(new LoginWithToken(loginUser));
            if(loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await customAuthStateProvider.SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }

        public async Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUserRequset externalLoginUser)
        {
            var loginResponse = await _mediator.Send(new LoginWithExternalAuthService(externalLoginUser));
            if (loginResponse.IsSuccess is false || loginResponse.Data is null)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await customAuthStateProvider.SaveJwtToLocalStorageAndUpdateAuthenticationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }
    }
}
