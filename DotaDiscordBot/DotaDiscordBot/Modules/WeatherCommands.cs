using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.Modules
{
    public class WeatherCommands : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string WeatherApiKey = "Api Key";

        [Command("weather")]
        public async Task GetWeatherAsync(params string[]? location)
        {
            var loc = string.Join(' ', location);
            try
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={loc}&appid={WeatherApiKey}&units=metric";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseData);
                    await ReplyAsync($"Weather in {loc}: {weatherData.Weather[0].Main} - {weatherData.Main.Temp}°C");
                }
                else
                {
                    await ReplyAsync("Unable to retrieve weather information.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("An error occurred while retrieving weather information.");
            }
        }
    }
    public class WeatherData
    {
        public Weather[] Weather { get; set; }
        public Main Main { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
    }
    public class Main
    {
        public float Temp { get; set; }
    }
}

