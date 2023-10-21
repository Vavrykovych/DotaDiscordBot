using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class WinRateMatchesCommands : ModuleBase<SocketCommandContext>
    {
        [Command("get-win/rate")]
        public async Task GetWinRateUser(string id)
        {
            await ReplyAsync($"Search Win/Rate user....");

            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://api.opendota.com/api/players/{id}/wl");
                var content = await response.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                if (response.IsSuccessStatusCode && (data.win != 0 && data.lose != 0))
                {
                    response = await httpClient.GetAsync($"https://api.opendota.com/api/players/{id}");
                    content = await response.Content.ReadAsStringAsync();
                    dynamic PlayerData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                    double WinRate = Math.Round(((double)data.win / ((double)data.win + (double)data.lose)) * 100.0, 2);
                    await ReplyAsync($"A user with name {PlayerData.profile.personaname} was found:\nWin: {data.win}\nlose: {data.lose}\nWinRate = {WinRate}%");
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
