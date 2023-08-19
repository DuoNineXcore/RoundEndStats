using Exiled.API.Interfaces;
using System.ComponentModel;

namespace RES
{
    public class Config : IConfig
    {
        [Description("what this shit do")]
        public bool IsEnabled { get; set; } = true;
        
        [Description("Console debug logs")]
        public bool Debug { get; set; } = false;

        public bool DisplayKills { get; set; } = true;

        public bool DisplayDeaths { get; set; } = true;

        public bool DisplayObjectives { get; set; } = true;

        public bool DisplaySurvivalTime { get; set; } = true;

        public int BroadcastSize { get; set; } = 15;

        public string StatsFormat { get; set; } = "{playerName}: Kills - {kills}, Deaths - {deaths}, Round Time - {time}";
    }
}