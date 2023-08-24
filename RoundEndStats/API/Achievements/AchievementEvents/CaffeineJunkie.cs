using Exiled.Events.EventArgs.Player;
using System;
using Exiled.API.Features;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Player colaConsumer = null;
        private DateTime? colaTimerStart = null;

        public void OnItemUsage(UsingItemEventArgs ev)
        {
            if (ev.Player == null)
            {
                Utils.LogMessage($"Player is null in OnItemUsage.", Utils.LogLevel.Error);
                return;
            }

            if (ev.Item.Type == ItemType.SCP207)
            {
                if (colaConsumer == null)
                {
                    colaConsumer = ev.Player;
                    Utils.LogMessage($"{ev.Player.Nickname} started consuming cola.", Utils.LogLevel.Debug);
                }

                if (colaConsumer == ev.Player)
                {
                    if (colaTimerStart == null)
                    {
                        colaTimerStart = DateTime.Now;
                        Utils.LogMessage($"Cola timer started for {ev.Player.Nickname}.", Utils.LogLevel.Debug);
                    }
                }
            }
        }

        public void OnPlayerDeath(DiedEventArgs ev)
        {
            if (ev.Player == null)
            {
                Utils.LogMessage($"Player is null in OnPlayerDeath.", Utils.LogLevel.Error);
                return;
            }

            if (colaConsumer == ev.Player)
            {
                Utils.LogMessage($"{ev.Player.Nickname}, the cola consumer, died.", Utils.LogLevel.Debug);
                colaConsumer = null;
                colaTimerStart = null;
            }
        }

        public void Update()
        {
            if (colaConsumer != null && colaTimerStart.HasValue && DateTime.Now - colaTimerStart.Value >= TimeSpan.FromMinutes(2))
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("Caffeine Junkie", colaConsumer);
                Utils.LogMessage($"{colaConsumer.Nickname} was awarded the 'Caffeine Junkie' achievement.", Utils.LogLevel.Info);
                colaConsumer = null;
                colaTimerStart = null;
            }
        }
    }
}
