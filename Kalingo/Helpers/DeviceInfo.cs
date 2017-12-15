using System;

namespace Kalingo.Helpers
{
    public class DeviceInfo
    {
        public const string Version = "1.0";

        public static string GetDeviceId()
        {
            try
            {
                return Plugin.DeviceInfo.CrossDeviceInfo.Current.Id;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}