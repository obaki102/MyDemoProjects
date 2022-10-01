
namespace MyDemoProjects.Application.Features.Authentication.Commands
{
    public record LoginWithJwtToken(LoginUserRequest User) : IRequest<ApplicationResponse<TokenResponse>>;

    public class LoginWithJwtTokenHandler : IRequestHandler<LoginWithJwtToken, ApplicationResponse<TokenResponse>>
    {
        private readonly IIdentityService _identityService;
        public LoginWithJwtTokenHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ApplicationResponse<TokenResponse>> Handle(LoginWithJwtToken request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginUserAsync(request.User.Email, request.User.Password);
        }
    }


}
