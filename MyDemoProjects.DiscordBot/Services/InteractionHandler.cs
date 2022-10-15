using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MyDemoProjects.DiscordBot.Handlers;
using System.Reflection;


namespace MyDemoProjects.DiscordBot.Services
{
    public class InteractionHandler : IInteractionHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly InteractionService _commands;

        public InteractionHandler(
            DiscordShardedClient client,
            InteractionService commands)
        {
            _client = client;
            _commands = commands;
        }
        public async Task InitializeAsync()
        {
            // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Initializer.ServiceProvider);

            // Process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;

            // Process the command execution results 
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
            _commands.ModalCommandExecuted += ModalCommandExecuted;


            foreach (var module in _commands.Modules)
            {
                Console.WriteLine($"{nameof(InteractionHandler)} | Commands", $"Module '{module.Name}' initialized.");
            }
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }

        private Task ModalCommandExecuted(ModalCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            return Task.CompletedTask;
        }
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var context = new ShardedInteractionContext(_client, arg);
                var result = await _commands.ExecuteCommandAsync(context, Initializer.ServiceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
