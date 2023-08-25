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

                Utils.LogMessage($"Player {ev.Player.Nickname} used SCP item: {friendlyName}. Added to SCP item usage log.", Utils.LogLevel.Debug);
            }
            else
            {
                Utils.LogMessage($"Player {ev.Player.Nickname} used an item {ev.Item.Type} that is not recognized as an SCP item.", Utils.LogLevel.Warning);
            }
        }
    }
}
