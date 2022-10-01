
using MyDemoProjects.Application.Shared.DTOs.Request;

namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginWithExternalAuthService(LoginExternalUserRequset ExternalUser) : IRequest<ApplicationResponse<TokenResponse>>;

public class LoginWithExternalAuthServiceHandler : IRequestHandler<LoginWithExternalAuthService, ApplicationResponse<TokenResponse>>
{
    private readonly IIdentityService _identityService;
    public LoginWithExternalAuthServiceHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResponse<TokenResponse>> Handle(LoginWithExternalAuthService request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginExternalUserAsync(request.ExternalUser);
    }
}



