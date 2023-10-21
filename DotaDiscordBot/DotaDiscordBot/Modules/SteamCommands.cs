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
        [Command("get-info")]

        public async Task SteamViewer (string id)
        {
            await ReplyAsync($"Search user info....");

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://api.opendota.com/api/players/{id}/wl");
                var content = await response.Content.ReadAsStringAsync();
                dynamic WinLoseData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (response.IsSuccessStatusCode && (WinLoseData.win != 0 && WinLoseData.lose != 0))
                {
                    response = await httpClient.GetAsync($"https://api.opendota.com/api/players/{id}");
                    content = await response.Content.ReadAsStringAsync();
                    dynamic PlayerData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                    double WinRate = Math.Round
                        (
                        ((double)WinLoseData.win / ((double)WinLoseData.win + (double)WinLoseData.lose)) * 100.0, 2
                        );

                    await ReplyAsync($"A user with name {PlayerData.profile.personaname}" +
                        $" was found:\nHis MMR = {PlayerData.mmr_estimate.estimate}" +
                        $"\nWin: {WinLoseData.win}\nlose: {WinLoseData.lose}\nWinRate = {WinRate}%");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"User with id: {id} not found, or you entered an invalid id, or no games have been played on the account");
            }
        }
    }
}
