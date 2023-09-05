using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Enums;
using InventorySystem.Items.Usables.Scp330;
using System.Linq;
using PluginAPI.Core.Attributes;
using InventorySystem.Items;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    /*
    public partial class AchievementEvents
    {
        [PluginEvent(ServerEventType.PlayerUsedItem)]
        public void OnEatingScp330(Player plr, ItemBase item)
        {
            var mainEventHandlers = RoundEndStats.Instance.mainEventHandlers;

            if (item.Candy is CandyPink)
            {
                Utils.LogMessage($"{ev.Player.Nickname} has eaten a pink candy from SCP-330.", Utils.LogLevel.Debug);

                var explosionKills = mainEventHandlers.killLog.Count(k =>
                    k.AttackerName == ev.Player.Nickname &&
                    k.DamageInfo.Type == Exiled.API.Enums.DamageType.Explosion);

                if (explosionKills >= 5)
                {
                    Utils.LogMessage($"{ev.Player.Nickname} has the required explosion kills after eating the pink candy.", Utils.LogLevel.Debug);
                    achievementTracker.AwardAchievement("Suicide Bomber", ev.Player);
                }
            }
        }
    }
    */
}
