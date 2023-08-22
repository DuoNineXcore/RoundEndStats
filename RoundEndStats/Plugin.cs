using Exiled.API.Features;

namespace RES
{
    public class RoundEndStats : Plugin<Config>
    {
        public override string Author => "DuoNineXcore";
        public override string Name => "RoundEndStats";
        public override string Prefix => Name;
        public static RoundEndStats Instance;
        private EventHandlers eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            eventHandlers = new EventHandlers();
            base.OnEnabled();

            Exiled.Events.Handlers.Player.Died += eventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting += eventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Server.RoundEnded += eventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers += eventHandlers.OnWaiting;
        }

        public override void OnDisabled()
        {
            Instance = null;
            eventHandlers = null;
            base.OnDisabled();

            Exiled.Events.Handlers.Player.Died -= eventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting -= eventHandlers.OnPlayerHurt;
            Exiled.Events.Handlers.Server.RoundEnded -= eventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= eventHandlers.OnWaiting;
        }
    }
}