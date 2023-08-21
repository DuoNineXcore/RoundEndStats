using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Enums;
using PlayerRoles;
using UnityEngine;
using RES.API;

namespace RES
{
    public class EventHandlers
    {
        private Dictionary<Player, int> humanKills = new Dictionary<Player, int>();
        private Dictionary<Player, int> humanDeaths = new Dictionary<Player, int>();
        private Dictionary<Player, int> scpKills = new Dictionary<Player, int>();
        private Dictionary<Player, List<RoleTypeId>> scpTerminations = new Dictionary<Player, List<RoleTypeId>>();

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (var player in Player.List)
            {
                BroadcastPlayerStats(player);
            }
        }

        private void BroadcastPlayerStats(Player player)
        {
            if (player == null)
            {
                Log.Warn("Attempted to broadcast stats to a null player.");
                return;
            }

            int kills = GetHumanKills(player);
            int deaths = GetHumanDeaths(player);
            int matchTime = RoundSummary.roundTime;

            Player topScpPlayer = GetTopScpKiller();
            Player topHumanPlayer = GetTopHumanKiller();

            string topScpName = topScpPlayer?.Nickname ?? "There were no top SCP players this round.";
            string topScpKillsCount = topScpPlayer != null ? scpKills[topScpPlayer].ToString() : "0";

            string topHumanName = topHumanPlayer?.Nickname ?? "There were no top Human players this round.";
            string topHumanKillsCount = topHumanPlayer != null ? humanKills[topHumanPlayer].ToString() : "0";

            string statsMessage = RoundEndStats.Instance.Config.StatsFormat
                .Replace("{playerName}", player.Nickname)
                .Replace("{kills}", kills.ToString())
                .Replace("{deaths}", deaths.ToString())
                .Replace("{matchTime}", matchTime.ToString())
                .Replace("{mvpMessage}", ConstructMvpMessage(GetMVP()))
                .Replace("{topSCPName}", topScpName)
                .Replace("{topSCPKills}", topScpKillsCount)
                .Replace("{topHumanName}", topHumanName)
                .Replace("{topHumanKills}", topHumanKillsCount);

            player.Broadcast(RoundEndStats.Instance.Config.BroadcastDuration, $"<size={RoundEndStats.Instance.Config.BroadcastSize}>{statsMessage}</size>", Broadcast.BroadcastFlags.Normal);
        }

        private string ConstructMvpMessage(Player mvp)
        {
            string mvpRoleColor = API.Utils.ToHex(mvp.Role.Type.GetColor());
            string killTypeMessage = mvp.Role.Team == Team.SCPs ? "SCP kills" : "human kills";
            string terminationMessage = API.Utils.FormatScpTerminations(scpTerminations, mvp);

            if (string.IsNullOrEmpty(terminationMessage))
            {
                return $"MVP: <color={mvpRoleColor}>{mvp.Nickname}</color> with {GetPlayerKills(mvp)} {killTypeMessage}.";
            }
            else
            {
                return $"MVP: <color={mvpRoleColor}>{mvp.Nickname}</color> with {GetPlayerKills(mvp)} {killTypeMessage} and the termination of: {terminationMessage}";
            }
        }

        public int GetPlayerKills(Player player) => API.Utils.GetValueFromDictionary(player, scpKills);
        public int GetHumanKills(Player player) => API.Utils.GetValueFromDictionary(player, humanKills);
        public int GetHumanDeaths(Player player) => API.Utils.GetValueFromDictionary(player, humanDeaths);

        private Player GetTopHumanKiller()
        {
            var topHuman = humanKills.OrderByDescending(k => k.Value).FirstOrDefault();
            if (topHuman.Key == null)
            {
                Log.Warn("No top human killer found.");
            }
            return topHuman.Key;
        }

        private Player GetTopScpKiller()
        {
            var topScp = scpKills.OrderByDescending(k => k.Value).FirstOrDefault();
            if (topScp.Key == null)
            {
                Log.Warn("No top SCP killer found.");
            }
            return topScp.Key;
        }

        public Player GetMVP()
        {
            var combinedKills = scpKills.Concat(humanKills)
                                        .GroupBy(k => k.Key)
                                        .ToDictionary(g => g.Key, g => g.Sum(k => k.Value));

            return combinedKills.OrderByDescending(k => k.Value).FirstOrDefault().Key;
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (ev == null || ev.Attacker == null || ev.Player == null)
            {
                Log.Warn("OnPlayerDied event received with null arguments.");
                return;
            }

            Log.Info($"{ev.Attacker.Nickname} killed {ev.Player.Nickname}");

            UpdateKills(ev);
            UpdateScpTerminations(ev);
            UpdateDeaths(ev);
        }

        private void UpdateKills(DiedEventArgs ev)
        {
            Dictionary<Player, int> killDictionary = ev.Attacker.Role.Team == Team.SCPs ? scpKills : humanKills;
            API.Utils.IncrementValueInDictionary(ev.Attacker, killDictionary);
        }

        private void UpdateScpTerminations(DiedEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.SCPs && ev.Attacker.Role.Team != Team.SCPs)
            {
                if (!scpTerminations.ContainsKey(ev.Attacker))
                    scpTerminations[ev.Attacker] = new List<RoleTypeId>();

                scpTerminations[ev.Attacker].Add(ev.Player.Role.Type);
            }
        }

        private void UpdateDeaths(DiedEventArgs ev)
        {
            if (ev.Player.Role.Team != Team.SCPs)
            {
                API.Utils.IncrementValueInDictionary(ev.Player, humanDeaths);
            }
        }
    }
}
