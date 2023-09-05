using PlayerRoles;
using System;
using PlayerStatsSystem;

namespace RoundEndStats.API.Events
{
    public class KillEvent
    {
        public RoleTypeId AttackerRole { get; set; }
        public string AttackerName { get; set; }
        public RoleTypeId VictimRole { get; set; }
        public string VictimName { get; set; }
        public DamageHandlerBase DamageInfo { get; set; }
        public DateTime TimeOfKill { get; set; }

        public KillEvent(RoleTypeId attackerRole, string attackerName, RoleTypeId victimRole, string victimName, DamageHandlerBase damageInfo)
        {
            AttackerRole = attackerRole;
            AttackerName = attackerName;
            VictimRole = victimRole;
            VictimName = victimName;
            DamageInfo = damageInfo;
            TimeOfKill = DateTime.Now;

            Utils.LogMessage($"{attackerRole} - {attackerName} killed {victimRole} - {victimName} // Cause of death: {damageInfo.GetType()} // Time of kill: {TimeOfKill}", Utils.LogLevel.Debug);
        }
    }
}
