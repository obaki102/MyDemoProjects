using System.ComponentModel.DataAnnotations;

namespace MyDemoProjects.Server.Domain.Entities
{
    public class Token
    {
        [Key]
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } =  string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime DateCreated { get; set; }
        public TimeSpan TokenExpiresIn => TimeSpan.FromSeconds(ExpiresIn);
    }
}
