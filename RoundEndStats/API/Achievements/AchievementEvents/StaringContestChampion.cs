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
            }
        }

        public void CheckPlayerSurvival()
        {
            if (playerWhoTriggered096 != null && DateTime.Now >= survivalEndTime)
            {
                if (playerWhoTriggered096.IsAlive)
                {
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("Staring Contest Champion", playerWhoTriggered096);
                }
                playerWhoTriggered096 = null; 
            }
        }
    }
}

