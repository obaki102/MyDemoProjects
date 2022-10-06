

namespace MyDemoProjects.Application.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<CreateAccount, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserDetailsResponse>();
        }
    }
}
