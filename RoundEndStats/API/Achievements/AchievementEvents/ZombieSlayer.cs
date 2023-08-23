using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using Exiled.API.Features;
using PlayerRoles;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, int> zombieKillCounts = new Dictionary<Player, int>();

        public void OnPlayerKilledZombie(DiedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Scp0492 && ev.Attacker != null)
            {
                if (!zombieKillCounts.ContainsKey(ev.Attacker))
                {
                    zombieKillCounts[ev.Attacker] = 0;
                }

                zombieKillCounts[ev.Attacker]++;

                if (zombieKillCounts[ev.Attacker] == 5)
                {
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("Zombie Slayer", ev.Attacker);
                }
            }
        }
    }
}
