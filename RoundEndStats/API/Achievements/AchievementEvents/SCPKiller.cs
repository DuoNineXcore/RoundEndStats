using PlayerRoles;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public void TrackSCPDeath(Player killer)
        {
            var mainEventHandlers = RoundEndStats.Instance.mainEventHandlers;

            var scpKills = mainEventHandlers.killLog.Count(k =>
                k.AttackerName == killer.Nickname &&
                mainEventHandlers.IsRoleTypeSCP(k.VictimRole) &&
                k.VictimRole != RoleTypeId.Scp0492 &&
                k.VictimRole != RoleTypeId.Scp079);

            if (scpKills == 1)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("SCP-Killer", killer);
            }
            else if (scpKills > 1)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("SCP-Killer II", killer);
            }
        }
    }
}
