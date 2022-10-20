using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.Models;
using System.Threading.Tasks;

namespace MyDemoProjects.AzureFunctions
{
    public static class SignalR
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
         [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
         [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("messages")]
        public static Task SendMessage(
         [HttpTrigger(AuthorizationLevel.Function, "post")] ChatMessage chatMessage,
         [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = HubHandler.ReceivedMessage,
                    Arguments = new[] { chatMessage }
                });
        }
    }
}
