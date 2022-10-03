using System.Security.Claims;

namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record ValidateToken(string Jwt) : IRequest<ApplicationResponse<ClaimsPrincipal>>;

public class ValidateTokenHandler : IRequestHandler<ValidateToken, ApplicationResponse<ClaimsPrincipal>>
{
    private readonly IIdentityService _identityService;
    public ValidateTokenHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<ApplicationResponse<ClaimsPrincipal>> Handle(ValidateToken request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_identityService.ValidateTokenAndGetClaimsPrincipal(request.Jwt) ?? new ApplicationResponse<ClaimsPrincipal>());
    }
}

