namespace MyDemoProjects.Application.Infastructure.Identity.Models;

public class ApplicationUserToken : IdentityUserToken<string>
{
    [Key]
    public Guid Id { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
}

