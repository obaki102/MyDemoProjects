using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.DTOs.Response
{
    public record UserDetailsResponse
    {

        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }

    }

}
