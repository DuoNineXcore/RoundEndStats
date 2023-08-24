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
            if (ev.Player == null)
            {
                Utils.LogMessage("Player hurt event triggered with null player.", Utils.LogLevel.Error);
                return;
            }

            if (ev.Player.Health - ev.Amount <= 0)
            {
                lastRoleBeforeDeath[ev.Player] = ev.Player.Role.Type;
                Utils.LogMessage($"Stored last role before death for player {ev.Player.Nickname}.", Utils.LogLevel.Debug);
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Player == null || ev.Attacker == null)
            {
                Utils.LogMessage("Player died event triggered with null player or attacker.", Utils.LogLevel.Error);
                return;
            }

            RoleTypeId roleAtDeath = lastRoleBeforeDeath.ContainsKey(ev.Player) ? lastRoleBeforeDeath[ev.Player] : ev.Player.Role.Type;
            killLog.Add(new KillEvent(ev.Attacker.Role.Type, ev.Attacker.Nickname, roleAtDeath, ev.Player.Nickname, ev.DamageHandler));

            Utils.LogMessage($"{ev.Player.Nickname} was killed by {ev.Attacker.Nickname}.", Utils.LogLevel.Debug);

            if (ev.Attacker.Role == RoleTypeId.Scp096 && RoundEndStats.Instance.achievementEvents.playersWhoTriggered096.ContainsKey(ev.Player))
            {
                RoundEndStats.Instance.achievementEvents.playersWhoTriggered096.Remove(ev.Player);
            }

            RoundEndStats.Instance.achievementEvents.OnPlayerKilledZombie(ev);
            RoundEndStats.Instance.achievementEvents.OnPlayerDeath(ev);
            UpdateKills(ev);
        }

        private void UpdateKills(DiedEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player == null)
            {
                Utils.LogMessage("Update kills triggered with null player or attacker.", Utils.LogLevel.Error);
                return;
            }

            if (!playerKillsByRole.ContainsKey(ev.Attacker))
            {
                playerKillsByRole[ev.Attacker] = new Dictionary<RoleTypeId, int>();
            }

            if (!playerKillsByRole[ev.Attacker].ContainsKey(ev.Attacker.Role.Type))
            {
                playerKillsByRole[ev.Attacker][ev.Attacker.Role.Type] = 0;
            }

            playerKillsByRole[ev.Attacker][ev.Attacker.Role.Type]++;
            Utils.LogMessage($"Updated kill count for {ev.Attacker.Nickname}.", Utils.LogLevel.Debug);
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
