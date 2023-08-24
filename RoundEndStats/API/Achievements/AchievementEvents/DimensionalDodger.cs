﻿using Exiled.Events.EventArgs.Player;
using System;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundEndStats.API.Achievements.AchievementEvents
{
    public partial class AchievementEvents
    {
        public Dictionary<Player, int> pocketDimensionEscapes = new Dictionary<Player, int>();

        public void OnPlayerEscapedPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (ev.Player == null)
            {
                Utils.LogMessage($"Player is null in OnPlayerEscapedPocketDimension.", Utils.LogLevel.Error);
                return;
            }

            if (!pocketDimensionEscapes.ContainsKey(ev.Player))
            {
                pocketDimensionEscapes[ev.Player] = 0;
            }

            pocketDimensionEscapes[ev.Player]++;
            Utils.LogMessage($"{ev.Player.Nickname} escaped from the pocket dimension. Total escapes: {pocketDimensionEscapes[ev.Player]}", Utils.LogLevel.Debug);

            if (pocketDimensionEscapes[ev.Player] == 2)
            {
                RoundEndStats.Instance.achievementTracker.AwardAchievement("Dimensional Dodger", ev.Player);
                Utils.LogMessage($"{ev.Player.Nickname} was awarded the 'Dimensional Dodger' achievement.", Utils.LogLevel.Info);
            }
        }
    }
}
