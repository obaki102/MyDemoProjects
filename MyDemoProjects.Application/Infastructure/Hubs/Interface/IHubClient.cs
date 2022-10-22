using MyDemoProjects.Application.Shared.Events;
using MyDemoProjects.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Infastructure.Hubs.Interface
{
    public interface IHubClient
    {
        Task ConnectAsync();
        Task DisconnectAsync();

        event EventHandler<ChatMessageEventArgs>? ReceivedMessageHandler;

    }
}
