using System;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI;
using PlayerRoles;
using RoundEndStats.API.Achievements;
using RoundEndStats.API.Achievements.AchievementEvents;

namespace RoundEndStats.API.EventHandlers
{
    public partial class MainEventHandlers
    {
        private AchievementTracker achievementTracker = new AchievementTracker();
        private AchievementEvents achievementEvents = new AchievementEvents();

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaiting()
        {
            achievementEvents.pocketDimensionEscapes.Clear();
            achievementEvents.playersWhoTriggered096.Clear();
            killLog.Clear();

            Utils.LogMessage("Cleared event logs on waiting.", Utils.LogLevel.Debug);
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        public void OnRoundEnd(Player ply)
        {
            Utils.LogMessage("Round ended. Broadcasting player stats.", Utils.LogLevel.Info);
            foreach (var player in Player.GetPlayers())
            {
                Utils.LogMessage($"Player Stats are going to be broadcasted to player: {player}", Utils.LogLevel.Info);
                BroadcastPlayerStats(player);
                Utils.LogMessage($"Player Stats Broadcasted to player: {player}", Utils.LogLevel.Info);
            }
        }

        private void BroadcastPlayerStats(Player player)
        {
            try
            {
                Utils.LogMessage($"Constructing player broadcast for player {player?.Nickname ?? "NULL"}", Utils.LogLevel.Info);

                if (RoundEndStats.Instance == null)
                {
                    Utils.LogMessage($"RoundEndStats.Instance is null", Utils.LogLevel.Error);
                    return;
                }
                var playerStats = GetPlayerStats(player);
                var topKillerStats = GetTopKillerStats();
                var topScpKillerStats = GetTopScpKillerStats();
                var topHumanKillerStats = GetTopHumanKillerStats();
                var topScpItemUserStats = GetTopScpItemUserStats();
                Player firstEscapee = GetFirstPlayerToEscape();
                int totalEscapes = GetTotalEscapes();
                TimeSpan? playerEscapeTime = GetPlayerEscapeTime(player);
                string formattedEscapeTime = playerEscapeTime.HasValue ? playerEscapeTime.Value.ToString(@"hh\:mm\:ss") : "N/A";
                var playerTopAchievement = achievementTracker.GetPlayerTopAchievement(player);
                var globalTopAchievement = achievementTracker.GetGlobalTopAchievement();
                Utils.LogMessage($"Constructed player broadcast", Utils.LogLevel.Info);

                Utils.LogMessage($"Constructing stats message", Utils.LogLevel.Info);
                string statsMessage = RoundEndStats.Instance.Config.StatsFormat
                    .Replace("{playerName}", player.Nickname)
                    .Replace("{playerKills}", playerStats.Kills.ToString())
                    .Replace("{playerDeaths}", playerStats.Deaths.ToString())
                    .Replace("{playerEscapeTime}", formattedEscapeTime)

                    .Replace("{globalTopAchievement}", globalTopAchievement.ToString())
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
                Utils.LogMessage($"Stats message constructed, broadcasting.", Utils.LogLevel.Info);

                if (!RoundEndStats.Instance.Config.IsHint)
                {
                    player.SendBroadcast($"<size={RoundEndStats.Instance.Config.BroadcastSize}>{statsMessage}</size>", RoundEndStats.Instance.Config.BroadcastDuration, Broadcast.BroadcastFlags.Normal);
                    Utils.LogMessage($"Broadcasted stats to player {player.Nickname}.", Utils.LogLevel.Debug);
                }
                else
                {
                    player.ReceiveHint($"<size={RoundEndStats.Instance.Config.BroadcastSize}>{statsMessage}</size>", RoundEndStats.Instance.Config.BroadcastDuration);
                    Utils.LogMessage($"Displayed hint with stats to player {player.Nickname}.", Utils.LogLevel.Debug);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage($"An error occurred while broadcasting player stats: {ex}", Utils.LogLevel.Error);
            }
        }

        private (int Kills, int Deaths, int MatchTime) GetPlayerStats(Player player)
        {
            if (player == null)
            {
                Utils.LogMessage("Attempted to get player stats for a null player.", Utils.LogLevel.Error);
                return (0, 0, 0);
            }

            int playerKills = killLog.Count(k => k.AttackerName == player.Nickname);
            int playerDeaths = killLog.Count(k => k.VictimName == player.Nickname);
            int matchTime = RoundSummary.roundTime;

            Utils.LogMessage($"Retrieved stats for player {player.Nickname}: Kills={playerKills}, Deaths={playerDeaths}, MatchTime={matchTime}", Utils.LogLevel.Debug);

            return (playerKills, playerDeaths, matchTime);
        }

        private (string ColoredName, string RoleName, int Kills) GetTopKillerStats()
        {
            var topKillerEvent = killLog.GroupBy(k => k.AttackerName)
                                        .OrderByDescending(g => g.Count())
                                        .FirstOrDefault();

            if (topKillerEvent == null)
            {
                Utils.LogMessage("No top killer found.", Utils.LogLevel.Warning);
                return ("None", "Unknown Role", 0);
            }

            string topKillerName = topKillerEvent?.Key ?? "None";
            int topKillerKills = topKillerEvent?.Count() ?? 0;
            RoleTypeId topKillerRole = topKillerEvent?.First().AttackerRole ?? RoleTypeId.None;
            string topKillerColor = Player.Get(topKillerName)?.ReferenceHub.roleManager.CurrentRole.RoleColor.ToHex();
            string coloredName = $"<color={topKillerColor}>{topKillerName}</color>";
            return (coloredName, topKillerRole.ToString(), topKillerKills);
        }


        private (string ColoredName, string RoleName, int Kills) GetTopScpKillerStats()
        {
            var topScpKillerEvent = killLog.Where(k => IsRoleTypeSCP(k.AttackerRole) && !IsRoleTypeSCP(k.VictimRole))
                                           .GroupBy(k => k.AttackerName)
                                           .OrderByDescending(g => g.Count())
                                           .FirstOrDefault();

            if (topScpKillerEvent == null)
            {
                Utils.LogMessage("No top SCP killer found.", Utils.LogLevel.Warning);
                return ("None", "Unknown Role", 0);
            }

            string topScpKillerName = topScpKillerEvent?.Key ?? "None";
            int topScpKills = topScpKillerEvent?.Count() ?? 0;
            RoleTypeId topScpKillerRole = topScpKillerEvent?.First().AttackerRole ?? RoleTypeId.None;
            string topScpKillerColor = Player.Get(topScpKillerName)?.ReferenceHub.roleManager.CurrentRole.RoleColor.ToHex();
            string coloredName = $"<color={topScpKillerColor}>{topScpKillerName}</color>";

            return (coloredName, topScpKillerRole.ToString(), topScpKills);
        }

        private (string ColoredName, string RoleName, int Kills) GetTopHumanKillerStats()
        {
            Player topHumanPlayer = GetTopHumanPlayer();

            if (topHumanPlayer == null)
            {
                Utils.LogMessage("No top human player found.", Utils.LogLevel.Warning);
                return ("None", "Unknown Role", 0);
            }

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
                string topHumanColor = topHumanPlayer?.ReferenceHub.roleManager.CurrentRole.RoleColor.ToHex();
                topHumanNameColored = $"<color={topHumanColor}>{topHumanPlayer.Nickname}</color>";
            }

            return (topHumanNameColored, topHumanRoleName, topHumanKills);
        }

        private (string ColoredName, string RoleName, int ScpUses, string ItemList) GetTopScpItemUserStats()
        {
            var topScpItemUserGroup = scpItemUsageLog.GroupBy(u => u.User)
                                                     .OrderByDescending(g => g.Count())
                                                     .FirstOrDefault();

            if (topScpItemUserGroup == null)
            {
                Utils.LogMessage("No top SCP item user found.", Utils.LogLevel.Warning);
                return ("None", "Unknown Role", 0, "None");
            }

            Player topScpItemUser = topScpItemUserGroup?.Key;
            int scpUses = topScpItemUserGroup?.Count() ?? 0;

            if (topScpItemUser == null)
            {
                return ("None", "Unknown Role", 0, "None");
            }

            string roleName = topScpItemUser.Role.ToString();
            string color = topScpItemUser?.ReferenceHub.roleManager.CurrentRole.RoleColor.ToHex();
            string coloredName = $"<color={color}>{topScpItemUser.Nickname}</color>";

            List<string> itemsUsed = topScpItemUserGroup.Select(u => NameFormatter.GetFriendlySCPItemName(u.ScpItem)).ToList();
            string itemList = string.Join(", ", itemsUsed);

            return (coloredName, roleName, scpUses, itemList);
        }
    }
}
