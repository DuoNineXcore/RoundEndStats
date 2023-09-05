using PluginAPI;
using PluginAPI.Enums;
using PluginAPI.Core;
using RoundEndStats.API.Events;
using System.Collections.Generic;
using PluginAPI.Core.Attributes;
using InventorySystem.Items;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        private List<ScpItemUsageEvent> scpItemUsageLog = new List<ScpItemUsageEvent>();

        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void OnUsedItem(Player plr, ItemBase ev)
        {
            if (NameFormatter.SCPItemNameMap.ContainsKey(ev.ItemTypeId))
            {
                string friendlyName = NameFormatter.GetFriendlySCPItemName(ev.ItemTypeId);

                ScpItemUsageEvent scpItemUsageEvent = new ScpItemUsageEvent(plr, ev.ItemTypeId);

                scpItemUsageLog.Add(scpItemUsageEvent);

                Utils.LogMessage($"Player {plr.Nickname} used SCP item: {friendlyName}. Added to SCP item usage log.", Utils.LogLevel.Debug);
            }
            else
            {
                Utils.LogMessage($"Player {plr.Nickname} used an item {ev.ItemTypeId} that is not recognized as an SCP item.", Utils.LogLevel.Warning);
            }
        }
    }
}
