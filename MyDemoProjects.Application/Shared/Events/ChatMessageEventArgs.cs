using EllipticCurve.Utils;
using MyDemoProjects.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; set; } = new();
    }
}
