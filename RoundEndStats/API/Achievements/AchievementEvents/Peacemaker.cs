using Exiled.API.Features;
using System.Linq;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public void TrackSurvivalWithoutKilling(Player survivor)
        {
            var mainEventHandlers = RoundEndStats.Instance.mainEventHandlers;
            var kills = mainEventHandlers.killLog.Count(k => k.AttackerName == survivor.Nickname);

            if (kills == 0)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("Peacemaker", survivor);
            }
        }
    }
}
