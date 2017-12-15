using Android.App;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.User;

namespace Kalingo.Core
{
    public class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static int UserId { get; set; }
        public static string EmailAddress { get; set; }
        public static int GameId { get; set; }
        public static int CountryId { get; set; }
        public static int Gold { get; set; }
        public static int Silver { get; set; }
        public static int Bronze { get; set; }
        public static bool CoinsDialogShown { get; set; }
        public static int PromoUser { get; set; }
        public static int MinesBoomId = 1;
        public static int TotalChances { get; set; }
        public static int TotalGifts { get; set; }
        public static bool InterstitialMode { get; set; }
        public static bool MixedMode { get; set; }
        public static bool MaintenanceMode { get; set; }
        public static bool PlayAgainEnabled { get; set; }
        public static string Reward { get; set; }
        public static string InterstitialAdUnit { get; set; }
        public static string RewardedAdUnit { get; set; }

        public static void SaveSessionState(UserResponse user)
        {
            App.UserId = user.UserId;
            App.EmailAddress = user.EmailAddress;
            App.Gold = user.Gold;
            App.Silver = user.Silver;
            App.Bronze = user.Bronze;
            App.CountryId = user.CountryId;
            App.PromoUser = user.PromoId;
        }

        public static void Update(Config config)
        {
            TotalChances = config.TotalChances;
            TotalGifts = config.TotalGifts;
            InterstitialMode = config.InterstitialMode;
            MaintenanceMode = config.MaintenanceMode;
            PlayAgainEnabled = config.PlayAgainEnabled;
            InterstitialAdUnit = config.InterstitialAdUnit;
            RewardedAdUnit = config.RewardedAdUnit;
            MixedMode = config.MixedMode;
        }

        public static void ClearSession()
        {
            IsUserLoggedIn = false;
            UserId = 0;
            Gold = 0;
            Silver = 0;
            Bronze = 0;
            CountryId = 0;
        }
    }
}