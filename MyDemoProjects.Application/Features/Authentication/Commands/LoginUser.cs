namespace MyDemoProjects.Application.Features.Authentication.Commands
{
    public class LoginUser: IRequest<ApplicationResponse<TokenResponse>>
    {
        public string EmailAddress { get; set; } = string.Empty;
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
            return await _identityService.LoginUserAsync(request.EmailAddress, request.Password);
        }
    }


}
