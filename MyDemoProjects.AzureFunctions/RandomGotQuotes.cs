using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Text.Json;

namespace MyDemoProjects.AzureFunctions
{
    public static class RandomGotQuotes
    {
        [FunctionName("RandomGotQuotes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using(var client = new HttpClient())
            {
                var response = await client.GetStreamAsync("https://api.gameofthronesquotes.xyz/v1/random");
                var result = JsonSerializer.Deserialize<RandomGOTQuotesResponse>(response);
                return new OkObjectResult(result);
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
}
