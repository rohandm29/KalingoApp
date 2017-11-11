using Android.App;

namespace Kalingo.Core
{
    public class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }

        public static int UserId { get; set; }

        public static int GameId { get; set; }

        public static int CountryId { get; set; }

        public static int Gold { get; set; }
        
        public static int Silver { get; set; }

        public static bool CoinsDialogShown { get; set; }

        public static int PromoUser { get; set; }
    }
}