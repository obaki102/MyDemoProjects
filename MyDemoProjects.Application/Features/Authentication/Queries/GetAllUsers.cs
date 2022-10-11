namespace MyDemoProjects.Application.Features.Authentication.Queries
{
    public record GetAllUsers() : IRequest<ApplicationResponse<IEnumerable<UserDetailsResponse>>>;
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, ApplicationResponse<IEnumerable<UserDetailsResponse>>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetAllUsersHandler(IIdentityService identityServvice, IMapper mapper)
        {
            _identityService = identityServvice;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse<IEnumerable<UserDetailsResponse>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            if (users.IsSuccess is false)
            {
                return ApplicationResponse<IEnumerable<UserDetailsResponse>>.Fail();
            }
            var usersMappedToDto = _mapper.Map<IEnumerable<UserDetailsResponse>>(users.Data);
            return ApplicationResponse<IEnumerable<UserDetailsResponse>>.Success(usersMappedToDto);
        }

      
    }


}
