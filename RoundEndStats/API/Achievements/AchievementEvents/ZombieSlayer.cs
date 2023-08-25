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
            if (ev.Player == null || ev.Attacker == null)
            {
                Utils.LogMessage("Either the victim or the attacker is null in OnPlayerKilledZombie.", Utils.LogLevel.Error);
                return;
            }

            if (ev.Player.Role == RoleTypeId.Scp0492)
            {
                if (!zombieKillCounts.ContainsKey(ev.Attacker))
                {
                    zombieKillCounts[ev.Attacker] = 0;
                }

                zombieKillCounts[ev.Attacker]++;

                Utils.LogMessage($"{ev.Attacker.Nickname} has killed {zombieKillCounts[ev.Attacker]} zombies.", Utils.LogLevel.Debug);

                if (zombieKillCounts[ev.Attacker] == 5)
                {
                    achievementTracker.AwardAchievement("Zombie Slayer", ev.Attacker);
                    Utils.LogMessage($"{ev.Attacker.Nickname} has been awarded the 'Zombie Slayer' achievement.", Utils.LogLevel.Info);
                }
            }
        }
    }
}
