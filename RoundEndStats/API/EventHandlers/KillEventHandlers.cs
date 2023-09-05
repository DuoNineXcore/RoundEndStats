using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using RoundEndStats.API.Events;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using PlayerStatsSystem;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        public List<KillEvent> killLog = new List<KillEvent>();
        public Dictionary<Player, RoleTypeId> lastRoleBeforeDeath = new Dictionary<Player, RoleTypeId>();
        public Dictionary<Player, Dictionary<RoleTypeId, int>> playerKillsByRole = new Dictionary<Player, Dictionary<RoleTypeId, int>>();

        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnPlayerHurt(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (ply == null)
            {
                Utils.LogMessage("Player hurt event triggered with null player.", Utils.LogLevel.Error);
                return;
            }

            if (ply.Health <= 0)
            {
                if (atk != null)
                {
                    lastRoleBeforeDeath[ply] = ply.Role;
                    Utils.LogMessage($"Stored last role before death for player {ply.Nickname}.", Utils.LogLevel.Debug);
                }
            }
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnPlayerDied(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (ply == null || atk == null)
            {
                Utils.LogMessage("Player died event triggered with null player or attacker.", Utils.LogLevel.Error);
                return;
            }

            RoleTypeId roleAtDeath = lastRoleBeforeDeath.ContainsKey(ply) ? lastRoleBeforeDeath[ply] : ply.Role;
            killLog.Add(new KillEvent(atk.Role, atk.Nickname, roleAtDeath, ply.Nickname, dmg));

            Utils.LogMessage($"{ply.Nickname} was killed by {atk.Nickname}.", Utils.LogLevel.Info);

            if (atk.Role == RoleTypeId.Scp096 && achievementEvents.playersWhoTriggered096.ContainsKey(ply))
            {
                achievementEvents.playersWhoTriggered096.Remove(ply);
            }

            if (atk == null || ply == null)
            {
                Utils.LogMessage("Update kills triggered with null player or attacker.", Utils.LogLevel.Error);
                return;
            }

            if (!playerKillsByRole.ContainsKey(atk))
            {
                playerKillsByRole[atk] = new Dictionary<RoleTypeId, int>();
            }

            if (!playerKillsByRole[atk].ContainsKey(atk.Role))
            {
                playerKillsByRole[atk][atk.Role] = 0;
            }

            playerKillsByRole[atk][atk.Role]++;
            Utils.LogMessage($"Updated kill count for {atk.Nickname}.", Utils.LogLevel.Debug);

            achievementEvents.OnPlayerKilledZombie(ply, atk, dmg);
            achievementEvents.On207PlayerDeath(ply, atk, dmg);
        }

        public bool IsRoleTypeSCP(RoleTypeId roleType)
        {
            if (NameFormatter.RoleNameMap.TryGetValue(roleType, out string roleName))
            {
                return roleName.StartsWith("SCP-");
            }
            return false;
        }

        public bool IsRoleTypeHuman(RoleTypeId roleType)
        {
            if (NameFormatter.RoleNameMap.TryGetValue(roleType, out string roleName))
            {
                return !roleName.StartsWith("SCP-") && roleName != "Tutorial" && roleName != "Overwatch" && roleName != "Spectator";
            }
            return false;
        }

        private Player GetTopHumanPlayer()
        {
            Player topHuman = null;
            int maxKills = 0;

            foreach (var player in playerKillsByRole.Keys)
            {
                foreach (var role in playerKillsByRole[player].Keys)
                {
                    if (IsRoleTypeHuman(role) && playerKillsByRole[player][role] > maxKills)
                    {
                        maxKills = playerKillsByRole[player][role];
                        topHuman = player;
                    }
                }
            }

            if (topHuman != null)
            {
                Utils.LogMessage($"{topHuman.Nickname} is the top human player with {maxKills} kills.", Utils.LogLevel.Info);
            }
            else
            {
                Utils.LogMessage("No top human player found.", Utils.LogLevel.Warning);
            }

            return topHuman;
        }

        public Player GetMVP()
        {
            var mvp = killLog.GroupBy(k => k.AttackerName)
                             .OrderByDescending(g => g.Count())
                             .FirstOrDefault()?.Key;

            if (mvp != null)
            {
                Utils.LogMessage($"{mvp} is the MVP.", Utils.LogLevel.Info);
            }
            else
            {
                Utils.LogMessage("No MVP found.", Utils.LogLevel.Warning);
            }

            return Player.Get(mvp);
        }
    }
}
