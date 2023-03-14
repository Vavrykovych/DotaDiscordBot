using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class RolesCommands : ModuleBase<SocketCommandContext>
    {
        private ulong?[] AllowedRoleIds = { 1084248441120628776, 1084248355049328793, 1084223352627003412, 1084248835481686067, 1084248949558358080 };

        [Command("add-role")]
        public async Task AssignRoleCommand(SocketGuildUser user, params SocketRole[] roles)
        {
            await AssignRole(user, roles);
        }

        [Command("remove-role")]
        public async Task UnassignCommand(SocketGuildUser user, params SocketRole[] roles)
        {
            await UnassignRole(user, roles);
        }

        public async Task AssignRole(SocketGuildUser user, params SocketRole[] roles)
        {
            try
            {
                foreach (SocketRole role in roles)
                {
                    if (!AllowedRoleIds.Contains(role.Id))
                    {
                        await ReplyAsync($"Не можна присвоювати роль {role.Mention}.");
                        continue;
                    }

                    if (user.Roles.Contains(role))
                    {
                        await ReplyAsync($"{user.Mention} вже має роль {role.Name}.");
                        continue;
                    }

                    await user.AddRoleAsync(role);
                    await ReplyAsync($"{user.Mention} було додано роль {role.Name}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to assign role to {user.Username}: {ex.Message}");
                await ReplyAsync($"Не вийшло присвоїти роль {user.Mention}.");
            }
        }

        public async Task UnassignRole(SocketGuildUser user, params SocketRole[] roles)
        {
            try
            {
                foreach (SocketRole role in roles)
                {
                    if (!AllowedRoleIds.Contains(role.Id))
                    {
                        await ReplyAsync($"Не можна видалити роль {role.Mention}.");
                        continue;
                    }

                    if (!user.Roles.Contains(role))
                    {
                        await ReplyAsync($"{user.Mention} не має ролі {role.Name}.");
                        continue;
                    }

                    await user.RemoveRoleAsync(role);
                    await ReplyAsync($"{user.Mention} було видалено роль {role.Name}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to unassign role from {user.Username}: {ex.Message}");
                await ReplyAsync($"Не вийшло видалити роль {user.Mention}.");
            }
        }
    }
}
