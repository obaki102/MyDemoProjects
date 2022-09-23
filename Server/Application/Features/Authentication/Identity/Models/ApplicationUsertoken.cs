using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models;

public class ApplicationUserToken : IdentityUserToken<string>
{
    [Key]
    public Guid Id { get; set; }
    public virtual ApplicationUser User1 { get; set; } = default!;
}

