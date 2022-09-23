using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        
        public string? Description { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ApplicationRole() : base()
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
        }
    }
}
