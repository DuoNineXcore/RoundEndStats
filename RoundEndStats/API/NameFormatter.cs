using PlayerRoles;
using System.Collections.Generic;

namespace RoundEndStats.API
{
    public static class NameFormatter
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
                return roleId.ToString();
            }
        }

        public static readonly Dictionary<ItemType, string> SCPItemNameMap = new Dictionary<ItemType, string>
        {
            { ItemType.SCP018, "SCP-018" },
            { ItemType.SCP1576, "SCP-1576" },
            { ItemType.SCP1853, "SCP-1853" },
            { ItemType.SCP207, "SCP-207" },
            { ItemType.SCP2176, "SCP-2176" },
            { ItemType.SCP244a, "SCP-244-A" },
            { ItemType.SCP244b, "SCP-244-B" },
            { ItemType.AntiSCP207, "Anti SCP-207" },
            { ItemType.SCP500, "SCP-500" },
            { ItemType.SCP330, "SCP-330" },
            { ItemType.SCP268, "SCP-268" },
        };

        public static string GetFriendlySCPItemName(ItemType scpitem)
        {
            if (SCPItemNameMap.TryGetValue(scpitem, out string friendlyName))
            {
                return friendlyName;
            }
            else
            {
                return scpitem.ToString();
            }
        }
    }
}