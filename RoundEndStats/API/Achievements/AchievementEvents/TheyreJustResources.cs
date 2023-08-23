using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using Exiled.API.Features;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        private Dictionary<Player, int> scientistKillCount = new Dictionary<Player, int>();

        public void OnPlayerKilled(DiedEventArgs ev)
        {
            if (ev.Attacker.Role == RoleTypeId.Scientist && ev.Player.Role == RoleTypeId.ClassD)
            {
                if (!scientistKillCount.ContainsKey(ev.Attacker))
                {
                    scientistKillCount[ev.Attacker] = 0;
                }

                scientistKillCount[ev.Attacker]++;

                if (scientistKillCount[ev.Attacker] >= 5)
                {
                    RoundEndStats.Instance.achievementTracker.AwardAchievement("They're just resources.", ev.Attacker);
                    scientistKillCount.Remove(ev.Attacker);
                }
            }
        }
    }
}
