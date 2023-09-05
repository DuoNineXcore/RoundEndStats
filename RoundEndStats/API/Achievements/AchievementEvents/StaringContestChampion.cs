using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;
using System.Collections.Generic;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public Dictionary<Player, DateTime> playersWhoTriggered096 = new Dictionary<Player, DateTime>();
        private Player playerWhoTriggered096;
        private DateTime survivalEndTime;

        [PluginEvent(ServerEventType.Scp096Enraging)]
        public void OnPlayerTriggered096(Player ply, float initialDuration)
        {
            playerWhoTriggered096 = ply;
            survivalEndTime = DateTime.Now.AddSeconds(20);
            Utils.LogMessage($"{ply.Nickname} has triggered SCP-096.", Utils.LogLevel.Debug);
        }

        [PluginEvent(ServerEventType.Scp096AddingTarget)]
        public void OnAddingTarget(Player ply, Player atk, bool isForLooking)
        {
            if (ply == playerWhoTriggered096)
            {
                DateTime newEndTime = survivalEndTime.AddSeconds(3);

                if ((newEndTime - DateTime.Now).TotalSeconds > 35)
                {
                    newEndTime = DateTime.Now.AddSeconds(35);
                }

                survivalEndTime = newEndTime;
                Utils.LogMessage($"SCP-096 has added {ply.Nickname} as a target. Updated survival end time to {survivalEndTime}.", Utils.LogLevel.Debug);
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
