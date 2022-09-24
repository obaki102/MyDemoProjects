namespace MyDemoProjects.Application.Infastructure.Identity.Models;

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public string? Description { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;



}
