namespace MyDemoProjects.Application.Features.Authentication.Commands
{
    public class LoginUser: IRequest<ApplicationResponse<TokenResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationResponse<TokenResponse>>
    {
        private readonly IIdentityService _identityService;
        public LoginUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ApplicationResponse<TokenResponse>> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginUserAsync(request.Email, request.Password);
        }
    }


}
