using Microsoft.AspNetCore.Identity;

namespace MyDemoProjects.Server.Application.Features.Authentication.Identity.Models;

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public string? Description { get; set; }
    public virtual ApplicationUser User1 { get; set; } = default!;



}
