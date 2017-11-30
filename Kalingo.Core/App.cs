using Android.App;

namespace Kalingo.Core
{
    public class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static string UserId { get; set; }
        public static int GameId { get; set; }
        public static int CountryId { get; set; }
        public static int Gold { get; set; }
        public static int Silver { get; set; }
        public static bool CoinsDialogShown { get; set; }
        public static int PromoUser { get; set; }
        public static int MinesBoomId = 1;
        public static int TotalChances { get; set; }
        public static int TotalGifts { get; set; }
        public static bool InterstitialMode { get; set; }
        public static bool MaintenanceMode { get; set; }
        public static bool PlayAgainEnabled { get; set; }

        public static void Update(int totalChances, int totalGifts, bool interstitialMode, bool maintenanceMode, bool playAgainEnabled)
        {
            TotalChances = totalChances;
            TotalGifts = totalGifts;
            InterstitialMode = interstitialMode;
            MaintenanceMode = maintenanceMode;
            PlayAgainEnabled = playAgainEnabled;
        }

        public static void ClearSession()
        {
            IsUserLoggedIn = false;
            UserId = "0";
            Gold = 0;
            Silver = 0;
            CountryId = 0;
        }
    }
}