
namespace MyDemoProjects.Application.Infastructure.Identity.Models;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    [Key]
    public Guid Id { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
}

