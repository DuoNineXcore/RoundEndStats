using PluginAPI.Core;
using PluginAPI;
using PluginAPI.Core.Attributes;
using System.Linq;
using System.Collections.Generic;
using PlayerRoles;
using PluginAPI.Enums;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, RoleTypeId> initialRoles = new Dictionary<Player, RoleTypeId>();

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            foreach (var player in Player.GetPlayers())
            {
                initialRoles[player] = player.Role;
            }
        }

        public void TrackSurvivalWithoutKilling(Player survivor)
        {
            var mainEventHandlers = RoundEndStats.Instance.mainEventHandlers;
            var kills = mainEventHandlers.killLog.Count(k => k.AttackerName == survivor.Nickname);

            Utils.LogMessage($"{survivor.Nickname} has {kills} kills.", Utils.LogLevel.Debug);

            if (kills == 0 && initialRoles.TryGetValue(survivor, out var initialRole) && initialRole == survivor.Role)
            {
                achievementTracker.AwardAchievement("Peacemaker", survivor);
                Utils.LogMessage($"{survivor.Nickname} was awarded the 'Peacemaker' achievement.", Utils.LogLevel.Debug);
            }
        }
    }
}
