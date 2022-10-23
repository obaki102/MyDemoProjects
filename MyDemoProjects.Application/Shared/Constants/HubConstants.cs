using MyDemoProjects.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Constants
{
    public static class HubConstants
    {
        public const string ChatHubUrl = "/chathub";
    }
    public static class HubHandler
    {
        public const string ReceivedMessage = "ReceiveMessage";
        public const string UserOnline = "UserOnline";
        public const string UserOffline = "UserOffline";
    }

}
