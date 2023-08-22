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
        private Dictionary<Player, int> kills = new Dictionary<Player, int>();
        private Dictionary<Player, int> deaths = new Dictionary<Player, int>();
        private Dictionary<Player, List<Tuple<RoleTypeId, string>>> scpTerminations = new Dictionary<Player, List<Tuple<RoleTypeId, string>>>();
        private Dictionary<Player, RoleTypeId> lastRoleBeforeDeath = new Dictionary<Player, RoleTypeId>();
        private API.Utils _utils;

        public EventHandlers()
        {
            _utils = new API.Utils();
        }

        public void OnWaiting()
        {
            kills.Clear();
            deaths.Clear();
            scpTerminations.Clear();
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (var player in Player.List)
            {
                BroadcastPlayerStats(player);
            }
        }

        private void BroadcastPlayerStats(Player player)
        {
            _utils.LogMessage("BroadcastPlayerStats started.", ConsoleColor.Yellow);

            if (player == null)
            {
                _utils.LogMessage("Attempted to broadcast stats to a null player.", ConsoleColor.Cyan);
                return;
            }

            int playerKills = GetPlayerKills(player);
            int deaths = GetPlayerDeaths(player);
            int matchTime = RoundSummary.roundTime;

            _utils.LogMessage($"Player: {player.Nickname}, Kills: {playerKills}, Deaths: {deaths}, MatchTime: {matchTime}", ConsoleColor.Yellow);

            var sortedPlayersByKills = kills.OrderByDescending(k => k.Value).Select(k => k.Key).ToList();

            Player topKiller = sortedPlayersByKills.FirstOrDefault();
            Player topScpPlayer = sortedPlayersByKills.FirstOrDefault(p => p.Role.Team == Team.SCPs);
            Player topHumanPlayer = sortedPlayersByKills.FirstOrDefault(p => p.Role.Team != Team.SCPs);

            _utils.LogMessage($"Top SCP Player: {topScpPlayer?.Nickname}, Top Human Player: {topHumanPlayer?.Nickname}", ConsoleColor.Yellow);

            string topScpRoleName = topScpPlayer != null
                ? (RoleNameFormatter.RoleNameMap.TryGetValue(topScpPlayer.Role.Type, out var scpName)
                    ? scpName
                    : topScpPlayer.Role.Type.ToString())
                : "Unknown SCP";
            string topScpName = topScpPlayer != null
                ? $"<color=red>{topScpPlayer.Nickname}</color>"
                : "There were no top SCP players this round.";
            string topScpKillsCount = topScpPlayer != null ? kills[topScpPlayer].ToString() : "0";

            string topHumanRoleColor = topHumanPlayer?.Role.Type.GetColor().ToHex() ?? "#FFFFFF";

            string topHumanRoleName = topHumanPlayer != null
                ? (RoleNameFormatter.RoleNameMap.TryGetValue(topHumanPlayer.Role.Type, out var humanName)
                    ? humanName
                    : topHumanPlayer.Role.Type.ToString())
                : "Unknown Role";

            string topHumanName = topHumanPlayer != null
                ? $"<color={topHumanRoleColor}>{topHumanPlayer.Nickname}</color>"
                : "There were no top Human players this round.";

            string topHumanKillsCount = topHumanPlayer != null ? kills[topHumanPlayer].ToString() : "0";

            string topKillerRoleName = topKiller != null
                ? (RoleNameFormatter.RoleNameMap.TryGetValue(topKiller.Role.Type, out var killerName)
                ? killerName
                : topKiller.Role.Type.ToString())
                : "Unknown Role";

            string topKillerRoleColor = topKiller?.Role.Type.GetColor().ToHex() ?? "#FFFFFF"; 

            string topKillerNameColored = topKiller != null
                ? $"<color={topKillerRoleColor}>{topKiller.Nickname} [{topKillerRoleName}]</color>"
                : "There was no top killer this round.";


            _utils.LogMessage($"Top SCP Name: {topScpName}, Top Human Name: {topHumanName}", ConsoleColor.Yellow);

            string statsMessage = RoundEndStats.Instance.Config.StatsFormat
                .Replace("{playerName}", player.Nickname)
                .Replace("{playerKills}", playerKills.ToString())
                .Replace("{TopKiller}", topKillerNameColored)
                .Replace("{TopKillerRole}", topKillerRoleName)
                .Replace("{playerDeaths}", deaths.ToString())
                .Replace("{matchTime}", matchTime.ToString())
                .Replace("{mvpMessage}", ConstructMvpMessage(GetMVP()))
                .Replace("{topSCPName}", topScpName)
                .Replace("{topSCPKills}", topScpKillsCount)
                .Replace("{topHumanName}", topHumanName)
                .Replace("{topHumanKills}", topHumanKillsCount)
                .Replace("{topSCPRole}", topScpRoleName)
                .Replace("{topHumanRole}", topHumanRoleName);

            _utils.LogMessage($"Stats Message: {statsMessage}", ConsoleColor.Yellow);

            player.Broadcast(RoundEndStats.Instance.Config.BroadcastDuration, $"<size={RoundEndStats.Instance.Config.BroadcastSize}>{statsMessage}</size>", Broadcast.BroadcastFlags.Normal);

            _utils.LogMessage("BroadcastPlayerStats completed.", ConsoleColor.Yellow);
        }

        private string ConstructMvpMessage(Player mvp)
        {
            string mvpRoleColor = _utils.ToHex(mvp.Role.Type.GetColor());
            string terminationMessage = _utils.FormatScpTerminations(scpTerminations, mvp);

            if (string.IsNullOrEmpty(terminationMessage))
            {
                _utils.LogMessage($"MVP {mvp.Nickname} had no SCP terminations.", ConsoleColor.DarkGreen);
                return $"MVP: <color={mvpRoleColor}>{mvp.Nickname}</color> with {GetPlayerKills(mvp)} kills.";
            }
            else
            {
                _utils.LogMessage($"MVP {mvp.Nickname} terminated: {terminationMessage}", ConsoleColor.DarkGreen);
                return $"MVP: <color={mvpRoleColor}>{mvp.Nickname}</color> with {GetPlayerKills(mvp)} kills and the termination of {terminationMessage}";
            }
        }

        public void OnPlayerHurt(HurtingEventArgs ev)
        {
            if (ev.Player.Health - ev.Amount <= 0) // This is a fatal damage
            {
                lastRoleBeforeDeath[ev.Player] = ev.Player.Role.Type;
            }
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            RoleTypeId roleAtDeath = lastRoleBeforeDeath.ContainsKey(ev.Player) ? lastRoleBeforeDeath[ev.Player] : ev.Player.Role.Type;

            if (IsRoleTypeSCP(roleAtDeath) && ev.Attacker.Role.Team != Team.SCPs)
            {
                if (!scpTerminations.ContainsKey(ev.Attacker))
                    scpTerminations[ev.Attacker] = new List<Tuple<RoleTypeId, string>>();

                scpTerminations[ev.Attacker].Add(new Tuple<RoleTypeId, string>(roleAtDeath, ev.Player.Nickname));
            }

            _utils.LogMessage($"{ev.Player.Nickname} was a {ev.Player.Role.Type} when they died.", ConsoleColor.DarkCyan);

            if (ev == null || ev.Attacker == null || ev.Player == null)
            {
                _utils.LogMessage("OnPlayerDied event received with null arguments.", ConsoleColor.Cyan);
                return;
            }

            _utils.LogMessage($"{ev.Attacker.Nickname} killed {ev.Player.Nickname}", ConsoleColor.DarkCyan);

            UpdateKills(ev);
            UpdateScpTerminations(ev);
            UpdateDeaths(ev);
        }

        private bool IsRoleTypeSCP(RoleTypeId roleType)
        {
            List<RoleTypeId> scpRoles = new List<RoleTypeId>
            {
                RoleTypeId.Scp049,
                RoleTypeId.Scp079,
                RoleTypeId.Scp096,
                RoleTypeId.Scp106,
                RoleTypeId.Scp173,
                RoleTypeId.Scp939
            };

            return scpRoles.Contains(roleType);
        }

        public int GetPlayerKills(Player player) => _utils.GetValueFromDictionary(player, kills);
        public int GetPlayerDeaths(Player player) => _utils.GetValueFromDictionary(player, deaths);

        public Player GetMVP()
        {
            foreach (var player in scpTerminations.Keys)
            {
                if (scpTerminations[player].Any())
                {
                    return player;
                }
            }
            var mvp = kills.OrderByDescending(k => k.Value).FirstOrDefault().Key;
            if (mvp == null)
            {
                _utils.LogMessage("No MVP found.", ConsoleColor.Red);
            }
            return mvp;
        }

        private void UpdateKills(DiedEventArgs ev)
        {
            _utils.IncrementValueInDictionary(ev.Attacker, kills);
        }

        private void UpdateScpTerminations(DiedEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.SCPs && ev.Attacker.Role.Team != Team.SCPs)
            {
                if (!scpTerminations.ContainsKey(ev.Attacker))
                    scpTerminations[ev.Attacker] = new List<Tuple<RoleTypeId, string>>();

                scpTerminations[ev.Attacker].Add(new Tuple<RoleTypeId, string>(ev.Player.Role.Type, ev.Player.Nickname));

                _utils.LogMessage($"{ev.Attacker.Nickname} terminated SCP: {ev.Player.Nickname} [{ev.Player.Role.Type}]", ConsoleColor.DarkCyan);
            }
            else
            {
                _utils.LogMessage($"{ev.Attacker.Nickname} did not terminate an SCP. Killed: {ev.Player.Nickname} [{ev.Player.Role.Type}]", ConsoleColor.DarkCyan);
            }
        }

        private void UpdateDeaths(DiedEventArgs ev)
        {
            if (ev.Player.Role.Team != Team.SCPs)
            {
                _utils.IncrementValueInDictionary(ev.Player, deaths);
            }
        }
    }
}
