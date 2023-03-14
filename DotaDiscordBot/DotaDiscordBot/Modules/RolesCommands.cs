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
        [Command("add-role")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task AssignRoleCommand(SocketGuildUser user, SocketRole role)
        {
            await AssignRole(user, role);
        }

        [Command("remove-role")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task UnassignCommand(SocketGuildUser user, SocketRole role)
        {
            await UnassignRole(user, role);
        }

        public async Task AssignRole(SocketGuildUser user, SocketRole role)
        {
            try
            {
                if (user.Roles.Contains(role))
                {
                    await ReplyAsync($"{user.Mention} already has the {role.Name} role.");
                    return;
                }

                await user.AddRoleAsync(role);
                await ReplyAsync($"{user.Mention} has been assigned the {role.Name} role.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to assign role to {user.Username}: {ex.Message}");
                await ReplyAsync($"Failed to assign role to {user.Mention}. Please try again later.");
            }
        }

        public async Task UnassignRole(SocketGuildUser user, SocketRole role)
        {
            try
            {
                // Check if the user doesn't have the role.
                if (!user.Roles.Contains(role))
                {
                    await ReplyAsync($"{user.Mention} doesn't have the {role.Name} role.");
                    return;
                }

                // Remove the role from the user.
                await user.RemoveRoleAsync(role);
                await ReplyAsync($"{user.Mention} has been unassigned the {role.Name} role.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur.
                Console.WriteLine($"Failed to unassign role from {user.Username}: {ex.Message}");
                await ReplyAsync($"Failed to unassign role from {user.Mention}. Please try again later.");
            }
        }
    }
}
