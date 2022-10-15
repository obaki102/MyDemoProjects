using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MyDemoProjects.DiscordBot.DTOs;
using System.Net.Http;
using System.Text.Json;
using RunMode = Discord.Commands.RunMode;


namespace MyDemoProjects.DiscordBot.Modules
{
    public class PingCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly IConfiguration _configuration;
        public PingCommands(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Command("ping", RunMode = RunMode.Async)]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Zup {Context.User.Username} !");
        }

        [Command("pogi", RunMode = RunMode.Async)]
        public async Task Pogi()
        {
            await Context.Message.ReplyAsync($"Josh pogi");
        }

        [Command("got", RunMode = RunMode.Async)]
        public async Task Got()
        {
            using (var client = new HttpClient())
            {
                var gotQuotesResponse = await client.GetStreamAsync("https://api.gameofthronesquotes.xyz/v1/random");
                if (gotQuotesResponse == null)
                {
                    Console.WriteLine("gotQuuotesResponse not found");
                }
                var gotQuotesResult = JsonSerializer.Deserialize<RandomGOTQuotesResponse>(gotQuotesResponse);
                var embed = new EmbedBuilder();
                embed.AddField("Got Quotes",
                    gotQuotesResult.Sentence)
                    .WithAuthor(Context.Client.CurrentUser)
                    .WithFooter(footer => footer.Text = gotQuotesResult.Character.Name)
                    .WithColor(Color.Blue);
                
                await Context.Message.ReplyAsync(embed: embed.Build());
            }
        }


        [Command("D2HotFix", RunMode = RunMode.Async)]
        public async Task LatestHotFix()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", _configuration.GetSection("XApiKey").Value);
                var response = await client.GetStreamAsync("https://www.bungie.net/Platform//Trending/Categories");
                if (response == null)
                {
                    Console.WriteLine("Response not found");
                    return;
                }
                var result = JsonSerializer.Deserialize<GetTrendingCategoriesResponse>(response);
                if(result == null ) 
                {
                    Console.WriteLine("No result.");
                    return;
                }
                var latestHotFix = result.Response.Categories
                    .FirstOrDefault(c => c.CategoryName.Equals("Latest")).Entries.Results
                    .OrderByDescending(o => o.CreationDate)
                    .FirstOrDefault(f => f.DisplayName.Contains("Destiny 2 Hotfix"));
                var embed = new EmbedBuilder();
                embed.AddField(latestHotFix.DisplayName,
                    $"[Hot Fix](https://www.bungie.net{latestHotFix.Link})")
                    .WithImageUrl($"https://www.bungie.net{latestHotFix.FeatureImage}")
                    .WithAuthor(Context.Client.CurrentUser)
                    .WithColor(Color.Blue);
              
                await Context.Message.ReplyAsync(embed: embed.Build());
            }
        }


    }
}
