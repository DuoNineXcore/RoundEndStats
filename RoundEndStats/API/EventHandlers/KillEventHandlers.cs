using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundEndStats.API;
using RoundEndStats.API.Events;
using System.Collections.Generic;
using System.Linq;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        public List<KillEvent> killLog = new List<KillEvent>();
        public Dictionary<Player, RoleTypeId> lastRoleBeforeDeath = new Dictionary<Player, RoleTypeId>();
        public Dictionary<Player, Dictionary<RoleTypeId, int>> playerKillsByRole = new Dictionary<Player, Dictionary<RoleTypeId, int>>();

        public void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.Player.Health - ev.Amount <= 0)
            {
                lastRoleBeforeDeath[ev.Player] = ev.Player.Role.Type;
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {

            RoleTypeId roleAtDeath = lastRoleBeforeDeath.ContainsKey(ev.Player) ? lastRoleBeforeDeath[ev.Player] : ev.Player.Role.Type;
            killLog.Add(new KillEvent(ev.Attacker.Role.Type, ev.Attacker.Nickname, roleAtDeath, ev.Player.Nickname, ev.DamageHandler));

            if (ev.Attacker.Role == RoleTypeId.Scp096 && RoundEndStats.Instance.achievementEvents.playersWhoTriggered096.ContainsKey(ev.Player))
            {
                RoundEndStats.Instance.achievementEvents.playersWhoTriggered096.Remove(ev.Player);
            }

            RoundEndStats.Instance.achievementEvents.OnPlayerKilledZombie(ev);
            UpdateKills(ev);
        }

        private void UpdateKills(DiedEventArgs ev)
        {
            if (!playerKillsByRole.ContainsKey(ev.Attacker))
            {
                playerKillsByRole[ev.Attacker] = new Dictionary<RoleTypeId, int>();
                ev.Player.Role.Type.GetFullName();
            }

            if (!playerKillsByRole[ev.Attacker].ContainsKey(ev.Attacker.Role.Type))
            {
                playerKillsByRole[ev.Attacker][ev.Attacker.Role.Type] = 0;
            }

            playerKillsByRole[ev.Attacker][ev.Attacker.Role.Type]++;
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

            return topHuman;
        }

        public Player GetMVP()
        {
            var mvp = killLog.GroupBy(k => k.AttackerName)
                             .OrderByDescending(g => g.Count())
                             .FirstOrDefault()?.Key;

            return Player.Get(mvp);
        }
    }
}
