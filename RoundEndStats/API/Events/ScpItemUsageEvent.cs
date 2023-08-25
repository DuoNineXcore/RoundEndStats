using Exiled.API.Features;

namespace RoundEndStats.API.Events
{
    public class ScpItemUsageEvent
    {
        public Player User { get; private set; }
        public ItemType ScpItem { get; private set; }

        public ScpItemUsageEvent(Player user, ItemType scpItem)
        {
            User = user;
            ScpItem = scpItem;
        }

        public override string ToString()
        {
            return $"{User.Nickname} used {ScpItem}";
        }
    }
}
