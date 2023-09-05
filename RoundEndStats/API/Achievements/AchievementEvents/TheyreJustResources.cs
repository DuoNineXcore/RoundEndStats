using PluginAPI.Core;
using PluginAPI;
using PluginAPI.Core.Attributes;
using PlayerRoles;
using System.Collections.Generic;
using PlayerStatsSystem;
using PluginAPI.Enums;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, int> scientistKillCount = new Dictionary<Player, int>();

        [PluginEvent(ServerEventType.PlayerDeath)]
        void OnPlayerDied(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (atk.Role == RoleTypeId.Scientist && ply.Role == RoleTypeId.ClassD)
            {
                if (!scientistKillCount.ContainsKey(atk))
                {
                    scientistKillCount[atk] = 0;
                }

                scientistKillCount[atk]++;

                Utils.LogMessage($"Scientist {atk.Nickname} has killed Class-D {ply.Nickname}. Total kills: {scientistKillCount[atk]}", Utils.LogLevel.Debug);

                if (scientistKillCount[atk] >= 5)
                {
                    Utils.LogMessage($"Scientist {atk.Nickname} has reached the required kill count for the achievement 'They're just resources.'", Utils.LogLevel.Debug);
                    achievementTracker.AwardAchievement("They're just resources.", atk);
                    scientistKillCount.Remove(atk);
                }
            }
        }
    }
}
