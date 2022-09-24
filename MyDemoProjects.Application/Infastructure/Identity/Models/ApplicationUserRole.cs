namespace MyDemoProjects.Application.Infastructure.Identity.Models;

public class ApplicationUserRole : IdentityUserRole<string>
{
    [Key]
    public Guid Id { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;

}
