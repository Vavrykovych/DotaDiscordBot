using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class CommonCommands : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public CommonCommands(CommandService commandService)
        {
            _commandService = commandService;
        }


        [Command("ping")]
        [Summary("Ping")]
        public async Task Ping()
        {
            await ReplyAsync(Context.User.Mention + " pong");
        }

        [Command("pidar-scanner")]
        public async Task PidarScanner()
        {
            await ReplyAsync("Шукаємо підара...");
            IGuild guild = Context.Guild;
            IEnumerable<IGuildUser> users = await guild.GetUsersAsync();
            var filteredUsers = users.Where(x => !x.IsBot && x.Status != UserStatus.Offline).ToArray();
            if (filteredUsers.Length == 0)
            {
                await ReplyAsync("Підара не знайдено(");
                return;
            }

            Random rand = new Random();
            var padarIndex = rand.Next(0, filteredUsers.Count());
            var pidarUser = filteredUsers[padarIndex];

            await ReplyAsync($"Підара знайдено, це - {pidarUser.DisplayName}");
        }

        [Command("roll")]
        public async Task Roll(int min = 0, int max = 100)
        {
            if (min > max)
            {
                (min, max) = (max, min);
            }

            var random = new Random();
            var roll = random.Next(min, max + 1);

            await ReplyAsync($"{Context.User.Mention} твоє число: {roll}");
        }


        [Command("help")]
        [Summary("Displays a list of available commands or information about a specific command.")]
        public async Task HelpCommand([Remainder][Summary("The name of the command to get help for (optional).")] string commandName = null)
        {
            if (commandName == null)
            {
                // Display a list of available commands.
                var commands = _commandService.Commands;

                var embed = new EmbedBuilder()
                    .WithTitle("Available commands:")
                    .WithDescription(string.Join("\n", commands.Select(c => c.Name)))
                    .WithColor(Color.Green)
                    .Build();

                await ReplyAsync(embed: embed);
            }
            else
            {
                // Display help information for a specific command.
                var command = _commandService.Commands.FirstOrDefault(c => c.Name == commandName);

                if (command == null)
                {
                    await ReplyAsync($"Command '{commandName}' not found.");
                    return;
                }

                var embed = new EmbedBuilder()
                    .WithTitle($"Help: {command.Name}")
                    .WithDescription(command.Summary)
                    .AddField("Usage", $"/{command.Name} {string.Join(" ", command.Parameters.Select(p => $"[{p.Name}]"))}")
                    .WithColor(Color.Blue)
                    .Build();

                await ReplyAsync(embed: embed);
            }
        }

        [Command("flip")]
        public async Task CoinFlipAsync()
        {
            Random random = new Random();
            int result = random.Next(3);
            string outcome = result == 0 ? "Камінь" : (result == 1 ? "Ножиці" : "Папір");
            await ReplyAsync($"{Context.User.Mention} {outcome}!");
        }
    }
}
