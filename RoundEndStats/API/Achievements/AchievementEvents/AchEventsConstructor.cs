using RoundEndStats.API.EventHandlers;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private AchievementTracker achievementTracker;
        private MainEventHandlers mainEventHandlers;

        public AchievementEvents(AchievementTracker tracker, MainEventHandlers handlers)
        {
            achievementTracker = tracker;
            mainEventHandlers = handlers;
        }
    }
}
