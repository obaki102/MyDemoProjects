namespace MyDemoProjects.Application.Features.Authentication.Queries
{
    public record GetAllUsers() : IRequest<ApplicationResponse<UserDetailsResponse>>;
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, ApplicationResponse<UserDetailsResponse>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetAllUsersHandler(IIdentityService identityServvice, IMapper mapper)
        {
            _identityService = identityServvice;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse<UserDetailsResponse>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            var usersMappedToDto = _mapper.Map<IEnumerable<UserDetailsResponse>>(users.ListOfData);
            return ApplicationResponse<UserDetailsResponse>.Success(usersMappedToDto);
        }

      
    }


}
