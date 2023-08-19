using Exiled.API.Features;
using Exiled.Events.Handlers;

namespace RES
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "DuoNineXcore";
        public override string Name => "RoundEndStats";
        public override string Prefix => Name;
        public static Plugin Instance;
        private EventHandlers eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();
            Exiled.Events.Handlers.Player.Died += eventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.KillingPlayer += eventHandlers.OnPlayerKilling;
            Exiled.Events.Handlers.Server.RoundEnded += eventHandlers.OnRoundEnd;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Exiled.Events.Handlers.Player.Died -= eventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.KillingPlayer -= eventHandlers.OnPlayerKilling;
            Exiled.Events.Handlers.Server.RoundEnded -= eventHandlers.OnRoundEnd;
            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            eventHandlers = new EventHandlers();
        }

        private void UnregisterEvents()
        {
            eventHandlers = null;
        }
    }
}