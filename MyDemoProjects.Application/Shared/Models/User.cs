using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfileUrl { get; set; } = string.Empty;
        public string NameIdentifier { get; set; } = string.Empty;
        public bool Status { get; set; } = false; // TODO implement api to track user status
        public char Initials
        {
            get
            {
                if (!string.IsNullOrEmpty(Email))
                    return Email.ToUpper()[0];

                return 'D';
            }
           
        }
    }
}
