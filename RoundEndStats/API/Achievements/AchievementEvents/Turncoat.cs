using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using Exiled.API.Features;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, Player> handcuffedPlayers = new Dictionary<Player, Player>();

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if ((ev.Target.Role == RoleTypeId.FacilityGuard || ev.Target.Role == RoleTypeId.NtfPrivate || ev.Target.Role == RoleTypeId.NtfSergeant || ev.Target.Role == RoleTypeId.NtfCaptain || ev.Target.Role == RoleTypeId.Scientist) && (ev.Player.Role == RoleTypeId.ClassD || ev.Player.Role == RoleTypeId.ChaosRifleman || ev.Player.Role == RoleTypeId.ChaosRepressor || ev.Player.Role == RoleTypeId.ChaosMarauder))
            {
                handcuffedPlayers[ev.Target] = ev.Player;
            }
        }

        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleTypeId.ChaosConscript)
            {
                if (handcuffedPlayers.TryGetValue(ev.Player, out Player cuffer) && ev.Player.IsCuffed)
                {
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("Turncoat", cuffer);
                    handcuffedPlayers.Remove(ev.Player);
                }
            }
        }
    }
}
