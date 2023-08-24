using Exiled.Events.EventArgs.Scp330;
using InventorySystem.Items.Usables.Scp330;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public void OnEatingScp330(EatingScp330EventArgs ev)
        {
            var mainEventHandlers = RoundEndStats.Instance.mainEventHandlers;

            if (ev.Candy is CandyPink)
            {
                Utils.LogMessage($"{ev.Player.Nickname} has eaten a pink candy from SCP-330.", Utils.LogLevel.Debug);

                var explosionKills = mainEventHandlers.killLog.Count(k =>
                    k.AttackerName == ev.Player.Nickname &&
                    k.DamageInfo.Type == Exiled.API.Enums.DamageType.Explosion);

                if (explosionKills >= 5)
                {
                    Utils.LogMessage($"{ev.Player.Nickname} has the required explosion kills after eating the pink candy.", Utils.LogLevel.Debug);
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("Suicide Bomber", ev.Player);
                }
            }
        }
    }
}
