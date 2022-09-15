using Microsoft.EntityFrameworkCore;

namespace MyDemoProjects.Server.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
