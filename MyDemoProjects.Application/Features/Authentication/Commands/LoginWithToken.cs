namespace MyDemoProjects.Application.Features.Authentication.Commands
{
    public record LoginWithToken(LoginUserRequest User) : IRequest<ApplicationResponse<TokenResponse>>;

    public class LoginWithJwtTokenHandler : IRequestHandler<LoginWithToken, ApplicationResponse<TokenResponse>>
    {
        private readonly IIdentityService _identityService;
        public LoginWithJwtTokenHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ApplicationResponse<TokenResponse>> Handle(LoginWithToken request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginUserAsync(request.User.Email, request.User.Password);
        }
    }


}
