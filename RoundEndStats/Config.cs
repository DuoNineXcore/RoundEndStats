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

        public ushort BroadcastDuration { get; set; } = 10;

        public int BroadcastSize { get; set; } = 30;

        public string StatsFormat { get; set; } =
            "<b>{playerName}: Kills - {kills}, Deaths - {deaths}\n" +
            "{mvpMessage}\n" +
            "Top SCP Killer: {topSCPName} with {topSCPKills} kills\n" +
            "Top Human Killer: {topHumanName} with {topHumanKills} kills</b>";

    }
}