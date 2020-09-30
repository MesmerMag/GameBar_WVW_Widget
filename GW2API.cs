using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using GameBarWidget.Model;

namespace GameBarWidget
{
    public class GW2API
    {
        public static (Dictionary<int, World> Worlds, List<Match> Matches) FetchData()
        {
            var client = new WebClient();
            var worldsData = client.DownloadString("https://api.guildwars2.com/v2/worlds?ids=all");
            var worldsResult = JsonSerializer.Deserialize<List<World>>(worldsData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var matchesData = client.DownloadString("https://api.guildwars2.com/v2/wvw/matches?ids=all");
            var matchesResult = JsonSerializer.Deserialize<List<Match>>(matchesData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            client.Dispose();

            return (
                Worlds: worldsResult.ToDictionary(
                    world => world.Id,
                    world => new World() {Id = world.Id, Name = world.Name, Population = world.Population}
                ),
                Matches: matchesResult
            );
        }
    }
}