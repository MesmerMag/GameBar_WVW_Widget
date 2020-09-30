using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameBarWidget.Model
{
    public class Match
    {
        public string Id { get; set; }
        public string Region => Id.Substring(0, 1) == "1" ? "NA" : "EU";
        [JsonPropertyName("start_time")] public DateTime StartTime { get; set; }
        [JsonPropertyName("end_time")] public DateTime EndTime { get; set; }
        public RgbIntValues Scores { get; set; }
        public RgbIntValues Worlds { get; set; }
        [JsonPropertyName("all_worlds")] public AllWorlds AllWorlds { get; set; }
        public RgbIntValues Deaths { get; set; }
        public RgbIntValues Kills { get; set; }

        [JsonPropertyName("victory_points")] public RgbIntValues VictoryPoints { get; set; }
        public List<Skirmish> Skirmishes { get; set; }
    }

    public class RgbIntValues
    {
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
    }

    public class AllWorlds
    {
        public List<int> Red { get; set; }
        public List<int> Green { get; set; }
        public List<int> Blue { get; set; }
    }

    public class Skirmish
    {
        public int Id { get; set; }
        public RgbIntValues Scores { get; set; }
    }
}