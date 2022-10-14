using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyDemoProjects.DiscordBot;
using MyDemoProjects.DiscordBot.Handlers;
using MyDemoProjects.DiscordBot.Modules;
using MyDemoProjects.DiscordBot.Services;

//var config = new ConfigurationBuilder()
//    .AddJsonFile($"appsettings.json")
//    .AddEnvironmentVariables()
//    .Build();
var client = new DiscordShardedClient(new DiscordSocketConfig
{
    GatewayIntents  = Discord.GatewayIntents.All,
    LogGatewayIntentWarnings = false,
    AlwaysDownloadUsers = true,
    LogLevel = LogSeverity.Debug
});


var commands = new CommandService(new CommandServiceConfig
{
    // Again, log level:
    LogLevel = LogSeverity.Info,

    // There's a few more properties you can set,
    // for example, case-insensitive commands.
    CaseSensitiveCommands = false,
});

// Setup your DI container.
Initializer.Init();
Initializer.RegisterInstance(client);
Initializer.RegisterInstance(commands);
Initializer.RegisterType<ICommandHandler,CommandHandler>();

await MainAsync();

async Task MainAsync()
{
    await Initializer.ServiceProvider.GetRequiredService<ICommandHandler>().InitializeAsync();

    client.ShardReady += async shard =>
    {
        Console.WriteLine($"Shard Number {shard.ShardId} is connected and ready!");
    };

    client.Log += async (LogMessage log) =>
    {
        Console.WriteLine(log.Message);
    };
    await client.LoginAsync(TokenType.Bot, "MTAyNzQwNjQwNDE0OTA3MTk1Mg.GEEroy.1Mmjg0sa7cmfl9engwWIB0Q5Od1Rdk2-VhYN7Q");
    await client.StartAsync();

    // Wait infinitely so your bot actually stays connected.
    await Task.Delay(Timeout.Infinite);
}