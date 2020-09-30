using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GameBarWidget.Model;
using GameBarWidget.Utilities;

namespace GameBarWidget
{
    public sealed partial class Widget : Page
    {
        private (Dictionary<int, World> Worlds, List<Match> Matches) _apiResponse;
        public readonly WidgetViewModel ViewModel;

        public Widget()
        {
            InitializeComponent();
            ViewModel = new WidgetViewModel();
            FetchData();
            RegionRadioNA.IsChecked = true;
            Repaint();
        }

        private void FetchData()
        {
            _apiResponse = GW2API.FetchData();
        }

        private void Repaint()
        {
            var greenColor = Colors.DarkGreen;
            var blueColor = Colors.DarkBlue;
            var redColor = Colors.DarkRed;

            var tiers =
                _apiResponse.Matches
                    .Where(match => match.Region == ViewModel.Region)
                    .Select(match => new List<Server>
                        {
                            Server.CreateFromMatch(match, "Green", greenColor, _apiResponse.Worlds),
                            Server.CreateFromMatch(match, "Blue", blueColor, _apiResponse.Worlds),
                            Server.CreateFromMatch(match, "Red", redColor, _apiResponse.Worlds)
                        }
                        .OrderByDescending(s => s.VP)
                        .ThenByDescending(s => s.Score)
                        .Select((s, idx) =>
                        {
                            s.Rank = (idx + 1) + IntegerUtil.GetOrdinalSuffix(idx + 1);
                            switch (idx)
                            {
                                case 0:
                                    s.NextServerBackgroundColor = greenColor;
                                    break;
                                case 1:
                                    s.NextServerBackgroundColor = blueColor;
                                    break;
                                case 2:
                                    s.NextServerBackgroundColor = redColor;
                                    break;
                            }

                            return s;
                        })
                        .ToList())
                    .Select(servers => new Tier {TierNum = servers[0].Tier, Servers = servers,})
                    .ToList();

            var numTiers = tiers.Count;
            for (var tierIdx = 0; tierIdx < numTiers; tierIdx++)
            {
                var tier = tiers[tierIdx];
                // Prevent IDE's nullable warning:
                if (tier == null)
                {
                    continue;
                }

                var nextTier = tiers.ElementAtOrDefault(tierIdx + 1);
                var isLastTier = tier == tiers.LastOrDefault();

                for (var serverIdx = 0; serverIdx < tier.Servers.Count; serverIdx++)
                {
                    var server = tier.Servers[serverIdx];
                    // Prevent IDE's nullable warning:
                    if (server == null)
                    {
                        continue;
                    }

                    var prevServer = tier.Servers.ElementAtOrDefault(serverIdx - 1);
                    var nextServer = tier.Servers.ElementAtOrDefault(serverIdx + 1);
                    var nextServer2 = tier.Servers.ElementAtOrDefault(serverIdx + 2);

                    // Figure out if positions are locked:

                    var locked = false;

                    if (prevServer != null && nextServer != null && server.MinVP > nextServer.MaxVP && server.MaxVP < prevServer.MinVP)
                    {
                        locked = true;
                    }
                    else if (prevServer != null && server.MaxVP < prevServer.MinVP)
                    {
                        locked = true;
                    }
                    else if (nextServer != null && server.MinVP > nextServer.MaxVP)
                    {
                        locked = true;
                    }

                    server.Locked = locked;

                    // Figure out next matchup:

                    var isLosingServer = server == tier.Servers.LastOrDefault();

                    if (nextServer == null && nextTier != null)
                    {
                        nextServer = nextTier.Servers[0];
                    }

                    if (nextServer2 == null && nextTier != null)
                    {
                        nextServer2 = nextTier.Servers[1];
                    }

                    // If VP is tied with the next server, change the VP text color
                    if (nextServer != null && server.VP == nextServer.VP)
                    {
                        server.VPTextColor = Colors.Salmon;
                        nextServer.VPTextColor = Colors.Salmon;
                    }

                    // ReSharper disable once InvertIf
                    // Figure out who the servers are going to be in the next matchup.
                    if (
                        // Swap this tier's losing server with the next tier's winning server
                        isLosingServer &&
                        // unless it's the last tier
                        !isLastTier &&
                        // unless there is no next server (shouldn't happen)
                        nextServer != null &&
                        // unless there's a tie between the next tier's first and second place servers
                        (nextServer2 == null || nextServer.VP != nextServer2.VP)
                    )
                    {
                        nextServer.NextServerName = server.Name;
                        server.NextServerName = nextServer.Name;
                    }
                }
            }

            ViewModel.Tiers = tiers;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
            Repaint();
        }

        private void RegionRadio_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.Region = (sender as RadioButton)?.Content?.ToString();
            Repaint();
        }
    }
}