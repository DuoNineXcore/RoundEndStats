using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Enums;
using PlayerRoles;
using UnityEngine;
using RoundEndStats.API;
using LiteNetLib4Mirror.Open.Nat;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using static Achievements.AchievementManager;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        public void OnWaiting()
        {
            var achievementEvents = RoundEndStats.Instance.achievementEvents;
            achievementEvents.pocketDimensionEscapes.Clear();
            achievementEvents.playersWhoTriggered096.Clear();
            killLog.Clear();
        }

        public void OnRoundEnd(RoundEndedEventArgs ev)
        {
            foreach (var player in Player.List)
            {
                BroadcastPlayerStats(player);
            }
        }

        private void BroadcastPlayerStats(Player player)
        {
            var playerStats = GetPlayerStats(player);
            var topKillerStats = GetTopKillerStats();
            var topScpKillerStats = GetTopScpKillerStats();
            var topHumanKillerStats = GetTopHumanKillerStats();
            var topScpItemUserStats = GetTopScpItemUserStats();
            Player firstEscapee = GetFirstPlayerToEscape();
            int totalEscapes = GetTotalEscapes();
            TimeSpan? playerEscapeTime = GetPlayerEscapeTime(player);
            string formattedEscapeTime = playerEscapeTime.HasValue ? playerEscapeTime.Value.ToString(@"hh\:mm\:ss") : "N/A";
            var playerTopAchievement = RoundEndStats.Instance.achievementTracker.GetTopUnlockedAchievement(player);

            string statsMessage = RoundEndStats.Instance.Config.StatsFormat
                .Replace("{playerName}", player.Nickname)
                .Replace("{playerKills}", playerStats.Kills.ToString())
                .Replace("{playerDeaths}", playerStats.Deaths.ToString())
                .Replace("{playerEscapeTime}", formattedEscapeTime)

                .Replace("{playerTopAchievement}", playerTopAchievement?.Name ?? "None")
                .Replace("{matchTime}", playerStats.MatchTime.ToString())

                .Replace("{firstEscapee}", firstEscapee?.Nickname ?? "None")
                .Replace("{totalEscapes}", totalEscapes.ToString())

                .Replace("{topKiller}", topKillerStats.ColoredName)
                .Replace("{topKillerRole}", topKillerStats.RoleName)
                .Replace("{topKillerKills}", topKillerStats.Kills.ToString())

                .Replace("{topSCPName}", topScpKillerStats.ColoredName)
                .Replace("{topSCPRole}", topScpKillerStats.RoleName)
                .Replace("{topSCPKills}", topScpKillerStats.Kills.ToString())

                .Replace("{topSCPItemUserName}", topScpItemUserStats.ColoredName)
                .Replace("{topSCPItemUserRole}", topScpItemUserStats.RoleName)
                .Replace("{topSCPItemUserCount}", topScpItemUserStats.ScpUses.ToString())
                .Replace("{topSCPItemUserList}", topScpItemUserStats.ItemList)

                .Replace("{topHumanName}", topHumanKillerStats.ColoredName)
                .Replace("{topHumanRole}", topHumanKillerStats.RoleName)
                .Replace("{topHumanKills}", topHumanKillerStats.Kills.ToString());

            player.Broadcast(RoundEndStats.Instance.Config.BroadcastDuration, $"<size={RoundEndStats.Instance.Config.BroadcastSize}>{statsMessage}</size>", Broadcast.BroadcastFlags.Normal);
        }

        private (int Kills, int Deaths, int MatchTime) GetPlayerStats(Player player)
        {
            int playerKills = killLog.Count(k => k.AttackerName == player.Nickname);
            int playerDeaths = killLog.Count(k => k.VictimName == player.Nickname);
            int matchTime = RoundSummary.roundTime;

            return (playerKills, playerDeaths, matchTime);
        }

        private (string ColoredName, string RoleName, int Kills) GetTopKillerStats()
        {
            var topKillerEvent = killLog.GroupBy(k => k.AttackerName)
                                        .OrderByDescending(g => g.Count())
                                        .FirstOrDefault();

            string topKillerName = topKillerEvent?.Key ?? "None";
            int topKillerKills = topKillerEvent?.Count() ?? 0;
            RoleTypeId topKillerRole = topKillerEvent?.First().AttackerRole ?? RoleTypeId.None;
            string topKillerColor = topKillerRole.GetColor().ToHex();
            string coloredName = $"<color={topKillerColor}>{topKillerName}</color>";

            return (coloredName, topKillerRole.ToString(), topKillerKills);
        }

        private (string ColoredName, string RoleName, int Kills) GetTopScpKillerStats()
        {
            var topScpKillerEvent = killLog.Where(k => IsRoleTypeSCP(k.AttackerRole) && !IsRoleTypeSCP(k.VictimRole))
                                           .GroupBy(k => k.AttackerName)
                                           .OrderByDescending(g => g.Count())
                                           .FirstOrDefault();

            string topScpKillerName = topScpKillerEvent?.Key ?? "None";
            int topScpKills = topScpKillerEvent?.Count() ?? 0;
            RoleTypeId topScpKillerRole = topScpKillerEvent?.First().AttackerRole ?? RoleTypeId.None;
            string topScpKillerColor = topScpKillerRole.GetColor().ToHex();
            string coloredName = $"<color={topScpKillerColor}>{topScpKillerName}</color>";

            return (coloredName, topScpKillerRole.ToString(), topScpKills);
        }

        private (string ColoredName, string RoleName, int Kills) GetTopHumanKillerStats()
        {
            Player topHumanPlayer = GetTopHumanPlayer();
            string topHumanNameColored = "None";
            string topHumanRoleName = "Unknown Role";
            int topHumanKills = 0;

            if (topHumanPlayer != null)
            {
                var topHumanKillerEvent = killLog.Where(k => k.AttackerName == topHumanPlayer.Nickname && IsRoleTypeSCP(k.VictimRole))
                                                 .GroupBy(k => k.AttackerRole)
                                                 .OrderByDescending(g => g.Count())
                                                 .FirstOrDefault();

                topHumanKills = topHumanKillerEvent?.Count() ?? 0;
                RoleTypeId topHumanRole = topHumanKillerEvent?.Key ?? RoleTypeId.None;
                topHumanRoleName = NameFormatter.RoleNameMap.TryGetValue(topHumanRole, out var humanName) ? humanName : topHumanRole.ToString();
                string topHumanColor = topHumanRole.GetColor().ToHex();
                topHumanNameColored = $"<color={topHumanColor}>{topHumanPlayer.Nickname}</color>";
            }

            return (topHumanNameColored, topHumanRoleName, topHumanKills);
        }

        private (string ColoredName, string RoleName, int ScpUses, string ItemList) GetTopScpItemUserStats()
        {
            var topScpItemUserGroup = scpItemUsageLog.GroupBy(u => u.User)
                                                     .OrderByDescending(g => g.Count())
                                                     .FirstOrDefault();

            Player topScpItemUser = topScpItemUserGroup?.Key;
            int scpUses = topScpItemUserGroup?.Count() ?? 0;

            if (topScpItemUser == null)
            {
                return ("None", "Unknown Role", 0, "None");
            }

            string roleName = topScpItemUser.Role.Type.ToString();
            string color = topScpItemUser.Role.Type.GetColor().ToHex();
            string coloredName = $"<color={color}>{topScpItemUser.Nickname}</color>";

            List<string> itemsUsed = topScpItemUserGroup.Select(u => NameFormatter.GetFriendlySCPItemName(u.ScpItem)).ToList();
            string itemList = string.Join(", ", itemsUsed);

            return (coloredName, roleName, scpUses, itemList);
        }
    }
}
