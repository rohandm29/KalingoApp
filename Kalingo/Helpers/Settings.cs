// Helpers/Settings.cs This file was automatically added when you installed the Settings Plugin.If you are not using a PCL then comment this file back in to use it.

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Kalingo.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications.All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static void Add(string key, string value)
        {
            AppSettings.AddOrUpdateValue(key, value);
        }

        public static string Get(string key)
        {
            return AppSettings.GetValueOrDefault(key, SettingsDefault);
        }
    }
}