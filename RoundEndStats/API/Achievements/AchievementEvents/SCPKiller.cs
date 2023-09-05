using PlayerRoles;
using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using RoundEndStats.API.EventHandlers;
using System.Linq;
using PlayerStatsSystem;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void TrackSCPDeath(Player scp, Player atk, DamageHandlerBase dmg)
        {
            var scpKills = mainEventHandlers.killLog.Count(k =>
                k.AttackerName == atk.Nickname &&
                mainEventHandlers.IsRoleTypeSCP(k.VictimRole) &&
                k.VictimRole != RoleTypeId.Scp0492 &&
                k.VictimRole != RoleTypeId.Scp079);

            Utils.LogMessage($"{atk.Nickname} has killed {scpKills} SCPs.", Utils.LogLevel.Debug);

            if (scpKills == 1)
            {
                achievementTracker.AwardAchievement("SCP-Killer", atk);
                Utils.LogMessage($"{atk.Nickname} was awarded the 'SCP-Killer' achievement.", Utils.LogLevel.Debug);
            }
            else if (scpKills > 1)
            {
                achievementTracker.AwardAchievement("SCP-Killer II", atk);
                Utils.LogMessage($"{atk.Nickname} was awarded the 'SCP-Killer II' achievement.", Utils.LogLevel.Debug);
            }
        }
    }
}
