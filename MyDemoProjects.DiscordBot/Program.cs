using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyDemoProjects.DiscordBot;
using MyDemoProjects.DiscordBot.Handlers;
using MyDemoProjects.DiscordBot.Modules;
using MyDemoProjects.DiscordBot.Services;


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

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

// Setup your DI container.
Initializer.Init();
Initializer.RegisterInstance(client);
Initializer.RegisterInstance(commands);
Initializer.RegisterInstance(config);
Initializer.RegisterType<ICommandHandler,CommandHandler>();

await MainAsync();

async Task MainAsync()
{
    await Initializer.ServiceProvider.GetRequiredService<ICommandHandler>().InitializeAsync();

    client.ShardReady += async shard =>
    {
        Console.WriteLine($"Shard Number {shard.ShardId} is connected and ready!");
    };

    var token = config.GetSection("DiscordTokenKey").Value;

    if (string.IsNullOrWhiteSpace(token))
    {
        Console.WriteLine("Token is null or empty.");
        return;
    }

    client.Log += async (LogMessage log) =>
    {
        Console.WriteLine(log.Message);
    };
    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    // Wait infinitely so your bot actually stays connected.
    await Task.Delay(Timeout.Infinite);
}