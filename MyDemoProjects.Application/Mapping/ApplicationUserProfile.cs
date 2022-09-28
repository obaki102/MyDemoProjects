using MyDemoProjects.Application.Shared.DTO;
using MyDemoProjects.Application.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Mapping
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<CreateAccountRequest, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserDetailsResponse>();
        }
    }
}
