namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record CreateAccount(CreateAccountRequest User) : IRequest<ApplicationResponse<bool>>;

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
        var newUser = _mapper.Map<ApplicationUser>(request.User);
        return await _identityService.CreateUserAsync(newUser, request.User.Password);
    }

}

