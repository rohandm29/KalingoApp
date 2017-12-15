using System;

namespace Kalingo.Core
{
    public class MinesBoomHelper
    {
        private static readonly Random Random = new Random(10);

        public int GetButtonId(string id)
        {
            return int.Parse(id.Substring(id.LastIndexOf("imageButton", StringComparison.Ordinal),
                id.Length - id.LastIndexOf("imageButton", StringComparison.Ordinal)));
        }

        public static bool GetRandomFlag()
        {
            return Random.Next(10)%2 == 0;
        }

        public static string GetCoinType(int id)
        {
            switch (id)
            {
                case 1:
                    return "Gold";
                case 2:
                    return "Silver";
                case 3:
                    return "Bronze";
                default:
                    return string.Empty;
            }
        }
    }
}