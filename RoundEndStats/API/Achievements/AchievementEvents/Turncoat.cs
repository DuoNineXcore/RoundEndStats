using PlayerRoles;
using System.Collections.Generic;
using PluginAPI;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

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

        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public void OnHandcuffing(Player ply, Player trg)
        {
            if (trg == null || ply == null)
            {
                Utils.LogMessage("Either the target or the player is null in OnHandcuffing.", Utils.LogLevel.Error);
                return;
            }

            if (FacilityRoles.Contains(trg.Role) && ChaosRoles.Contains(ply.Role))
            {
                handcuffedPlayers[trg] = ply;
                Utils.LogMessage($"{ply.Nickname} has handcuffed {trg.Nickname}.", Utils.LogLevel.Debug);
            }
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        void OnPlayerEscaped(Player ply, RoleTypeId role)
        {
            if (ply == null)
            {
                Utils.LogMessage("Player is null in OnRoleChange.", Utils.LogLevel.Error);
                return;
            }

            if (ply.Role == RoleTypeId.ChaosConscript && handcuffedPlayers.TryGetValue(ply, out Player cuffer))
            {
                achievementTracker.AwardAchievement("Turncoat", cuffer);
                Utils.LogMessage($"{cuffer.Nickname} has been awarded the 'Turncoat' achievement for converting {ply.Nickname}.", Utils.LogLevel.Info);
                handcuffedPlayers.Remove(ply);
            }
        }
    }
}
