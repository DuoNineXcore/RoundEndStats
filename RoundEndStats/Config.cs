using Exiled.API.Interfaces;
using System.ComponentModel;

namespace RoundEndStats
{
    public class Config : IConfig
    {
        [Description("uhhhhhhhhhhhhh")]
        public bool IsEnabled { get; set; } = true;

        [Description("Console Debug logs.")]
        public bool Debug { get; set; } = true;

        [Description("Duration of the Round End Stats broadcast.")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("The size of the same fucking thing.")]
        public int BroadcastSize { get; set; } = 30;

        [Description("The text that will be displayed in the broadcast.")]
        public string StatsFormat { get; set; } = "<b>{playerName}: Kills - {playerKills}, Deaths - {playerDeaths}\n {mvpMessage}\n Top SCP Killer: {topSCPName} with {topSCPKills} kills\n Top Human Killer: {topHumanName} with {topHumanKills} kills</b>";
    }
}