using Exiled.API.Features;
using PlayerRoles;
using System;

namespace RoundEndStats.API.Events
{
    public class EscapeEvent
    {
        public Player Player { get; }
        public RoleTypeId RoleAtEscape { get; }
        public DateTime Timestamp { get; }

        public EscapeEvent(Player player, RoleTypeId roleAtEscape)
        {
            Player = player;
            RoleAtEscape = roleAtEscape;
            Timestamp = DateTime.Now;
        }
    }

}
