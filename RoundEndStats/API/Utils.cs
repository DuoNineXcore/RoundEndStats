using System.Collections.Generic;
using System.Linq;
using PluginAPI;
using PluginAPI.Core;
using PlayerRoles;
using UnityEngine;
using System;
using System.Diagnostics;

namespace RoundEndStats.API
{
    public static class Utils
    {
        public static int GetValueFromDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            if (dictionary.TryGetValue(player, out int value))
            {
                LogMessage($"Retrieved value {value} for player {player.Nickname}", LogLevel.Debug);
                return value;
            }
            return 0;
        }

        public static string FormatScpTerminations(Dictionary<Player, List<Tuple<RoleTypeId, string>>> scpTerminations, Player player)
        {
            if (!scpTerminations.TryGetValue(player, out var terminations) || !terminations.Any())
                return string.Empty;

            var terminationCounts = terminations
                .GroupBy(tuple => tuple.Item1)
                .ToDictionary(group => group.Key, group => group.ToList());

            var formattedTerminations = terminationCounts.Select(kvp =>
            {
                string scpName = NameFormatter.RoleNameMap.TryGetValue(kvp.Key, out var friendlyName) ? friendlyName : kvp.Key.ToString();
                var names = kvp.Value.Select(tuple => tuple.Item2).Distinct().ToList();

                if (names.Count == 1)
                {
                    return $"{scpName} [{names[0]}]";
                }
                else
                {
                    string combinedNames = string.Join(", ", names.Take(names.Count - 1)) + " and " + names.Last();
                    return $"{scpName} [{combinedNames}]";
                }
            }).ToList();

            if (formattedTerminations.Count == 1)
                return formattedTerminations[0];

            var lastScp = formattedTerminations.Last();
            formattedTerminations.RemoveAt(formattedTerminations.Count - 1);

            return string.Join(", ", formattedTerminations) + " and " + lastScp;
        }

        public enum LogLevel
        {
            Error,
            Warning,
            Debug,
            Info
        }

        public static void LogMessage(string message, LogLevel level = LogLevel.Info)
        {
            if (!ShouldLog(level))
                return;

            StackFrame frame = new StackTrace(1, false).GetFrame(1);
            var method = frame.GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType.Name;

            ConsoleColor color = GetColorForLevel(level);
            Log.Raw($"[RoundEndStats - {className}.{methodName} - {level}] {message}");
        }

        private static bool ShouldLog(LogLevel level)
        {
            if (level == LogLevel.Error)
                return true;

            return level == LogLevel.Info;
        }

        private static ConsoleColor GetColorForLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Warning:
                    return ConsoleColor.DarkBlue;
                case LogLevel.Debug:
                    return ConsoleColor.Blue;
                case LogLevel.Info:
                    return ConsoleColor.Blue;
                default:
                    return ConsoleColor.White;
            }
        }

        public static void IncrementValueInDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            if (dictionary.TryGetValue(player, out int currentValue))
            {
                dictionary[player] = currentValue + 1;
                LogMessage($"Incremented value for player {player.Nickname}. New value: {dictionary[player]}", LogLevel.Debug);
            }
            else
            {
                dictionary[player] = 1;
                LogMessage($"Initialized value for player {player.Nickname} in dictionary.", LogLevel.Debug);
            }
        }

        public static string ToHex(Color color)
        {
            int r = Mathf.FloorToInt(color.r * 255);
            int g = Mathf.FloorToInt(color.g * 255);
            int b = Mathf.FloorToInt(color.b * 255);
            string hexValue = $"#{r:X2}{g:X2}{b:X2}";
            LogMessage($"Converted Color {color} to Hex: {hexValue}", LogLevel.Debug);
            return hexValue;
        }
    }
}
