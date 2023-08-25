using Exiled.API.Features;
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
            Utils.LogMessage("Achievement Tracker initialized.", Utils.LogLevel.Debug);
        }

        public void AwardAchievement(string achievementName, Player ply)
        {
            if (ply == null)
            {
                Utils.LogMessage("Attempted to award achievement to null player.", Utils.LogLevel.Error);
                return;
            }

            if (!achievementsUnlocked[achievementName])
            {
                Utils.LogMessage($"{ply.Nickname} has received the {achievementName} achievement.", Utils.LogLevel.Info);
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

            if (topAchievement != null)
            {
                Utils.LogMessage($"Global top achievement is {topAchievement.Name}.", Utils.LogLevel.Debug);
            }
            else
            {
                Utils.LogMessage("No global top achievement found.", Utils.LogLevel.Warning);
            }

            return topAchievement;
        }

        public Achievement GetPlayerTopAchievement(Player player)
        {
            if (player == null)
            {
                Utils.LogMessage("Attempted to get top achievement for null player.", Utils.LogLevel.Error);
                return null;
            }

            if (!unlockedAchievements.TryGetValue(player, out List<Achievement> playerAchievements))
            {
                Utils.LogMessage($"{player.Nickname} has no unlocked achievements.", Utils.LogLevel.Warning);
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

            if (playerTopAchievement != null)
            {
                Utils.LogMessage($"{player.Nickname}'s top achievement is {playerTopAchievement.Name}.", Utils.LogLevel.Debug);
            }
            else
            {
                Utils.LogMessage($"{player.Nickname} has no top achievement.", Utils.LogLevel.Warning);
            }

            return playerTopAchievement;
        }

        public List<Achievement> GetSortedUnlockedAchievements()
        {
            List<Achievement> unlockedAchievementsList = new List<Achievement>();

            foreach (var achievement in AchievementRegistry.AllAchievements)
            {
                if (achievementsUnlocked[achievement.Name])
                {
                    unlockedAchievementsList.Add(achievement);
                }
            }

            Utils.LogMessage($"Sorting {unlockedAchievementsList.Count} unlocked achievements.", Utils.LogLevel.Debug);
            return unlockedAchievementsList.OrderBy(a => a.Importance).ToList();
        }
    }
}
