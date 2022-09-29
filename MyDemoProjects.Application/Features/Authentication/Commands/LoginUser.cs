using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginUser(LoginUserRequest User) : IRequest<ApplicationResponse<bool>>;

public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

    public LoginUserHandler(IIdentityService identityService, CustomAuthenticationStateProvider customAuthenticationStateProvider)
    {
        _identityService = identityService;
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
    }
    public async Task<ApplicationResponse<bool>> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        var isLoginSuccess = await _identityService.LoginUserAsync(request.User.Email, request.User.Password);
        if (isLoginSuccess.IsSuccess)
        {
            _customAuthenticationStateProvider.NotifyAuthenticationStateChanged();
        }
        return isLoginSuccess;
    }
}


