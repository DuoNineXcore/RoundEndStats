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

        public MainEventHandlers mainEventHandlers = new MainEventHandlers();
        public AchievementTracker achievementTracker = new AchievementTracker();
        public AchievementEvents achievementEvents = new AchievementEvents();

        [PluginEntryPoint("RoundEndStats", "1.0.0", "Yeah", "DuoNineXcore")]
        void OnEnabled()
        {
            API.Utils.LogMessage("Enabling RoundEndStats plugin...", API.Utils.LogLevel.Info);

            Instance = this;

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
            EventManager.UnregisterEvents(mainEventHandlers);
            API.Utils.LogMessage("RoundEndStats Disabled.", API.Utils.LogLevel.Info);
        }

        [PluginConfig]
        public Config Config;
    }
}
