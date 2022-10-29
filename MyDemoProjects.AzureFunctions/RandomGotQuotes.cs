using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.AzureFunctions
{
    public static class RandomGotQuotes
    {
        public static HttpClient httpClient = new HttpClient();
        [FunctionName("RandomGotQuotesHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var gotQuuotesResponse = await httpClient.GetStreamAsync("https://api.gameofthronesquotes.xyz/v1/random");
            if (gotQuuotesResponse == null)
            {
                return new NotFoundResult();
            }
            var gotQuuotesResult = JsonSerializer.Deserialize<RandomGOTQuotesResponse>(gotQuuotesResponse);

            var embedRequest = new Embed("Random GOT Quotes", gotQuuotesResult.Sentence, 5814783, new Footer(gotQuuotesResult.Character.Name));
            var rootRequest = new Root("", new List<Embed> { embedRequest });
            var serializedRequest = JsonSerializer.Serialize<Root>(rootRequest);
            //Send response to discord webhook.
            var postResponse = await httpClient.PostAsync("https://discord.com/api/webhooks/1027409178727297024/R1ek93N_HYYdxotCdefbyQRGUiDW3KrRpgHvdkr04u6DhY8YR90v2Guq-i1nFMuoqswd", new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json"));
            if (!postResponse.IsSuccessStatusCode)
            {
                return new BadRequestResult();
            }

            log.LogInformation(postResponse.StatusCode.ToString());
            return new OkObjectResult(rootRequest);
        }

    }

    public record Embed(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("color")] int Color,
        [property: JsonPropertyName("footer")] Footer Footer
    );

    public record Footer(
        [property: JsonPropertyName("text")] string Text
    );

    public record Root(
        string Content,
        [property: JsonPropertyName("embeds")] List<Embed> Embeds
    );

}
