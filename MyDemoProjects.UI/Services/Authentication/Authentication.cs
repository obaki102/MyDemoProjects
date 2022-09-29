using MediatR;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class Authentication : IAuthentication
    {
        private readonly IMediator _mediator;

        public Authentication(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser)
        {
            return await _mediator.Send(new CreateAccount(newUser));
        }

        public async Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser)
        {
            return await _mediator.Send(new Login(loginUser));
        }
    }
}
