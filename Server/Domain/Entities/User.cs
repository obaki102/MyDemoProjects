namespace MyDemoProjects.Server.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
      
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
        //TO DO: Create custom role class
    }
}
