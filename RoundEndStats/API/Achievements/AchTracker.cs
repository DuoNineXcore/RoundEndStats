using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using RoundEndStats.API.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundEndStats.API.Achievements
{
    public class AchievementTracker
    {
        public Dictionary<string, bool> achievementsUnlocked;
        private Dictionary<Player, List<Achievement>> unlockedAchievements = new Dictionary<Player, List<Achievement>>();

        public AchievementTracker()
        {
            achievementsUnlocked = new Dictionary<string, bool>();
        }

        public void AwardAchievement(string achievementName, Player ply)
        {
            if (!achievementsUnlocked[achievementName])
            {
                Utils.LogMessage($"{ply} has received the {achievementName} achievement.", ConsoleColor.DarkCyan);
                achievementsUnlocked[achievementName] = true;
            }
        }

        public Achievement GetGlobalTopAchievement()
        {
            Achievement topAchievement = null;

            foreach (var playerAchievements in unlockedAchievements.Values)
            {
                foreach (var achievement in playerAchievements)
                {
                    if (topAchievement == null || achievement.Importance < topAchievement.Importance)
                    {
                        topAchievement = achievement;
                    }
                }
            }

            return topAchievement;
        }

        public Achievement GetPlayerTopAchievement(Player player)
        {
            if (!unlockedAchievements.TryGetValue(player, out List<Achievement> playerAchievements))
            {
                return null;
            }

            Achievement playerTopAchievement = null;

            foreach (var achievement in playerAchievements)
            {
                if (playerTopAchievement == null || achievement.Importance < playerTopAchievement.Importance)
                {
                    playerTopAchievement = achievement;
                }
            }

            return playerTopAchievement;
        }


        public List<Achievement> GetSortedUnlockedAchievements()
        {
            List<Achievement> unlockedAchievements = new List<Achievement>();

            foreach (var achievement in AchievementRegistry.AllAchievements)
            {
                if (achievementsUnlocked[achievement.Name])
                {
                    unlockedAchievements.Add(achievement);
                }
            }
            return unlockedAchievements.OrderBy(a => a.Importance).ToList();
        }
    }
}
