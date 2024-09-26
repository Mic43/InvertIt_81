using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using AdDealsSDKWP8;
using Autofac;
using GameLogic.Board;
using GameLogic.Game;
using ImageTools.IO.Gif;
using Infrastructure.Storage;
using Microsoft.ApplicationInsights;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Configuration.InAppPurchase.Ads;
using NoNameGame.Controllers;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Controllers.GameLogic;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.Controllers.GameLogic.GameWonActions;
using NoNameGame.Controllers.Hints;
using NoNameGame.Controllers.Hints.HintsCount;
using NoNameGame.Controllers.Levels;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Controllers.Sound;
using NoNameGame.Controllers.Themes;
using NoNameGame.Controllers.Tutorial;
using NoNameGame.Controllers.Vibrator;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Levels;
using NoNameGame.Resources;

//#if DEBUG
//    using MockIAPLib;
//    using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
//#endif


namespace NoNameGame
{
    public partial class App : Application
    {        
        public TutorialControlDisplayer TutorialControlDisplayer { get;  private set; }
        public GameController GameController { get; private set; }
        public ChallangeGameController ChallengeGameController { get; private set; }
        public HintsPurchaseController HintsPurchaseController { get; set; }
        public LevelsController LevelsController { get; private set; }
        public ThemeController ThemeController { get; private set; }
        public ThemeBrushesProvider ThemeBrushesProvider { get; private set; }
        public ITutorialController TutorialController { get; set; }
        public AnimationDirectionController AnimationDirectionController { get; private set; }
        public SoundController SoundController { get; private set; }
        public VibrationController VibrationController { get; private set; }
        public IFiniteTypeStorer<ThemeType,ThemeData> ThemeUnlocker { get;  private set; }
        public ICurrentAnimationDelayProvider CurrentAnimationDelayProvider { get; set; }
        public IFiniteTypeStorer<AnimationDirectionType, AnimationDirectionData> AnimationDirectionUnlocker { get; private set; }
        public IPeriodicAnimationFactory PeriodicAnimationFactory { get; private set; }
        public ThemeStarsToUnlockProvider ThemeStarsToUnlockProvider { get; private set; }
        public ProgressResetter ProgressResetter { get; private set; }
        public PlayerStatsController PlayerStatsController { get; private set; }
        public NewItemUnlockedStorer NewItemUnlockedStorer { get; private set; }
        public NewAchievementsController NewAchievementsController { get; private set; }
        public IEventsBus EventsBus { get; private set; }
        public IAreaStateTransitionsManager AreaStateTransitionManager { get; private set; }

        public ChallengeLoginController ChallengeLoginController { get; private set; }
    

        public IAdsRemovalProvider AdsRemovalProvider { get; private set; }
        public IFullScreenAdDisplayer AppExitAdDisplayer { get; private set; }
        public InGameFullSceenAdDisplayerFactory InGameFullSceenAdDisplayerFactory { get; private set; }

        public Stopwatch Stopwatch { get; private set; }
        private void InitializeControllers()
        {
            EventsBus = GameConfiguration.CreateEventBus();
            LevelsController = GameConfiguration.CreateLevelController();
            ThemeController = GameConfiguration.CreateThemeController();
            ThemeUnlocker = GameConfiguration.CreateThemeUnlocker();
            VibrationController = GameConfiguration.CreateVibrationController();
            AnimationDirectionUnlocker = GameConfiguration.CreateAnimationDirectionUnlocker();
            AnimationDirectionController = GameConfiguration.CreateAnimationDirectionController();
            GameController = new GameController(GameConfiguration.CreateLevelProvider(),
                GameConfiguration.GetAchievementsController(),
                GameConfiguration.CreateGameWonAction(),GameConfiguration.CreateNewItemUnlockedStorer(),
                EventsBus, GameConfiguration.CreateLevelProgressStorer(),
                GameConfiguration.Container.Resolve<IHintsCountDecreaser>(),
                GameConfiguration.Container.Resolve<IHintsCountProvider>(), GameConfiguration.Container.Resolve<ICurrentLevelDataProvider>()
                );
            ChallengeGameController = new ChallangeGameController(new EmptyGameWonAction(), GameConfiguration.CreatePointsCalculator(),
                GameConfiguration.CreateWinVerifier(Enumerable.Empty<BoardCoordinate>()),GameConfiguration.CreateSolver(),EventsBus);
            SoundController = new SoundController();
            PeriodicAnimationFactory = GameConfiguration.GetPeriodicAnimationFactory();
            ThemeBrushesProvider = GameConfiguration.CreateThemeBrushesProvider();
            ThemeStarsToUnlockProvider = GameConfiguration.CreateThemeConditionDescriptionProvier();
            CurrentAnimationDelayProvider = GameConfiguration.CreateCurrentAnimationDelayProvider();
            ProgressResetter = GameConfiguration.GetProgressResetter();
            TutorialController = GameConfiguration.CreateTutorialController();
            PlayerStatsController = GameConfiguration.CreatePlayerStatsController();
            NewItemUnlockedStorer = GameConfiguration.CreateNewItemUnlockedStorer();
            AreaStateTransitionManager = GameConfiguration.CreateAreaStateTransitionManagerFactory().Create(ThemeController.CurrentTheme);
            TutorialControlDisplayer = GameConfiguration.CreateTutorialWindowProvider();
            NewAchievementsController = GameConfiguration.CreateNewAchievementsController();

            AppExitAdDisplayer = GameConfiguration.CreateAppExitAdDisplayer();
            InGameFullSceenAdDisplayerFactory = GameConfiguration.CreateInGameFullScreenAdsDisplayerFactory();
            AdsRemovalProvider = GameConfiguration.AdsRemovalProvider;
            HintsPurchaseController = GameConfiguration.CreateHintsController();
            ChallengeLoginController = GameConfiguration.CreateChallengeLoginController(InvertItServiceClient);
            ChallengesController = GameConfiguration.CreateChallengesController(InvertItServiceClient);

        }
        public ChallengesController ChallengesController { get; set; }


        public MobileServiceClient InvertItServiceClient = new MobileServiceClient(Constants.InvertItServiceAddress,Constants.InvertItServiceAppKey);

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        public class IsDeviceAlreadyRegisteredResposne
        {
            public bool Value { get; set; }
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            WindowsAppInitializer.InitializeAsync();
            UnhandledException += Application_UnhandledException;
                        
            // Standard XAML initialization
            InitializeComponent(); 
            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Current.Host.Settings.EnableFrameRateCounter = true;             
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
            Stopwatch = new Stopwatch();
            InitializeControllers();
            ImageTools.IO.Decoders.AddDecoder<GifDecoder>();          
        }        

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            AdManager.Init(Constants.AdDealsAppId, Constants.AdDealsAppKey);
            AdDuplex.AdDuplexTrackingSDK.StartTracking(Constants.AdDuplexAppKey);
            HintsPurchaseController.FullfillPendingPurchases();

            EventsBus.Publish(new AppStarted());
        }

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            AdManager.Init(Constants.AdDealsAppId, Constants.AdDealsAppKey);

            if (e.IsApplicationInstancePreserved && !GameController.IsGameFinished)
            {     
               GameController.ResumeGame();
                return;
            }

            // Check to see if the key for the application state data is in the State dictionary.
            if (PhoneApplicationService.Current.State.ContainsKey("GameData"))
            {
                GameController.RestoreGame((AppRestoreData)PhoneApplicationService.Current.State["GameData"]);
            }
            // Must be called only when app was tombstoned - in dormant it is called to early before RequestPurchase is finished
            HintsPurchaseController.FullfillPendingPurchases();

            EventsBus.Publish(new AppActivated());
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            MusicPlayer.Stop();
            if (GameController.CanSerialize())
            {
                var gameData = GameController.SerializeGame();
                PhoneApplicationService.Current.State["GameData"] = gameData;               
            }         
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            HandleAdsErrors(e);
            SendExceptionInfo(e.ExceptionObject);
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }
        private void SendExceptionInfo(Exception ex)
        {
            var telemetryClient = GameConfiguration.Container.Resolve<TelemetryClient>();            
            telemetryClient.TrackException(ex);
            telemetryClient.Flush();
        }
        private void HandleAdsErrors(ApplicationUnhandledExceptionEventArgs e)
        {
            if (e != null)
            {
                Exception exception = e.ExceptionObject;
                if ((exception is XmlException || exception is NullReferenceException) &&
                    exception.ToString().ToUpper().Contains("INNERACTIVE"))
                {
                    Debug.WriteLine("Handled Inneractive exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is NullReferenceException && exception.ToString().ToUpper().Contains("SOMA"))
                {
                    Debug.WriteLine("Handled Smaato null reference exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if ((exception is System.IO.IOException || exception is NullReferenceException) &&
                         exception.ToString().ToUpper().Contains("GOOGLE"))
                {
                    Debug.WriteLine("Handled Google exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is ObjectDisposedException && exception.ToString().ToUpper().Contains("MOBFOX"))
                {
                    Debug.WriteLine("Handled Mobfox exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if ((exception is NullReferenceException) &&
                         exception.ToString().ToUpper().Contains("MICROSOFT.ADVERTISING"))
                {
                    Debug.WriteLine("Handled Microsoft.Advertising exception {0}", exception);
                    e.Handled = true;
                    return;
                }
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;
        private bool reset;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();

            RootFrame.Navigating+= RootFrameOnNavigating;
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += RootFrameOnNavigate;


            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

          
        }

        private void RootFrameOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (reset && e.IsCancelable && e.Uri.OriginalString == "/MainPage.xaml")
            {
                e.Cancel = true;
                reset = false;
            }
        }
        private void RootFrameOnNavigate(object sender, NavigationEventArgs e)
        {
            reset = e.NavigationMode == NavigationMode.Reset;

        }

        // Do not add any additional code to this method

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
 
}