using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Models
{
    public class ChatMessage
    {
        private DateTime _createDate;

        public ChatMessage()
        {
            _createDate = DateTime.Now;
        }
     
        public User User { get; set; } = new User();
        public string Message { get; set; } = string.Empty;

        public  DateTime MessageCreateDate { get => _createDate; }
        //TO DO
        public string Gif { get; set; } = string.Empty;

    }
}
