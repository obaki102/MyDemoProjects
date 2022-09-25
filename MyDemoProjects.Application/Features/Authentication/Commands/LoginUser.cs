namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginUser(LoginUserRequest User) : IRequest<ApplicationResponse<bool>>;

public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;

    public LoginUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<ApplicationResponse<bool>> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginUserAsync(request.User.Email, request.User.Password);
    }
}


