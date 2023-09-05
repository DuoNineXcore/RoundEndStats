using System.Collections.Generic;
using PluginAPI.Core;
using PluginAPI;
using PluginAPI.Core.Attributes;
using PlayerRoles;
using PluginAPI.Enums;
using PlayerStatsSystem;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, int> zombieKillCounts = new Dictionary<Player, int>();

        public void OnPlayerKilledZombie(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (ply == null || atk == null)
            {
                Utils.LogMessage("Either the victim or the attacker is null in OnPlayerKilledZombie.", Utils.LogLevel.Error);
                return;
            }

            if (ply.Role == RoleTypeId.Scp0492)
            {
                if (!zombieKillCounts.ContainsKey(atk))
                {
                    zombieKillCounts[atk] = 0;
                }

                zombieKillCounts[atk]++;

                Utils.LogMessage($"{atk.Nickname} has killed {zombieKillCounts[atk]} zombies.", Utils.LogLevel.Debug);

                if (zombieKillCounts[atk] == 5)
                {
                    achievementTracker.AwardAchievement("Zombie Slayer", atk);
                    Utils.LogMessage($"{atk.Nickname} has been awarded the 'Zombie Slayer' achievement.", Utils.LogLevel.Info);
                }
            }
        }
    }
}
