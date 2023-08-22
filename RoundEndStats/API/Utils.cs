using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;

namespace RES.API
{
    public class Utils
    {
        public int GetValueFromDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            return dictionary.ContainsKey(player) ? dictionary[player] : 0;
        }

        public string FormatScpTerminations(Dictionary<Player, List<Tuple<RoleTypeId, string>>> scpTerminations, Player player)
        {
            if (!scpTerminations.ContainsKey(player) || !scpTerminations[player].Any())
                return string.Empty;

            var terminationCounts = scpTerminations[player]
                .GroupBy(tuple => tuple.Item1)
                .ToDictionary(group => group.Key, group => group.ToList());

            var formattedTerminations = terminationCounts.Select(kvp =>
            {
                string scpName = RoleNameFormatter.RoleNameMap.TryGetValue(kvp.Key, out var friendlyName) ? friendlyName : kvp.Key.ToString();
                var names = kvp.Value.Select(tuple => tuple.Item2).Distinct().ToList();

                if (names.Count() == 1)
                {
                    return $"{scpName} [{names[0]}]";
                }
                else
                {
                    string combinedNames = string.Join(", ", names.Take(names.Count() - 1)) + " and " + names.Last();
                    return $"{scpName} [{combinedNames}]";
                }
            }).ToList();

            if (formattedTerminations.Count() == 1)
                return formattedTerminations[0];

            var lastScp = formattedTerminations.Last();
            formattedTerminations.RemoveAt(formattedTerminations.Count() - 1);

            return string.Join(", ", formattedTerminations) + " and " + lastScp;
        }

        public void LogMessage(string message, ConsoleColor color)
        {
            StackFrame frame = new StackTrace(1, false).GetFrame(1); // 1 up from this method
            string methodName = frame.GetMethod().Name;
            string className = frame.GetMethod().DeclaringType.Name;

            if (RoundEndStats.Instance.Config.Debug)
            {
                Log.SendRaw($"[RoundEndStats - {className}.{methodName}] {message}", color);
            }
        }

        public void IncrementValueInDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            if (dictionary.ContainsKey(player))
                dictionary[player]++;
            else
                dictionary[player] = 1;
        }

        public string ToHex(Color color)
        {
            int r = Mathf.FloorToInt(color.r * 255);
            int g = Mathf.FloorToInt(color.g * 255);
            int b = Mathf.FloorToInt(color.b * 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
