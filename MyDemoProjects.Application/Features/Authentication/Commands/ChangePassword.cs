namespace MyDemoProjects.Application.Features.Authentication.Commands;

public class ChangePassword : IRequest<ApplicationResponse<bool>>
{
    public string EmailAddress { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

}

public class ChangePasswordHandler : IRequestHandler<ChangePassword, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ApplicationResponse<bool>> Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        return  await _identityService.ChangePasswordAsync(request.EmailAddress, request.CurrentPassword, request.NewPassword);
    }
}
