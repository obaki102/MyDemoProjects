using Discord;
using Discord.Commands;
using MyDemoProjects.DiscordBot.DTOs;
using System.Text.Json;
using RunMode = Discord.Commands.RunMode;


namespace MyDemoProjects.DiscordBot.Modules
{
    public class PingCommands : ModuleBase<ShardedCommandContext>
    {
        public CommandService CommandService { get; set; }

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
                    //.WithUrl("https://example.com")
                    //.WithCurrentTimestamp();

                //Your embed needs to be built before it is able to be sent
                await Context.Message.ReplyAsync(embed: embed.Build());
            }
        }
    }
}
