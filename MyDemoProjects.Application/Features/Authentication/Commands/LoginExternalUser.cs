
namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginExternalUser : IRequest<ApplicationResponse<TokenResponse>>
{
    public string Provider { get; init; } = string.Empty;
    public string EmailAddress { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string PictureUrl { get; init; } = string.Empty;
}

public class LoginExternalUserHandler : IRequestHandler<LoginExternalUser, ApplicationResponse<TokenResponse>>
{
    private readonly IIdentityService _identityService;
    public LoginExternalUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResponse<TokenResponse>> Handle(LoginExternalUser request, CancellationToken cancellationToken)
    {
        return await _identityService.LoginExternalUserAsync(request);
    }
}



