using Exiled.API.Extensions;
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
        }

        public void OnPlayerEscape(EscapingEventArgs ev)
        {
            TimeSpan escapeTime = DateTime.Now - roundStartTime;
            playerEscapeTimes[ev.Player] = escapeTime;

            escapeLog.Add(new EscapeEvent(ev.Player, ev.Player.Role.Type));
        }

        private Player GetFirstPlayerToEscape()
        {
            return escapeLog.OrderBy(e => e.Timestamp).FirstOrDefault()?.Player;
        }

        private int GetTotalEscapes()
        {
            return escapeLog.Count;
        }

        private TimeSpan? GetPlayerEscapeTime(Player player)
        {
            if (playerEscapeTimes.TryGetValue(player, out TimeSpan escapeTime))
            {
                return escapeTime;
            }
            return null;
        }
    }
}
