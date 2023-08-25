using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using Exiled.API.Features;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, Player> handcuffedPlayers = new Dictionary<Player, Player>();

        private static readonly HashSet<RoleTypeId> FacilityRoles = new HashSet<RoleTypeId>
        {
            RoleTypeId.FacilityGuard,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfCaptain,
            RoleTypeId.Scientist
        };

        private static readonly HashSet<RoleTypeId> ChaosRoles = new HashSet<RoleTypeId>
        {
            RoleTypeId.ClassD,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosMarauder
        };

        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.Target == null || ev.Player == null)
            {
                Utils.LogMessage("Either the target or the player is null in OnHandcuffing.", Utils.LogLevel.Error);
                return;
            }

            if (FacilityRoles.Contains(ev.Target.Role) && ChaosRoles.Contains(ev.Player.Role))
            {
                handcuffedPlayers[ev.Target] = ev.Player;
                Utils.LogMessage($"{ev.Player.Nickname} has handcuffed {ev.Target.Nickname}.", Utils.LogLevel.Debug);
            }
        }

        public void OnRoleChange(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null)
            {
                Utils.LogMessage("Player is null in OnRoleChange.", Utils.LogLevel.Error);
                return;
            }

            if (ev.NewRole == RoleTypeId.ChaosConscript && handcuffedPlayers.TryGetValue(ev.Player, out Player cuffer) && ev.Player.IsCuffed)
            {
                achievementTracker.AwardAchievement("Turncoat", cuffer);
                Utils.LogMessage($"{cuffer.Nickname} has been awarded the 'Turncoat' achievement for converting {ev.Player.Nickname}.", Utils.LogLevel.Info);
                handcuffedPlayers.Remove(ev.Player);
            }
        }
    }
}
