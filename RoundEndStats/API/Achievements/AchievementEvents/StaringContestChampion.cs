using Exiled.Events.EventArgs.Scp096;
using System;
using Exiled.API.Features;
using System.Collections.Generic;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public Dictionary<Player, DateTime> playersWhoTriggered096 = new Dictionary<Player, DateTime>();
        private Player playerWhoTriggered096;
        private DateTime survivalEndTime;

        public void OnPlayerTriggered096(EnragingEventArgs ev)
        {
            playerWhoTriggered096 = ev.Player;
            survivalEndTime = DateTime.Now.AddSeconds(20);
            Utils.LogMessage($"{ev.Player.Nickname} has triggered SCP-096.", Utils.LogLevel.Debug);
        }

        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (ev.Target == playerWhoTriggered096)
            {
                DateTime newEndTime = survivalEndTime.AddSeconds(3);

                if ((newEndTime - DateTime.Now).TotalSeconds > 35)
                {
                    newEndTime = DateTime.Now.AddSeconds(35);
                }

                survivalEndTime = newEndTime;
                Utils.LogMessage($"SCP-096 has added {ev.Target.Nickname} as a target. Updated survival end time to {survivalEndTime}.", Utils.LogLevel.Debug);
            }
        }

        public void CheckPlayerSurvival()
        {
            if (playerWhoTriggered096 != null && DateTime.Now >= survivalEndTime)
            {
                if (playerWhoTriggered096.IsAlive)
                {
                    achievementTracker.AwardAchievement("Staring Contest Champion", playerWhoTriggered096);
                    Utils.LogMessage($"{playerWhoTriggered096.Nickname} survived SCP-096's rage and was awarded the 'Staring Contest Champion' achievement.", Utils.LogLevel.Debug);
                }
                playerWhoTriggered096 = null;
            }
        }
    }
}
