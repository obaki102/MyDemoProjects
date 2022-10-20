using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.Models;
using System.Net.Http;
using System.Text.Json;
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
        public static async Task SendMessage(
         [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
         [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            var jsonContent = await req.Content.ReadAsStreamAsync();
            var chatMesasgeResult = await JsonSerializer.DeserializeAsync<ChatMessage>(jsonContent);
            await signalRMessages.AddAsync(
               new SignalRMessage
               {
                   Target = HubHandler.ReceivedMessage,
                   Arguments = new[] { chatMesasgeResult }
               });
        }
    }
}
