using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models;

public class ApplicationUserRole : IdentityUserRole<string>
{
    [Key]
    public Guid Id { get; set; }
    public virtual ApplicationUser User1 { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;

}
