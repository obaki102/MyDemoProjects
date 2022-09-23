using Microsoft.AspNetCore.Identity;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public string? Description { get; set; }
        public string? Group { get; set; }
        public virtual ApplicationRole Role { get; set; } = default!;


    }
}
