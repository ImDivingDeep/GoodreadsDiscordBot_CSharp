using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoodreadsDiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider serviceProvider)
        {
            _commands = commands;
            _client = client;
            _services = serviceProvider;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += Client_MessageReceived;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), _services);
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg is not SocketUserMessage message) return;
            if (message.Author.IsBot) return;

            if (message.Content.Contains("https://www.goodreads.com/book"))
            {
                // Parse book
                var url = Regex.Match(message.Content, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                var book = await _services.GetService<GoodreadsService>().ParsePage(url.Value);

                var embed = new EmbedBuilder()
                                .WithImageUrl(book.ImageUrl)
                                .WithTitle(book.Title)
                                .WithDescription(book.Description)
                                .Build();

                await message.Channel.SendMessageAsync(embed: embed);
            }

            return;
        }
    }
}
