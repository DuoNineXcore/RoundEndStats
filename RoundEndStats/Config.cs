using System.ComponentModel;

namespace RoundEndStats
{
    public class Config
    {
        [Description("uhhhhhhhhhhhhh")]
        public bool IsEnabled { get; set; } = true;

        [Description("Console Debug logs.")]
        public bool Debug { get; set; } = true;

        [Description("Should it be a hint?")]
        public bool IsHint { get; set; } = false;

        [Description("Duration of the Round End Stats broadcast.")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("The Size of the broadcast")]
        public int BroadcastSize { get; set; } = 30;

        [Description("The text that will be displayed in the broadcast.")]
        public string StatsFormat { get; set; } = "<b>{playerName}: Kills - {playerKills}, Deaths - {playerDeaths}\n Top SCP Killer: {topSCPName} with {topSCPKills} kills\n Top Human Killer: {topHumanName} with {topHumanKills} kills</b>";
    }
}