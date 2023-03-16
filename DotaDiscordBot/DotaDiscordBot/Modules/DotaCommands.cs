using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class DotaCommands : ModuleBase<SocketCommandContext>
    {
        [Command("randomhero")]
        public async Task RandomHeroAsync(string role)
        {
            await ReplyAsync($"Шукаєм героя на роль {role}...");

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://api.opendota.com/api/heroes");
            var content = await response.Content.ReadAsStringAsync();

            // Parse the JSON response into a JArray
            var heroArray = JArray.Parse(content);

            var filteredHeroes = heroArray.Where(hero =>
            {
                var heroRoles = hero["roles"].ToObject<string[]>();
                return heroRoles.Contains(role);
            }).ToList();

            if (filteredHeroes.Count == 0)
            {
                var roles = new HashSet<string>();
                foreach (var hero in heroArray)
                {
                    foreach (var heroRole in hero["roles"].ToObject<string[]>())
                    {
                        roles.Add(heroRole);
                    }
                }

                await ReplyAsync($"Не знайдено героя на роль {role}.\nCписок усіх можоливих ролей:\n{string.Join("\n", roles)}");
                return;
            }

            var randomHero = filteredHeroes[new Random().Next(filteredHeroes.Count)];

            var heroName = (string)randomHero["localized_name"];

            await ReplyAsync($"Рандомний герой на роль {role}: {heroName}");
        }
    }
}
