﻿using System.ComponentModel.DataAnnotations;

namespace MyDemoProjects.Server.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Role Role { get; set; }
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
        //TO DO: Create custom role class
    }
}