using Exiled.API.Features;
using RoundEndStats.API.EventHandlers;
using System.Collections.Generic;
using System.Linq;

namespace RoundEndStats.API.Achievements
{
    public class AchievementTracker
    {
        private Player player;
        private Dictionary<string, bool> achievementsUnlocked;
        public MainEventHandlers mainEventHandlers;

        public AchievementTracker(Player player)
        {
            this.player = player;
            achievementsUnlocked = new Dictionary<string, bool>();
            mainEventHandlers = new MainEventHandlers();
        }

        public void TrackSCPDeath(Player killer)
        {
            var scpKills = mainEventHandlers.killLog.Count(k => k.AttackerName == killer.Nickname && mainEventHandlers.IsRoleTypeSCP(k.VictimRole));

            if (scpKills == 1)
            {
                AwardAchievement("SCP-Killer", killer);
            }
            else if (scpKills > 1)
            {
                AwardAchievement("SCP-Killer II", killer);
            }
        }

        public void TrackPinkCandyUsage(Player user, int count)
        {
            if (count >= 5)
            {
                AwardAchievement("Suicide Bomber", user);
            }
        }

        public void TrackSurvivalWithoutKilling(Player survivor)
        {
            var kills = mainEventHandlers.killLog.Count(k => k.AttackerName == survivor.Nickname);

            if (kills == 0)
            {
                AwardAchievement("Peacemaker", survivor);
            }
        }

        public void AwardAchievement(string achievementName, Player player)
        {
            if (!achievementsUnlocked[achievementName])
            {
                achievementsUnlocked[achievementName] = true;

                player.Broadcast(10, $"You have been awarded the \"{achievementName}\" achievement.", Broadcast.BroadcastFlags.Normal, true);
            }
        }
    }
}
