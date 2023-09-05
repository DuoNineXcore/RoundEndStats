using PluginAPI.Core;
using System;

namespace RoundEndStats.API.Events
{
    public class ScpItemUsageEvent
    {
        public Player User { get; private set; }
        public ItemType ScpItem { get; private set; }
        public DateTime Timestamp { get; }

        public ScpItemUsageEvent(Player user, ItemType scpItem)
        {
            User = user;
            ScpItem = scpItem;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{User.Nickname} used {ScpItem}";
        }
    }
}
