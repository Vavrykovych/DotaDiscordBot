using Discord.Commands;
using Discord.WebSocket;
using DotaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class DivideCommands : ModuleBase<SocketCommandContext>
    {
        [Command("divide")]
        public async Task DivideTeams()
        {
            try
            {
                var playersPool = new List<Player>();

                List<Player> team1;
                List<Player> team2;

                int triesCounter = 0;

                while (true)
                {
                    playersPool = ReadActivePlayers();

                    if (playersPool.Count() < 10)
                    {
                        throw new Exception("В каналі менше 10 гравців");
                    }

                    if (triesCounter > 1000000)
                    {
                        throw new Exception("Некоректні ролі");
                    }

                    triesCounter++;
                    team1 = new List<Player> { };
                    team2 = new List<Player> { };

                    var rand = new Random();


                    var carryes = playersPool.Where(x => x.Roles.Contains(Role.Carry)).ToArray();

                    if (carryes.Length < 2)
                    {
                        throw new Exception("В чаті менше двох кері");
                    }

                    var carry1 = rand.Next(0, carryes.Length);
                    var carry2 = rand.Next(0, carryes.Length);


                    while (carry1 == carry2)
                    {
                        carry2 = rand.Next(0, carryes.Length);
                    }


                    team1.Add(carryes[carry1]);
                    team2.Add(carryes[carry2]);
                    playersPool.Remove(carryes[carry1]);
                    playersPool.Remove(carryes[carry2]);


                    var miders = playersPool.Where(x => x.Roles.Contains(Role.Mid)).ToArray();

                    if (miders.Length < 2)
                    {
                        continue;
                    }


                    var mider1 = rand.Next(0, miders.Length);
                    var mider2 = rand.Next(0, miders.Length);
                    while (mider2 == mider1)
                    {
                        mider2 = rand.Next(0, miders.Length);
                    }


                    team1.Add(miders[mider1]);
                    team2.Add(miders[mider2]);

                    playersPool.Remove(miders[mider1]);
                    playersPool.Remove(miders[mider2]);


                    var offlaners = playersPool.Where(x => x.Roles.Contains(Role.Offlane)).ToArray();

                    if (offlaners.Length < 2)
                    {
                        continue;
                    }


                    var offlaner1 = rand.Next(0, offlaners.Length);
                    var offlaner2 = rand.Next(0, offlaners.Length);

                    while (offlaner1 == offlaner2)
                    {
                        offlaner2 = rand.Next(0, offlaners.Length);
                    }


                    team1.Add(offlaners[offlaner1]);
                    team2.Add(offlaners[offlaner2]);
                    playersPool.Remove(offlaners[offlaner1]);
                    playersPool.Remove(offlaners[offlaner2]);



                    var supports = playersPool.Where(x => x.Roles.Contains(Role.SoftSupport)).ToArray();
                    if (supports.Length < 2)
                    {
                        continue;
                    }


                    var support1 = rand.Next(0, supports.Length);
                    var support2 = rand.Next(0, supports.Length);

                    while (support2 == support1)
                    {
                        support2 = rand.Next(0, supports.Length);
                    }


                    team1.Add(supports[support1]);
                    team2.Add(supports[support2]);
                    playersPool.Remove(supports[support1]);
                    playersPool.Remove(supports[support2]);

                    supports = playersPool.Where(x => x.Roles.Contains(Role.HardSupport)).ToArray();
                    if (supports.Length < 2)
                    {
                        continue;
                    }


                    support1 = rand.Next(0, supports.Length);
                    support2 = rand.Next(0, supports.Length);

                    while (support1 == support2)
                    {
                        support2 = rand.Next(0, supports.Length);
                    }


                    team1.Add(supports[support1]);
                    team2.Add(supports[support2]);
                    playersPool.Remove(supports[support1]);
                    playersPool.Remove(supports[support2]);

                    break;
                }

                var message = "";

                message += "**Radiant team**\n";

                message += $"\tCarry - ***{team1[0].Name}***\n";
                message += $"\tMid - ***{team1[1].Name}***\n";
                message += $"\tOfflane - ***{team1[2].Name}***\n";
                message += $"\tSoftSupport - ***{team1[3].Name}***\n";
                message += $"\tHardSupport - ***{team1[4].Name}***\n\n";

                message += "**Dire team**\n";

                message += $"\tCarry - ***{team2[0].Name}***\n";
                message += $"\tMid - ***{team2[1].Name}***\n";
                message += $"\tOfflane - ***{team2[2].Name}***\n";
                message += $"\tSoftSupport - ***{team2[3].Name}***\n";
                message += $"\tHardSupport - ***{team2[4].Name}***\n";


                await ReplyAsync(message);

                await MoveToVoiceChannels(team1, team2);
                return;
            }
            catch (Exception ex)
            {
                await ReplyAsync("Неможливо поділити на команди\n" + ex.Message);
            }
        }


        [Command("back")]
        public async Task BackToMainChannel()
        {
            ulong? radientTeamChannelId = 1084408132073160724;
            ulong? direTeamChannelId = 1084408269717647400;

            ulong? mainChannelId = 874701687229673472;


            var channel = Context.Channel as SocketChannel;
            var users = channel.Users;

            foreach (SocketGuildUser user in users)
            {
                if (user.VoiceChannel?.Id == radientTeamChannelId
                    || user.VoiceChannel?.Id == direTeamChannelId)
                {
                    user.ModifyAsync(x => x.ChannelId = mainChannelId);
                }
            }

            await ReplyAsync("Повернув людей в основний канал");
        }

        private List<Player> ReadActivePlayers()
        {
            var channel = Context.Channel as SocketChannel;
            var users = channel.Users;

            var playersPool = new List<Player>();

            foreach (SocketGuildUser user in users)
            {
                if (user?.VoiceChannel?.Id == 874701687229673472)
                {
                    var player = MapDiscordUserToPlayerModel(user);

                    if (player != null)
                    {
                        playersPool.Add(player);
                    }
                }
            }

            return playersPool;
        }

        private Player? MapDiscordUserToPlayerModel(SocketGuildUser user)
        {
            var roles = user.Roles.Where(x => x.Id != 582142000565059594).Select(r => r.Name);
            if (!roles.Contains("Mid")
                && !roles.Contains("Carry")
                && !roles.Contains("Offlane")
                && !roles.Contains("SoftSupport")
                && !roles.Contains("HardSupport"))
            {
                return null;
            }

            var player = new Player { Name = user.DisplayName, Roles = new List<Role>(), DiscordUser = user };

            if (roles.Contains("Mid"))
            {
                player.Roles.Add(Role.Mid);
            }

            if (roles.Contains("Carry"))
            {
                player.Roles.Add(Role.Carry);
            }

            if (roles.Contains("Offlane"))
            {
                player.Roles.Add(Role.Offlane);
            }

            if (roles.Contains("SoftSupport"))
            {
                player.Roles.Add(Role.SoftSupport);
            }

            if (roles.Contains("HardSupport"))
            {
                player.Roles.Add(Role.HardSupport);
            }

            return player;
        }

        private async Task MoveToVoiceChannels(List<Player> radient, List<Player> dire)
        {
            ulong? radientTeamChannelId = 1084408132073160724;
            ulong? direTeamChannelId = 1084408269717647400;

            foreach (var player in radient)
            {
                if (player.DiscordUser != null)
                {
                    player.DiscordUser.ModifyAsync(x => x.ChannelId = radientTeamChannelId);
                }
            }

            foreach (var player in dire)
            {
                if (player.DiscordUser != null)
                {
                    player.DiscordUser.ModifyAsync(x => x.ChannelId = direTeamChannelId);
                }
            }
        }
    }
}
