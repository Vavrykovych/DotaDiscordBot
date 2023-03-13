using Discord.WebSocket;

namespace DotaBot.Models
{
    public class Player
    {
        public string Name { get; set; }

        public List<Role> Roles { get; set; }

        public SocketGuildUser DiscordUser { get; set; }
    }
}
