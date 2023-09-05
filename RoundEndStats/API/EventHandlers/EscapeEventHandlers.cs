using PlayerRoles;
using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
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

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            roundStartTime = DateTime.Now;
            Utils.LogMessage("Round started. Setting initial round start time.", Utils.LogLevel.Debug);
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public void OnPlayerEscape(Player plr, RoleTypeId role)
        {
            if (plr == null)
            {
                Utils.LogMessage("Player escape event triggered with null player.", Utils.LogLevel.Error);
                return;
            }

            TimeSpan escapeTime = DateTime.Now - roundStartTime;
            playerEscapeTimes[plr] = escapeTime;

            escapeLog.Add(new EscapeEvent(plr, role));
            Utils.LogMessage($"{plr.Nickname} has escaped. Logging escape event.", Utils.LogLevel.Debug);
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
            Utils.LogMessage($"Total escapes this round: {totalEscapes}.", Utils.LogLevel.Info);
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
                Utils.LogMessage($"Retrieved escape time for {player.Nickname}: {escapeTime}.", Utils.LogLevel.Info);
                return escapeTime;
            }

            Utils.LogMessage($"{player.Nickname} has not escaped yet.", Utils.LogLevel.Warning);
            return null;
        }
    }
}
