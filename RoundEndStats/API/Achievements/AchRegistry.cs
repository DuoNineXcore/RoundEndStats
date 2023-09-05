using System.Collections.Generic;

namespace RoundEndStats.API.Achievements
{
    public static class AchievementRegistry
    {
        public static List<Achievement> AllAchievements = new List<Achievement>
        {
            new Achievement { Name = "SCP-Killer II", Description = "Killed more than one SCP.", Importance = 1 },
            new Achievement { Name = "SCP-Killer", Description = "Killed an SCP.", Importance = 2 },
            //new Achievement { Name = "One Against Many", Description = "Killed a significant portion of a respawn wave.", Importance = 3 },
            new Achievement { Name = "Dimensional Dodger", Description = "Escaped SCP-106's pocket dimension twice.", Importance = 4 },
            new Achievement { Name = "Staring Contest Champion", Description = "Survived SCP-096's rage.", Importance = 5 },
            new Achievement { Name = "Zombie Slayer", Description = "Killed 5 SCP-049-2 instances.", Importance = 6 },
            new Achievement { Name = "Turncoat", Description = "Handcuffed a guard/MTF/scientist and turned them into Chaos Insurgency.", Importance = 7 },
            new Achievement { Name = "Caffeine Junkie", Description = "Drank more than 3 colas and survived for more than 2 minutes.", Importance = 8 },
            new Achievement { Name = "Peacemaker", Description = "Survived the round without killing anyone.", Importance = 9 },
            //new Achievement { Name = "Suicide Bomber", Description = "Pink Candied 5 Players.", Importance = 10 },
            new Achievement { Name = "They're just resources.", Description = "As a Scientist, kill 5 Class-D Personnel.", Importance = 11 },
        };
    }
}
