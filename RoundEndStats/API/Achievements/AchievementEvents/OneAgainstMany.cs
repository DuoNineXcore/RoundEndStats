using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using PlayerRoles;
using Exiled.Events.EventArgs.Server;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, int> mtfChaosKillCounts = new Dictionary<Player, int>();
        private int totalMtfChaosAtSpawn;

        public void OnMtfChaosRespawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox || ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                totalMtfChaosAtSpawn += ev.MaximumRespawnAmount;
            }
        }

        public void OnPlayerKilledByScp(DiedEventArgs ev)
        {
            if (IsMtfOrChaos(ev.Player.Role) && !ev.Attacker.Role.Type.IsHuman())
            {
                if (!mtfChaosKillCounts.ContainsKey(ev.Attacker))
                {
                    mtfChaosKillCounts[ev.Attacker] = 0;
                }

                mtfChaosKillCounts[ev.Attacker]++;

                if (mtfChaosKillCounts[ev.Attacker] >= (2.0 / 3.0) * totalMtfChaosAtSpawn)
                {
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("One Against Many", ev.Attacker);
                }
            }
        }

        private bool IsMtfOrChaos(RoleTypeId role)
        {
            return role == RoleTypeId.NtfPrivate || role == RoleTypeId.NtfSergeant || role == RoleTypeId.NtfCaptain ||
                   role == RoleTypeId.ChaosRifleman || role == RoleTypeId.ChaosRepressor || role == RoleTypeId.ChaosMarauder;
        }

    }
}
