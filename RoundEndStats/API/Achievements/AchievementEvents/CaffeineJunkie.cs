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
            if (ev.Item.Type == ItemType.SCP207)
            {
                if (colaConsumer == null)
                {
                    colaConsumer = ev.Player;
                }

                if (colaConsumer == ev.Player)
                {
                    if (colaTimerStart == null)
                    {
                        colaTimerStart = DateTime.Now;
                    }
                }
            }
        }

        public void OnPlayerDeath(DiedEventArgs ev)
        {
            if (colaConsumer == ev.Player)
            {
                colaConsumer = null;
                colaTimerStart = null;
            }
        }

        public void Update()
        {
            if (colaConsumer != null && colaTimerStart.HasValue && DateTime.Now - colaTimerStart.Value >= TimeSpan.FromMinutes(2))
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("Caffeine Junkie", colaConsumer);
                colaConsumer = null;
                colaTimerStart = null;
            }
        }
    }
}
