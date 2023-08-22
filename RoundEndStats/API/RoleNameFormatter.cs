using PlayerRoles;
using System.Collections.Generic;

namespace RES.API
{
    public static class RoleNameFormatter
    {
        public static readonly Dictionary<RoleTypeId, string> RoleNameMap = new Dictionary<RoleTypeId, string>
        {
            { RoleTypeId.Scp173, "SCP-173" },
            { RoleTypeId.Scp049, "SCP-049" },
            { RoleTypeId.Scp0492, "SCP-049-2" },
            { RoleTypeId.Scp079, "SCP-079" },
            { RoleTypeId.Scp096, "SCP-096" },
            { RoleTypeId.Scp106, "SCP-106" },
            { RoleTypeId.Scp939, "SCP-939" },
            { RoleTypeId.ClassD, "Class-D Personnel" },
            { RoleTypeId.Scientist, "Scientist" },
            { RoleTypeId.FacilityGuard, "Facility Guard" },
            { RoleTypeId.NtfPrivate, "Nine-Tailed Fox Private" },
            { RoleTypeId.NtfSergeant, "Nine-Tailed Fox Sergeant" },
            { RoleTypeId.NtfSpecialist, "Nine-Tailed Fox Specialist" },
            { RoleTypeId.NtfCaptain, "Nine-Tailed Fox Captain" },
            { RoleTypeId.ChaosRifleman, "Chaos Insurgency Rifleman" },
            { RoleTypeId.ChaosRepressor, "Chaos Insurgency Repressor" },
            { RoleTypeId.ChaosMarauder, "Chaos Insurgency Marauder" },
            { RoleTypeId.ChaosConscript, "Chaos Insurgency Conscript" },
            { RoleTypeId.Tutorial, "Tutorial" },
            { RoleTypeId.Overwatch, "Overwatch" },
            { RoleTypeId.Spectator, "Spectator" }
        };

        public static string GetFriendlyRoleName(RoleTypeId roleId)
        {
            if (RoleNameMap.TryGetValue(roleId, out string friendlyName))
            {
                return friendlyName;
            }
            else
            {
                // If the role isn't in our map, just return the raw identifier as a fallback.
                return roleId.ToString();
            }
        }
    }

}