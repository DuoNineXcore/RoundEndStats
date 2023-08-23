using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundEndStats.API.Achievements
{
    public static class AchievementRegistry
    {
        public static List<Achievement> AllAchievements = new List<Achievement>
        {
            new Achievement { Name = "SCP-Killer", Description = "Killed an SCP." },
            new Achievement { Name = "Suicide Bomber", Description = "Pink Candied 5 Players." },
            new Achievement { Name = "Peacemaker", Description = "Survived the round without killing anyone." },
            new Achievement { Name = "Class Dismissed", Description = "As a Scientist, killed a D-Class personnel." },
            new Achievement { Name = "Dimensional Dodger", Description = "Escaped SCP-106's pocket dimension twice in one round." },
            new Achievement { Name = "Staring Contest Champion", Description = "Survived SCP-096's enraged state without getting killed." },
            new Achievement { Name = "One Against Many", Description = "As an SCP, eliminated at least two-thirds of an MTF or Chaos Insurgency wave." },
            new Achievement { Name = "Turncoat", Description = "Converted Foundation Personnel to Chaos Insurgency as a Class-D or CI." },
            new Achievement { Name = "Caffeine Junkie", Description = "Drank more than 3 colas in a round and survived for at least 2 minutes afterward." },
            new Achievement { Name = "SCP-Killer II", Description = "Eliminated more than one SCP in a single round." },
            new Achievement { Name = "Zombie Slayer", Description = "Killed 5 instances of SCP-049-2 in one round." }
        };
    }
}
