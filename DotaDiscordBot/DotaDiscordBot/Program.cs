using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

using Discord;
using Discord.WebSocket;

public enum Role
{
    Mid,
    Carry,
    Offlane,
    HardSupport,
    SoftSupport
}

public class Player
{
    public string Name { get; set; }

    public List<Role> Roles { get; set; }

}

class Program
{
    private List<Player> GetHardcodedData => new List<Player>
    {
        new Player()
            {
                Name = "Po4vara",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            //new Player()
            //{
            //    Name = "Wakman",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.Hard, Role.SoftSupport, Role.HardSupport }
            //},
            new Player()
            {
                Name = "Oleg XSQ",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
            //new Player()
            //{
            //    Name = "Grandi show",
            //
            //    Roles = new List<Role> { Role.SoftSupport, Role.HardSupport }
            //},
            new Player()
            {
                Name = "Dencolog",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
            new Player()
            {
                Name = "Barabola",

                Roles = new List<Role> { Role.Mid }
            },
            //new Player()
            //{
            //    Name = "Mitric",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.SoftSupport, Role.HardSupport, Role.Hard, Role.SoftSupport }
            //},
            new Player()
            {
                Name = "Dmutro",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            new Player()
            {
                Name = "Despot",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport, Role.HardSupport }
            },
            new Player()
            {
                Name = "Wackman",

                Roles = new List<Role> { Role.HardSupport, Role.SoftSupport }
            },
            //new Player()
            //{
            //    Name = "Agent crip",
            //
            //    Roles = new List<Role> { Role.Hard, Role.SoftSupport, Role.HardSupport }
            //},
            //new Player()
            //{
            //    Name = "Matu",
            //
            //    Roles = new List<Role> { Role.Mid, Role.Carry, Role.Hard }
            //},


            new Player()
            {
                Name = "Gambit",
                Roles = new List<Role> { Role.Carry, Role.Offlane, Role.HardSupport, Role.SoftSupport }
            },
            new Player()
            {
                Name = "Mitrik",

                Roles = new List<Role> { Role.Offlane, Role.SoftSupport }
            },
            new Player()
            {
                Name = "Maskarpone",

                Roles = new List<Role> { Role.Mid, Role.Carry }
            },
    };

    private async Task<List<Player>> ReadPlayersFromChannel(ISocketMessageChannel channel)
    {
        var channelUsers = (await channel.GetUsersAsync().ToListAsync()).FirstOrDefault().ToList();
        var playersPool = new List<Player>();

        foreach (SocketGuildUser user in channelUsers)
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



    private async Task MoveToVoiceChannels(List<Player> players)
    {
    }




    private Player? MapDiscordUserToPlayerModel(SocketGuildUser user)
    {
        var roles = user.Roles.Where(x => x.Id != 582142000565059594).Select(r => r.Name);
        if (   !roles.Contains("Mid")
            && !roles.Contains("Carry")
            && !roles.Contains("Offlane")
            && !roles.Contains("SoftSupport")
            && !roles.Contains("HardSupport"))
        {
            return null;
        }

        var player = new Player { Name = user.DisplayName, Roles = new List<Role>() };

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




    private async Task DivideTeamsAsync(SocketMessage msg)
    {
        try
        {
            var playersPool = new List<Player>();

            List<Player> team1;
            List<Player> team2;

            int triesCounter = 0;

            while (true)
            {
                playersPool = GetHardcodedData;// Remove
                //playersPool = await ReadPlayersFromChannel(msg.Channel);


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
                
                if(carryes.Length < 2)
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


            await msg.Channel.SendMessageAsync(message);
            return;
        }
        catch (Exception ex)
        {
            await msg.Channel.SendMessageAsync("Неможливо поділити на команди\n" + ex.Message);
        }

    }

    public async Task CommandHandler(SocketMessage msg)
    {
        if (msg.Author.IsBot)
        {
            return;
        }

        if (msg.Channel.Id != 1084263139224322108) // only-bot-channel
        {
            return;
        }

        Console.WriteLine(msg.Content);

        if (!msg.ToString().StartsWith("/"))
        {
            return;
        }

        switch (msg.Content)
        {
            case ("/divide"):
                await DivideTeamsAsync(msg);
                break;

            case ("/move"):
                Console.WriteLine("Test");
                //msg.F.

                ulong? chId = 582143179445501953;

                var channelUsers1 = (await msg.Channel.GetUsersAsync().ToListAsync()).FirstOrDefault().ToList().First() as SocketGuildUser;
                await channelUsers1.ModifyAsync(x => x.ChannelId = chId);
                //channelUsers1.V
                //await channelUsers1.VoiceChannel.DisconnectAsync();

                //await channelUsers1.VoiceChannel.;

                break;

            case ("/pidar-scanner"):
                await msg.Channel.SendMessageAsync("Шукаємо підара...");
                var channelUsers = (await msg.Channel.GetUsersAsync().ToListAsync()).FirstOrDefault().ToList();
                Random rand = new Random();
                var padarIndex = rand.Next(0, channelUsers.Count);
                SocketGuildUser pidarUser = channelUsers[padarIndex] as SocketGuildUser;
                await msg.Channel.SendMessageAsync($"Підара знайдено... це - {pidarUser.DisplayName}");
                break;

            default:
                await msg.Channel.SendMessageAsync("Ти хуйню пишеш");
                break;  
        }
    }

    public async Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
    }

    private async Task MainAsync()
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent | GatewayIntents.GuildMembers
        };

        var client = new DiscordSocketClient(config);
        client.MessageReceived += CommandHandler;
        client.Log += Log;


        var token = "MTA4NDE1OTA1MTc4MjQzOTExNQ.GlFMJ6.QhwvNQHPd3wA0j4xsGWWbPXcrfUt-qN23OLXuU";

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();



        Console.ReadLine();
    }
    static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
}
