using MyDemoProjects.Application.Shared.Models;
using MyDemoProjects.Application.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Authentication.Queries
{
    public record GetAllUsers() : IRequest<ApplicationResponse<UserDetailsResponse>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, ApplicationResponse<UserDetailsResponse>>
    {
        private readonly IIdentityService _identityServvice;
        private readonly IMapper _mapper;
        public GetAllUsersHandler(IIdentityService identityServvice, IMapper mapper)
        {
            _identityServvice = identityServvice;
            _mapper = mapper;
        }

        public async Task<ApplicationResponse<UserDetailsResponse>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _identityServvice.GetAllUsersAsync();
            var usersMappedToDto = _mapper.Map<IEnumerable<UserDetailsResponse>>(users.ListOfData);

            return ApplicationResponse<UserDetailsResponse>.Success(usersMappedToDto);
        }

      
    }


}
