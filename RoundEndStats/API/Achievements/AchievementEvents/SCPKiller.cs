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

            Utils.LogMessage($"{killer.Nickname} has killed {scpKills} SCPs.", Utils.LogLevel.Debug);

            if (scpKills == 1)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("SCP-Killer", killer);
                Utils.LogMessage($"{killer.Nickname} was awarded the 'SCP-Killer' achievement.", Utils.LogLevel.Debug);
            }
            else if (scpKills > 1)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("SCP-Killer II", killer);
                Utils.LogMessage($"{killer.Nickname} was awarded the 'SCP-Killer II' achievement.", Utils.LogLevel.Debug);
            }
        }
    }
}
