using Discord.Commands;
using DotaBot.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class SteamCommands : ModuleBase<SocketCommandContext>
    {
        [Command("get-mmr")]

        public async Task SteamViewer (string id)
        {
            await ReplyAsync($"Search MMR user....");

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://api.opendota.com/api/players/{id}");
                var content = await response.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (response.IsSuccessStatusCode && data.ContainsKey("profile"))
                {
                    int mmrEstimate = data.mmr_estimate.estimate;
                    string personaname = data.profile.personaname;

                    await ReplyAsync($"A user with the name {personaname} was found, his MMR = {mmrEstimate}");

                }
                else
                {
                    throw new HttpRequestException();
                }

            }
            catch (HttpRequestException ex)
            {
                await ReplyAsync($"User with id: {id} not found, or you entered an invalid id");
            }
        }
    }
}
