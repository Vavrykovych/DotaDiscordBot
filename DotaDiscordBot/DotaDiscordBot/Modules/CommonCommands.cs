using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
        [Command("ping")]
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
            if (filteredUsers.Length == 0) return;

            Random rand = new Random();
            var padarIndex = rand.Next(0, filteredUsers.Count());
            var pidarUser = filteredUsers[padarIndex];
            await ReplyAsync($"Підара знайдено, це - {pidarUser.DisplayName}");
        }
    }
}
