using System.Dynamic;
using System.Windows.Media;

namespace NoNameGame.Configuration
{
    public static class Constants
    {

        
        public const string LevelsDbConnectionString =
            @"Data Source = appdata:/Levels\Levels.sdf; File Mode = read only;";
        public const string LevelsDbConnectionStringIso =
          @"Data Source = isostore:/Shared\Levels.sdf;";
        public const string GameName = "Invert it";

        public const string FullScreenGoogleAdId = "ca-app-pub-4997101767812389/4623772356";
        public static readonly string  VServZoneId = "bf5a1c33";
        public const string AdDealsAppId = "937";
        public const string AdDealsAppKey = "B4WYTT9BFPM1";

        public const string FacebookFanPageUrl = "https://www.facebook.com/InvertItFree";
        public const string FacebookContestUrl = @"https://www.facebook.com/InvertItFree/photos/a.750209911731533.1073741832.727313000687891/784926894926501/?type=1";
        public const string FeedbackEmail = "m.wudarczyk@hotmail.com";


        //"http://localhost:50521";
        //        "https://invertit.azure-mobile.net/",
        //      "dlUNQmiGGsloSfAQKyRsVeAxuwCIkc74"
        public const string InvertItServiceAddress = "http://localhost:50521";
        public const string InvertItServiceAppKey = "dlUNQmiGGsloSfAQKyRsVeAxuwCIkc74";
        public const string MsClientId = "0000000040136B5A";


       public static readonly Color AppBarForegroundColor = new Color() { A = 255, R = 50, B = 50, G = 50 };
       

        //public static readonly Color AppBarForegroundColor = Color.FromArgb(255, 181, 43, 27);

        public static readonly Color GameShapeStrokeColor = Color.FromArgb(255, 181, 43, 27);
        public static readonly double GameShapeStrokeWidth = 3.0;

        public static readonly Color PhoneChromeColor = Color.FromArgb(255, 221, 221, 221);
        public const int OverlayShapeSize = 60;
        public static readonly int AreaShapeMargin = 3;

        public static readonly int AskForRatingInitialDelay =1;
        public static readonly int AskForRatingPeriodLen = 3;
        public static readonly string RemoveAdsAppProductId = "9x9UltraLevelPack";
        public static readonly int InGameFullSreenAdsPeriod = 4;

        public static readonly int DefaultHintsCount = 3;
        public static readonly int GetHintBounceAnimationMaxHeight = 6;
        public static readonly int FreeHintsForAppUsage = 5;
        public static readonly int PlayEveryDayStreakLenght = 4;
        public static readonly int FreeHintsForAppUsageMessageInitialDelay =2;

        public static readonly int PopupWindowsButtonSize = 90;
        public static readonly string AdDuplexAppKey = "91ead24b-4e15-4d18-8fa1-71fa4016e292";
        public static readonly int TapAnimationTimeMs = 150;
    }
}