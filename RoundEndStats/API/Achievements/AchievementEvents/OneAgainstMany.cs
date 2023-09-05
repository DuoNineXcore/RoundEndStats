using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using System.Collections.Generic;
using PlayerRoles;
using Respawning;
using PlayerStatsSystem;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    /*
    public partial class AchievementEvents
    {
        
        private Dictionary<Player, int> mtfChaosKillCounts = new Dictionary<Player, int>();
        private int totalMtfChaosAtSpawn;

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnMtfChaosRespawn(SpawnableTeamType team)
        {
           totalMtfChaosAtSpawn += team.MaximumRespawnAmount;
           Utils.LogMessage($"MTF/Chaos respawned. Total MTF/Chaos at spawn: {totalMtfChaosAtSpawn}", Utils.LogLevel.Debug);
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerKilledByScp(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (atk == null)
            {
                Utils.LogMessage($"Attacker is null in OnPlayerKilledByScp.", Utils.LogLevel.Error);
                return;
            }

            if (IsMtfOrChaos(ply.Role) && !atk.Role.IsHuman())
            {
                if (!mtfChaosKillCounts.ContainsKey(atk))
                {
                    mtfChaosKillCounts[atk] = 0;
                }

                mtfChaosKillCounts[atk]++;
                Utils.LogMessage($"{atk.Nickname} killed an MTF/Chaos. Total kills by this SCP: {mtfChaosKillCounts[atk]}", Utils.LogLevel.Debug);

                if (mtfChaosKillCounts[atk] >= (2.0 / 3.0) * totalMtfChaosAtSpawn)
                {
                    achievementTracker.AwardAchievement("One Against Many", atk);
                    Utils.LogMessage($"{atk.Nickname} was awarded the 'One Against Many' achievement.", Utils.LogLevel.Info);
                }
            }
        }

        private bool IsMtfOrChaos(RoleTypeId role)
        {
            return role == RoleTypeId.NtfPrivate || role == RoleTypeId.NtfSergeant || role == RoleTypeId.NtfCaptain ||
                   role == RoleTypeId.ChaosRifleman || role == RoleTypeId.ChaosRepressor || role == RoleTypeId.ChaosMarauder;
        }
        
    }
    */
}
