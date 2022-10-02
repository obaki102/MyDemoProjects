using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record ChangePassword(ChangePasswordRequest User) : IRequest<ApplicationResponse<bool>>;

public class ChangePasswordHandler : IRequestHandler<ChangePassword, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResponse<bool>> Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        return  await _identityService.ChangePasswordAsync(request.User.Email, request.User.CurrentPassword, request.User.NewPassword);
    }
}
