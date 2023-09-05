using RoundEndStats.API.EventHandlers;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private AchievementTracker achievementTracker = new AchievementTracker();
        private MainEventHandlers mainEventHandlers = new MainEventHandlers();
    }
}
