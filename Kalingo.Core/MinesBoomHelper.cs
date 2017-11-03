using System;

namespace Kalingo.Core
{
    public class MinesBoomHelper
    {
        public int GetButtonId(string id)
        {
            return int.Parse(id.Substring(id.LastIndexOf("imageButton", StringComparison.Ordinal),
                length: id.Length - id.LastIndexOf("imageButton", StringComparison.Ordinal)));
        }
    }
}