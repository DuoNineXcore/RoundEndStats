using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using System.Collections.Generic;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public Dictionary<Player, int> pocketDimensionEscapes = new Dictionary<Player, int>();

        [PluginEvent(ServerEventType.PlayerExitPocketDimension)]
        public void OnPlayerEscapedPocketDimension(Player ply, bool success)
        {
            if (ply == null)
            {
                Utils.LogMessage($"Player is null in OnPlayerEscapedPocketDimension.", Utils.LogLevel.Error);
                return;
            }

            if (!pocketDimensionEscapes.ContainsKey(ply))
            {
                pocketDimensionEscapes[ply] = 0;
            }

            pocketDimensionEscapes[ply]++;
            Utils.LogMessage($"{ply.Nickname} escaped from the pocket dimension. Total escapes: {pocketDimensionEscapes[ply]}", Utils.LogLevel.Debug);

            if (pocketDimensionEscapes[ply] == 2)
            {
                achievementTracker.AwardAchievement("Dimensional Dodger", ply);
                Utils.LogMessage($"{ply.Nickname} was awarded the 'Dimensional Dodger' achievement.", Utils.LogLevel.Info);
            }
        }
    }
}
