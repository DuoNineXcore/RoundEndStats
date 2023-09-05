using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using RoundEndStats.API.Achievements;
using RoundEndStats.API.Achievements.AchievementEvents;
using RoundEndStats.API.EventHandlers;

namespace RoundEndStats
{
    public class RoundEndStats
    {
        public static RoundEndStats Instance;

        public MainEventHandlers mainEventHandlers;
        public AchievementTracker achievementTracker;
        public AchievementEvents achievementEvents;

        [PluginEntryPoint("RoundEndStats", "1.0.0", "Yeah", "DuoNineXcore")]
        void OnEnabled()
        {
            API.Utils.LogMessage("Enabling RoundEndStats plugin...", API.Utils.LogLevel.Info);

            Instance = this;

            achievementTracker = new AchievementTracker();
            achievementEvents = new AchievementEvents(achievementTracker, mainEventHandlers);
            mainEventHandlers = new MainEventHandlers(achievementTracker, achievementEvents);

            EventManager.RegisterEvents(mainEventHandlers);
            EventManager.RegisterEvents(achievementEvents);

            API.Utils.LogMessage("RoundEndStats Enabled.", API.Utils.LogLevel.Info);
        }

        [PluginUnload]
        void OnDisabled()
        {
            API.Utils.LogMessage("Disabling RoundEndStats plugin...", API.Utils.LogLevel.Info);

            Instance = null;

            mainEventHandlers = null;
            achievementTracker = null;
            achievementEvents = null;

            EventManager.UnregisterEvents(mainEventHandlers);
            EventManager.UnregisterEvents(achievementEvents);

            API.Utils.LogMessage("RoundEndStats Disabled.", API.Utils.LogLevel.Info);
        }

        [PluginConfig]
        public Config Config;
    }
}
