using Discord.WebSocket;
using Discord;
using System.Reflection;
using Discord.Commands;
using MyDemoProjects.DiscordBot.Services;
using Microsoft.AspNetCore.SignalR.Client;
using MyDemoProjects.Application.Shared.Models;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using MyDemoProjects.Application.Shared.Constants;
using Newtonsoft.Json;
using MyDemoProjects.DiscordBot.DTOs;

namespace MyDemoProjects.DiscordBot.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;
        private readonly HubConnection _hubConnection;
        private readonly HttpClient _httpClient;

        public CommandHandler(
            DiscordShardedClient client,
            CommandService commands,
            HubConnection hubConnection,
            HttpClient httpClient)
        {
            _client = client;
            _commands = commands;
            _hubConnection = hubConnection;
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Initializer.ServiceProvider);
            await _hubConnection.StartAsync();

            _hubConnection.On<object>(HubHandler.ReceivedMessage, async (receivedMessage) =>
            {
                var chatMessage = JsonConvert.DeserializeObject<ChatMessage>(receivedMessage.ToString());
                ulong id = 1032124362523955210; // 3
                var chnl = _client.GetChannel(id) as IMessageChannel; // 4
                if (!chatMessage.User.Name.Equals("Chatbot"))
                {
                    await chnl.SendMessageAsync($"from {chatMessage.User.Name} Message: {chatMessage.Message}"); // 5
                }
            });
            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;

            _commands.CommandExecuted += async (optional, context, result) =>
            {
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    // the command failed, let's notify the user that something happened.
                    await context.Channel.SendMessageAsync($"error: {result}");
                }
            };

            foreach (var module in _commands.Modules)
            {
                Console.WriteLine($"{nameof(CommandHandler)} | Commands", $"Module '{module.Name}' initialized.");
            }


        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            if (arg is not SocketUserMessage msg)
                return;

            // We don't want the bot to respond to itself or other bots.
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
                return;

            if (msg.Channel.Id == 1032124362523955210)
            {
                var chatMessage = new ChatMessage
                {
                    User = new User
                    {
                        Name = "Chatbot"
                    },
                    Message = msg.Content
                };
                // await _hubconnetion.SendAsync("ReceivedMessage", chatMessage);
                await _httpClient.PostAsJsonAsync("https://mydemoprojectsfunction.azurewebsites.net/api/messages?code=mKDB3ubiBfiwtJ0xnYD7VqX__VRopWheo2IY9wLkcdMWAzFu8E0wvQ==", chatMessage);
            }


            // Create a Command Context.
            var context = new ShardedCommandContext(_client, msg);
            var markPos = 0;
            if (msg.HasStringPrefix("obaki/", ref markPos) || msg.HasCharPrefix('?', ref markPos))
            {
                Console.WriteLine("It has 'obaki'");
                //  await context.Message.ReplyAsync("Zup boss");
                var result = await _commands.ExecuteAsync(context, markPos, Initializer.ServiceProvider);
            }


            // bail if the message is not a user one (system messages cannot have reactions)
            //var usermsg = msg as IUserMessage;
            //if (usermsg == null) return;

            //// standard Unicode emojis
            //Emoji emoji = new Emoji("👍");
            //// or
            //// Emoji emoji = new Emoji("\u23F8");

            //// custom guild emotes
            //Emote emote2 = Emote.Parse("<:dotnet:232902710280716288>");
            //// using Emote.TryParse may be safer in regards to errors being thrown;
            //// please note that the method does not verify if the emote exists,
            //// it simply creates the Emote object for you.

            //// add the reaction to the message
            //await usermsg.AddReactionAsync(emoji);
            //await usermsg.AddReactionAsync(emote2);


        }
    }

}
