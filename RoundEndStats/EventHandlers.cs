using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;

namespace RES
{
    public class EventHandlers
    {
        private Plugin plugin;
        private Dictionary<Player, int> humanKills = new Dictionary<Player, int>();
        private Dictionary<Player, int> humanDeaths = new Dictionary<Player, int>();
        private Dictionary<Player, int> scpKills = new Dictionary<Player, int>();

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            Player mvp = GetMVP();
            int mvpKills = GetPlayerKills(mvp);
            UnityEngine.Color mvpRoleColor = mvp.Role.Type.GetColor();

            foreach (var player in Player.List)
            {
                int kills = GetHumanKills(player);
                int deaths = GetHumanDeaths(player);
                int matchTime = RoundSummary.roundTime;

                string statsMessage = plugin.Config.StatsFormat
                    .Replace("{playerName}", player.Nickname)
                    .Replace("{kills}", kills.ToString())
                    .Replace("{deaths}", deaths.ToString())
                    .Replace("{matchTime}", matchTime.ToString())
                    .Replace("{mvpName}", $"<color={mvpRoleColor}>{mvp.Nickname}</color>")
                    .Replace("{mvpKills}", mvpKills.ToString());

                player.Broadcast(10, $"<size={plugin.Config.BroadcastSize}>{statsMessage}</size>", Broadcast.BroadcastFlags.Normal);
            }
        }

        public int GetPlayerKills(Player player)
        {
            if (scpKills.ContainsKey(player))
                return scpKills[player];
            return 0;
        }

        public int GetHumanKills(Player player)
        {
            if (humanKills.ContainsKey(player))
                return humanKills[player];
            return 0;
        }

        public int GetHumanDeaths(Player player)
        {
            if (humanDeaths.ContainsKey(player))
                return humanDeaths[player];
            return 0;
        }

        public Player GetMVP()
        {
            var combinedKills = scpKills.Concat(humanKills)
                                        .GroupBy(k => k.Key)
                                        .ToDictionary(g => g.Key, g => g.Sum(k => k.Value));

            return combinedKills.OrderByDescending(k => k.Value).FirstOrDefault().Key;
        }

        public void OnPlayerKilling(KillingPlayerEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.SCPs)
            {
                if (scpKills.ContainsKey(ev.Player))
                    scpKills[ev.Player]++;
                else
                    scpKills[ev.Player] = 1;
            }
            else
            {
                if (humanKills.ContainsKey(ev.Player))
                    humanKills[ev.Player]++;
                else
                    humanKills[ev.Player] = 1;
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev.Player.Role.Team != Team.SCPs)
            {
                if (humanDeaths.ContainsKey(ev.Player))
                    humanDeaths[ev.Player]++;
                else
                    humanDeaths[ev.Player] = 1;
            }
        }
    }
}
