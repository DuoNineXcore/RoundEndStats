using InventorySystem.Items.Usables;
using PlayerStatsSystem;
using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using System;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Player colaConsumer = null;
        private DateTime? colaTimerStart = null;

        [PluginEvent(ServerEventType.PlayerUseItem)]
        public void OnItemUsage(Player ply, UsableItem item)
        {
            if (ply == null)
            {
                Utils.LogMessage($"Player is null in OnItemUsage.", Utils.LogLevel.Error);
                return;
            }

            if (item.ItemTypeId == ItemType.SCP207)
            {
                if (colaConsumer == null)
                {
                    colaConsumer = ply;
                    Utils.LogMessage($"{ply.Nickname} started consuming cola.", Utils.LogLevel.Debug);
                }

                if (colaConsumer == ply)
                {
                    if (colaTimerStart == null)
                    {
                        colaTimerStart = DateTime.Now;
                        Utils.LogMessage($"Cola timer started for {ply.Nickname}.", Utils.LogLevel.Debug);
                    }
                }
            }
        }

        public void On207PlayerDeath(Player ply, Player atk, DamageHandlerBase dmg)
        {
            if (ply == null)
            {
                Utils.LogMessage($"Player is null in OnPlayerDeath.", Utils.LogLevel.Error);
                return;
            }

            if (colaConsumer == ply)
            {
                Utils.LogMessage($"{ply.Nickname}, the cola consumer, died.", Utils.LogLevel.Debug);
                colaConsumer = null;
                colaTimerStart = null;
            }
        }

        public void Update()
        {
            if (colaConsumer != null && colaTimerStart.HasValue && DateTime.Now - colaTimerStart.Value >= TimeSpan.FromMinutes(2))
            {
                achievementTracker.AwardAchievement("Caffeine Junkie", colaConsumer);
                Utils.LogMessage($"{colaConsumer.Nickname} was awarded the 'Caffeine Junkie' achievement.", Utils.LogLevel.Info);
                colaConsumer = null;
                colaTimerStart = null;
            }
        }
    }
}
