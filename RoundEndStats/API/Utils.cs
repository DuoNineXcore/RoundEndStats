using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Enums;
using PlayerRoles;
using UnityEngine;

namespace RES.API
{
    public static class Utils
    {
        public static int GetValueFromDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            return dictionary.ContainsKey(player) ? dictionary[player] : 0;
        }

        public static string FormatScpTerminations(Dictionary<Player, List<RoleTypeId>> scpTerminations, Player player)
        {
            if (!scpTerminations.ContainsKey(player) || !scpTerminations[player].Any())
                return string.Empty;

            var terminatedScps = scpTerminations[player].Select(role => role.ToString()).ToList();

            if (terminatedScps.Count == 1)
                return terminatedScps[0];

            var lastScp = terminatedScps.Last();
            terminatedScps.RemoveAt(terminatedScps.Count - 1);

            return string.Join(", ", terminatedScps) + " and " + lastScp;
        }

        public static void IncrementValueInDictionary(Player player, Dictionary<Player, int> dictionary)
        {
            if (dictionary.ContainsKey(player))
                dictionary[player]++;
            else
                dictionary[player] = 1;
        }

        public static string ToHex(Color color)
        {
            int r = Mathf.FloorToInt(color.r * 255);
            int g = Mathf.FloorToInt(color.g * 255);
            int b = Mathf.FloorToInt(color.b * 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
