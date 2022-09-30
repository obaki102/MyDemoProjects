using MediatR;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

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

        public async Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser)
        {
            var loginResponse = await _mediator.Send(new LoginWithJwtToken(loginUser));
            if(loginResponse.IsSuccess is false)
            {
                return ApplicationResponse<bool>.Fail(loginResponse.Messages);
            }

            await customAuthStateProvider.SaveJwtToLocalStorageAndUpadteNotificationState(loginResponse.Data.Token);
            return ApplicationResponse<bool>.Success(loginResponse.Messages);
        }
    }
}
