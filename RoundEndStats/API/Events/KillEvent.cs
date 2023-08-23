using PlayerRoles;

namespace RoundEndStats.API.Events
{
    public class KillEvent
    {
        public RoleTypeId AttackerRole { get; set; }
        public string AttackerName { get; set; }
        public RoleTypeId VictimRole { get; set; }
        public string VictimName { get; set; }

        public KillEvent(RoleTypeId? attackerRole, string attackerName, RoleTypeId victimRole, string victimName)
        {
            AttackerRole = attackerRole ?? RoleTypeId.None;
            AttackerName = attackerName ?? "Unknown";
            VictimRole = victimRole;
            VictimName = victimName;

            Utils.LogMessage($"{AttackerRole} - {AttackerName} killed {VictimRole} - {VictimName}", System.ConsoleColor.DarkCyan);
        }
    }
}
