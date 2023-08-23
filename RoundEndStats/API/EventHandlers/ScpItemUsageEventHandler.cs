using Exiled.Events.EventArgs.Player;
using RoundEndStats.API.Events;
using System.Collections.Generic;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        private List<ScpItemUsageEvent> scpItemUsageLog = new List<ScpItemUsageEvent>();

        public void OnUsedItem(UsedItemEventArgs ev)
        {
            if (NameFormatter.SCPItemNameMap.ContainsKey(ev.Item.Type))
            {
                string friendlyName = NameFormatter.GetFriendlySCPItemName(ev.Item.Type);

                ScpItemUsageEvent scpItemUsageEvent = new ScpItemUsageEvent(ev.Player, ev.Item.Type);

                scpItemUsageLog.Add(scpItemUsageEvent);
            }
        }
    }
}
