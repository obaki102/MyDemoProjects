using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyDemoProjects.Client.Pages.Login;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? DisplayName { get; set; }
        public string? ProfilePictureDataUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsLive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public ApplicationUser() : base()
        {
            UserClaims = new HashSet<ApplicationUserClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
            Logins = new HashSet<ApplicationUserLogin>();
            Tokens = new HashSet<ApplicationUserToken>();
        }
    }
}
