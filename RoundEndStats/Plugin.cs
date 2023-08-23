using Exiled.API.Features;
using RoundEndStats.API.EventHandlers;

namespace RoundEndStats
{
    public class RoundEndStats : Plugin<Config>
    {
        public override string Author => "DuoNineXcore";
        public override string Name => "RoundEndStats";
        public override string Prefix => "RES";
        public static RoundEndStats Instance;
        private MainEventHandlers mainEventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            mainEventHandlers = new MainEventHandlers();
            base.OnEnabled();

            //kill events
            Exiled.Events.Handlers.Player.Died += mainEventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting += mainEventHandlers.OnPlayerHurt;

            //escape events
            Exiled.Events.Handlers.Server.RoundStarted += mainEventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Escaping += mainEventHandlers.OnPlayerEscape;

            //item usage events 
            Exiled.Events.Handlers.Player.UsedItem += mainEventHandlers.OnUsedItem;

            //main events
            Exiled.Events.Handlers.Server.RoundEnded += mainEventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers += mainEventHandlers.OnWaiting;
        }

        public override void OnDisabled()
        {
            Instance = null;
            mainEventHandlers = null;
            base.OnDisabled();

            //kill events
            Exiled.Events.Handlers.Player.Died -= mainEventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.Hurting -= mainEventHandlers.OnPlayerHurt;

            //escape events
            Exiled.Events.Handlers.Server.RoundStarted -= mainEventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Escaping -= mainEventHandlers.OnPlayerEscape;

            //item usage events
            Exiled.Events.Handlers.Player.UsedItem -= mainEventHandlers.OnUsedItem;

            //main events
            Exiled.Events.Handlers.Server.RoundEnded -= mainEventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= mainEventHandlers.OnWaiting;
        }
    }
}