using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using GameBarWidget.Utilities;

namespace GameBarWidget.Model
{
    public class Server
    {
        public string Region { get; set; } // NA or EU
        public int Tier { get; set; }
        public string Color { get; set; } // Server color
        public string Name { get; set; }
        public Color NameBackgroundColor { get; set; }
        public string LinkTooltip { get; set; } // Linked Server tooltip
        public string Rank { get; set; }
        public bool Locked { get; set; }

        public string LockTooltip => Locked ? "Position locked until reset" : "Position can still change before reset";

        public int VP { get; set; } // Victory Points   
        public Color? VPTextColor { get; set; }
        public string VPTooltip { get; set; } // Victory Point tooltip
        public int MaxVP { get; set; } // Highest possible victory points
        public int MinVP { get; set; } // Lowest possible victory points
        public int Score { get; set; } // Current skirmish war score
        public Color NextServerBackgroundColor { get; set; } // Placeholder for next matchup
        public string NextServerName { get; set; } // Placeholder for next matchup

        public static Server CreateFromMatch(Match match, string serverColor, Color displayColor, Dictionary<int, World> worlds)
        {
            string GenerateMatchInfoTip(IEnumerable<int> list)
            {
                return list.Reverse().Aggregate(
                    "",
                    (current, id) =>
                    {
                        if (worlds.ContainsKey(id))
                            return current + (worlds[id].Name.PadRight(25) + "\t" + worlds[id].Population + "\r\n");

                        return current + ("Unknown Server " + id).PadRight(25) + "\t ---\r\n";
                    }).Trim();
            }

            var vp = ObjectUtil.GetPropertyValue<int>(match.VictoryPoints, serverColor);
            var maxVP = vp + ((85 - match.Skirmishes.Count) * 5);
            var minVP = vp + ((85 - match.Skirmishes.Count) * 3);

            return new Server()
            {
                Region = match.Region,
                Tier = int.Parse(match.Id.Substring(match.Id.Length - 1, 1)),
                Color = serverColor,
                Name = worlds[ObjectUtil.GetPropertyValue<int>(match.Worlds, serverColor)].Name,
                NameBackgroundColor = displayColor,
                LinkTooltip = GenerateMatchInfoTip(ObjectUtil.GetPropertyValue<IEnumerable<int>>(match.AllWorlds, serverColor)),
                VP = vp,
                VPTextColor = new UISettings().GetColorValue(UIColorType.Foreground),
                VPTooltip = "Highest\t " + maxVP.ToString() + "\r\n" + "Lowest\t " + minVP.ToString(),
                MaxVP = maxVP,
                MinVP = minVP,
                Score = ObjectUtil.GetPropertyValue<int>(match.Skirmishes.LastOrDefault()?.Scores, serverColor),
                // determined later:
                Rank = "",
                Locked = false,
                NextServerBackgroundColor = displayColor,
                NextServerName = worlds[ObjectUtil.GetPropertyValue<int>(match.Worlds, serverColor)].Name,
            };
        }
    }
}