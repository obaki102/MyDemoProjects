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

namespace MyDemoProjects.AzureFunctions
{
    public static class RandomGotQuotesHttpTrigger
    {
        [FunctionName("RandomGotQuotesHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using(var client = new HttpClient())
            {
                var gotQuuotesResponse = await client.GetStreamAsync("https://api.gameofthronesquotes.xyz/v1/random");
                if(gotQuuotesResponse == null)
                {
                    return new NotFoundResult();
                }
                var gotQuuotesResult = JsonSerializer.Deserialize<RandomGOTQuotesResponse>(gotQuuotesResponse);

                var embedRequest = new Embed("Random GOT Quotes", gotQuuotesResult.Sentence, 5814783, new Footer(gotQuuotesResult.Character.Name));
                var rootRequest = new Root("",new List<Embed>{ embedRequest });
                var serializedRequest = JsonSerializer.Serialize<Root>(rootRequest);
                var postResponse = await client.PostAsync("https://discord.com/api/webhooks/1027409178727297024/R1ek93N_HYYdxotCdefbyQRGUiDW3KrRpgHvdkr04u6DhY8YR90v2Guq-i1nFMuoqswd", new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json"));
                if (!postResponse.IsSuccessStatusCode)
                {
                    return new BadRequestResult();
                }
                
                log.LogInformation(postResponse.StatusCode.ToString()); 
                return new OkObjectResult(rootRequest);
            }
        }

    }

    public static class RandomGotQuotesTimeTrigger
    {
        /// <summary>
        /// Runs every 6 hrs.
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("RandomGotQuotesTimeTrigger")]
        public static async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("Function triggered.");

            using (var client = new HttpClient())
            {
                var gotQuuotesResponse = await client.GetStreamAsync("https://api.gameofthronesquotes.xyz/v1/random");
                var gotQuuotesResult = JsonSerializer.Deserialize<RandomGOTQuotesResponse>(gotQuuotesResponse);

                var embedRequest = new Embed("Random GOT Quotes", gotQuuotesResult.Sentence, 5814783, new Footer(gotQuuotesResult.Character.Name));
                var rootRequest = new Root("", new List<Embed> { embedRequest });
                var serializedRequest = JsonSerializer.Serialize<Root>(rootRequest);
                var postResponse = await client.PostAsync("https://discord.com/api/webhooks/1027409178727297024/R1ek93N_HYYdxotCdefbyQRGUiDW3KrRpgHvdkr04u6DhY8YR90v2Guq-i1nFMuoqswd", new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json"));

                log.LogInformation(postResponse.StatusCode.ToString());
            }
        }
    }

    public record Character(
     [property: JsonPropertyName("name")] string Name,
     [property: JsonPropertyName("slug")] string Slug,
     [property: JsonPropertyName("house")] House House
 );

    public record House(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("slug")] string Slug
    );

    public record RandomGOTQuotesResponse(
        [property: JsonPropertyName("sentence")] string Sentence,
        [property: JsonPropertyName("character")] Character Character
    );

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
