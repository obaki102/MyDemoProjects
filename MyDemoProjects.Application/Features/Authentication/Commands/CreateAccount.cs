namespace MyDemoProjects.Application.Features.Authentication.Commands;

public class CreateAccount : IRequest<ApplicationResponse<bool>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

public class CreateAccountHandler : IRequestHandler<CreateAccount, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public CreateAccountHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }
    public async Task<ApplicationResponse<bool>> Handle(CreateAccount request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<ApplicationUser>(request);
        return await _identityService.CreateUserAsync(newUser, request.Password);
    }

}

