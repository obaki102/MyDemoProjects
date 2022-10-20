using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyDemoProjects.DiscordBot;
using MyDemoProjects.DiscordBot.Handlers;
using MyDemoProjects.DiscordBot.Services;


var client = new DiscordShardedClient(new DiscordSocketConfig
{
    GatewayIntents  = GatewayIntents.All,
    LogGatewayIntentWarnings = false,
    AlwaysDownloadUsers = true,
    LogLevel = LogSeverity.Debug
});

var commands = new CommandService(new CommandServiceConfig
{
    LogLevel = LogSeverity.Info,
    CaseSensitiveCommands = false,
});
var interactionCommand = new InteractionService(client);

var hubConnection = new HubConnectionBuilder().WithUrl("https://mydemoprojectsfunction.azurewebsites.net/api/?Code=eo8KJcUhAJqIjNjhAaFStXRKfJSDi3AxYRT_F530PKlZAzFuEZBPcQ==").Build();

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

// Setup your DI container.
Initializer.Init();
Initializer.RegisterSingletonInstance(client);
Initializer.RegisterSingletonInstance(commands);
Initializer.RegisterSingletonInstance(interactionCommand);
Initializer.RegisterSingletonInstance(hubConnection);
Initializer.RegisterSingletonInstance(config);
Initializer.RegisterSingletonType<ICommandHandler,CommandHandler>();
Initializer.RegisterSingletonType<IInteractionHandler, InteractionHandler>();
Initializer.BuildServiceProvider();

await MainAsync();

async Task MainAsync()
{
    await Initializer.ServiceProvider.GetRequiredService<ICommandHandler>().InitializeAsync();
    await Initializer.ServiceProvider.GetRequiredService<IInteractionHandler>().InitializeAsync();

    client.ShardReady += async shard =>
    {
        Console.WriteLine($"Shard Number {shard.ShardId} is connected and ready!");
        //Register slash commands to guild
        await interactionCommand.RegisterCommandsToGuildAsync(UInt64.Parse(config.GetSection("RoninGuildId").Value), true);
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