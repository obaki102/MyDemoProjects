using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Threading.Tasks;

namespace MyDemoProjects.AzureFunctions
{
    public static class SignalR
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
         [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("messages")]
        public static Task SendMessage(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post")] object chatMessage,
         [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newMessage",
                    Arguments = new[] { chatMessage }
                });
        }
    }
}
