﻿using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundEndStats.API;
using RoundEndStats.API.Achievements;
using RoundEndStats.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        private List<EscapeEvent> escapeLog = new List<EscapeEvent>();
        private Dictionary<Player, TimeSpan> playerEscapeTimes = new Dictionary<Player, TimeSpan>();
        private DateTime roundStartTime;

        public void OnRoundStart()
        {
            roundStartTime = DateTime.Now;
            Utils.LogMessage("Round started. Setting initial round start time.", Utils.LogLevel.Debug);
        }

        public void OnPlayerEscape(EscapingEventArgs ev)
        {
            if (ev.Player == null)
            {
                Utils.LogMessage("Player escape event triggered with null player.", Utils.LogLevel.Error);
                return;
            }

            TimeSpan escapeTime = DateTime.Now - roundStartTime;
            playerEscapeTimes[ev.Player] = escapeTime;

            escapeLog.Add(new EscapeEvent(ev.Player, ev.Player.Role.Type));
            Utils.LogMessage($"{ev.Player.Nickname} has escaped. Logging escape event.", Utils.LogLevel.Debug);
        }

        private Player GetFirstPlayerToEscape()
        {
            var firstEscapee = escapeLog.OrderBy(e => e.Timestamp).FirstOrDefault()?.Player;

            if (firstEscapee != null)
            {
                Utils.LogMessage($"{firstEscapee.Nickname} is the first player to escape.", Utils.LogLevel.Info);
            }
            else
            {
                Utils.LogMessage("No player has escaped yet.", Utils.LogLevel.Warning);
            }

            return firstEscapee;
        }

        private int GetTotalEscapes()
        {
            int totalEscapes = escapeLog.Count;
            Utils.LogMessage($"Total escapes this round: {totalEscapes}.", Utils.LogLevel.Debug);
            return totalEscapes;
        }

        private TimeSpan? GetPlayerEscapeTime(Player player)
        {
            if (player == null)
            {
                Utils.LogMessage("Attempted to get escape time for null player.", Utils.LogLevel.Error);
                return null;
            }

            if (playerEscapeTimes.TryGetValue(player, out TimeSpan escapeTime))
            {
                Utils.LogMessage($"Retrieved escape time for {player.Nickname}: {escapeTime}.", Utils.LogLevel.Debug);
                return escapeTime;
            }

            Utils.LogMessage($"{player.Nickname} has not escaped yet.", Utils.LogLevel.Warning);
            return null;
        }
    }
}
