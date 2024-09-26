using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using Autofac;
using GameLogic.Board;
using GameLogic.BoardSolver;
using GameLogic.WinPointsCalculators;
using GameLogic.WinVerifiers;
using Infrastructure.Storage;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.Configuration.Achievements;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.Periodic;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Configuration.InAppPurchase;
using NoNameGame.Configuration.InAppPurchase.Ads;
using NoNameGame.Configuration.NewAchievements;
using NoNameGame.Configuration.NewAchievements.ProgressStorer;
using NoNameGame.Configuration.Themes;
using NoNameGame.Controllers;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Handlers.PurchaseHintSuggest;
using NoNameGame.Controllers.DomainEvents.Handlers.Telemetry;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;
using NoNameGame.Controllers.GameLogic.GameWonActions;
using NoNameGame.Controllers.Hints;
using NoNameGame.Controllers.Hints.HintsCount;
using NoNameGame.Controllers.Hints.HintsInAppProducts;
using NoNameGame.Controllers.Levels;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Controllers.Themes;
using NoNameGame.Controllers.Tutorial;
using NoNameGame.Controllers.Unlocks;
using NoNameGame.Controllers.Vibrator;
using NoNameGame.Helpers.DateTime;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Helpers.FullScreenAds.AdsEnablers;
using NoNameGame.Helpers.FullScreenAds.AdsProviders;
using NoNameGame.Helpers.Telemetry;
using NoNameGame.Levels;


namespace NoNameGame.Configuration
{
    public static class GameConfiguration
    {
        private static IContainer _container;
      
        private static readonly DbLevelProvider DbLevelProvider = new DbLevelProvider(Constants.LevelsDbConnectionString);
        private static readonly LevelPackInAppPurchaseInfoProvider _levelPackInAppPurchaseInfoProvider =
            new LevelPackInAppPurchaseInfoProvider();
        private static readonly ThemesDictionary ThemesDictionary = new ThemesDictionary();
        private static readonly AnimationDirectionDictionary AnimationDirectionDictionary = new AnimationDirectionDictionary();
        private static readonly ThemeInitialDataDictionaryBase ThemeInitialDataDictionary = CreateThemeDataDictionary();
        private static readonly AnimationDirectionDataDictionary AnimationDirectionDataDictionary = new AnimationDirectionDataDictionary();        
//        private static readonly IAchievementsCreator AchievementsCreator =
//           new AutomaticAchievementsCreator(CreatePlayerStatsProvier(), CreateThemeUnlocker(), ThemesDictionary,2*18,200*3 + 17*3);
        private static readonly IAchievementsCreator AchievementsCreator =
           new ManualAchievementsCreator(CreatePlayerStatsProvier(), CreateThemeUnlocker());
        private static readonly List<Achievement> AchviementsList; 
        private static readonly NewAchievementsProvider AchievementsProvider = new NewAchievementsProvider(CreateAchievementsStorer());
        private static readonly NewAchievementsRegistrator NewAchievementsRegistrator =
            new NewAchievementsRegistrator(AchievementsProvider,CreateLevelProvider(),CreateLevelProgressStorer());
        private static readonly HintPackInAppProductCollection _hintsPackCollection = HintsPackCollectionCreator.Create();            
        
        private static IAdsRemovalProvider _adsRemovalProvider;
        public static IAdsRemovalProvider AdsRemovalProvider
        {
            get
            {
                return _adsRemovalProvider ?? CreateAdsRemovalProvider();
            }
        }

        static GameConfiguration ()
        {
            RegisterAutoFacComponents();
            AchviementsList = AchievementsCreator.Get();           
        }
        public static LevelPackInAppPurchaseInfoProvider LevelPackInAppPurchaseInfoProvider
        {
            get { return _levelPackInAppPurchaseInfoProvider; }
        }
        public static IContainer Container
        {
            get { return _container; }
        }
        public static HintPackInAppProductCollection HintsPackCollection
        {
            get { return _hintsPackCollection; }
       }        
        private static IValueStorer<string, bool> CreateHintsPackPurchasedMarker()
        {
            return new SecureValueStorer<string, bool>(new ApsSettingsValueStorer<string, byte[]>(), bool.Parse);
        }
        private static void RegisterAutoFacComponents()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(ApsSettingsValueStorer<,>)).AsImplementedInterfaces().SingleInstance();

            var serviceName = "AutofacEventBus";
            containerBuilder.RegisterType<AutofacEventBus>().Named<IEventsBus>(serviceName).SingleInstance();
            containerBuilder.RegisterDecorator<IEventsBus>((c, inner) => new BackgroundThreadEventsBus(inner), serviceName).SingleInstance();

            containerBuilder.RegisterType<DateTimeNowProvider>().AsImplementedInterfaces().SingleInstance();

            NewAchievementsRegistrator.Register(containerBuilder);
            RegsiterEventHandlers(containerBuilder);

            containerBuilder.RegisterType<AnimateHintButtonSuggester>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.Register(context => new OnNthGameWon(Constants.InGameFullSreenAdsPeriod)).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<AppSettingsHintsCountStorer>().AsImplementedInterfaces().SingleInstance();            
            containerBuilder.Register(context => new HintsPackProductFullfiller(HintsPackCollection,
                                                                                    context.Resolve<IHintsCountIncreaser>(),
                                                                                    CreateHintsPackPurchasedMarker()
                                                                                    ))

                .AsImplementedInterfaces().SingleInstance();
            containerBuilder.Register(ctx => new CurrentLevelDataProvider(CreateLevelProvider())).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<TelemetryClient>().AsSelf().SingleInstance();
            
            _container = containerBuilder.Build();
           
        }
        private static void RegsiterEventHandlers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SendToastPromptHandler>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<GameInterruptedHandler>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<PlayerMadeManyMoves>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.Register(context => new GiveFreeHintsHandler(AchievementKey.AppStartedEveryDayStreak,
                Constants.FreeHintsForAppUsage, context.Resolve<IHintsCountIncreaser>()))
                .AsImplementedInterfaces().SingleInstance();
            containerBuilder.Register(
                context => new GameLeftHandler(context.Resolve<ICurrentLevelDataProvider>(), CreatePlayerStatsProvier(),
                    context.Resolve<TelemetryClient>()))
                .AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<AchievementUnlockedHandler>().AsImplementedInterfaces().SingleInstance();
        }
        public static IEventsBus CreateEventBus()
        {
            return Container.Resolve<IEventsBus>();
        }
        public static NewAchievementsProvider GetNewAchievementsProvider()
        {
            return AchievementsProvider;
        }
        public static NewAchievementsController CreateNewAchievementsController()
        {
            return new NewAchievementsController(GetNewAchievementsProvider().Get());
        }

        public static HintsPurchaseController CreateHintsController()
        {
            return new HintsPurchaseController(HintsPackCollection,Container.Resolve<IHintsCountProvider>(),Container.Resolve<IHintsPackProductFullfiller>());   
        }
        private static IValueStorer<AchievementKey,NewAchievementDto> _achievementStorer;
      
        private static IValueStorer<AchievementKey,NewAchievementDto> CreateAchievementsStorer()
        {
            if (_achievementStorer == null)
            {
                _achievementStorer = new AppSettingsProgressStorer();
            }
            return _achievementStorer;
        }
        private static ThemeInitialDataDictionaryBase CreateThemeDataDictionary()
        {
           //return new AllUnlockedThemeInitialDataDictionary(new ThemeInitialDataDictionary());
           return new AllThemesLockedExcept(ThemesDictionary.DefaultTheme,ThemesDictionary);
        }
        public static IWinPointsCalculator CreatePointsCalculator()
        {
            return new StarsWinPointsCalculator();
        }     
        public static IWinVerifier CreateWinVerifier(IEnumerable<BoardCoordinate> disabledAreasCoordinates )
        {
            if (disabledAreasCoordinates == null) throw new ArgumentNullException("disabledAreasCoordinates");
            //return new AllAreasMustBeChecked();
            return new WinVerifierWithDisabledAreas(new AllAreasMustBeChecked(),disabledAreasCoordinates);
        }
        public static IBoardSolver CreateSolver()
        {
            return new SimpleBoardSolver();
        }
        public static ILevelProvider CreateLevelProvider()
        {
            return DbLevelProvider;
        }
        public static ThemeController CreateThemeController()
        {
            return new ThemeController(ThemesDictionary);
        }
        public static ThemeBrushesProvider CreateThemeBrushesProvider()
        {
            return new ThemeBrushesProvider(ThemesDictionary);
        }
        public static AnimationDirectionController CreateAnimationDirectionController()
        {
            return new AnimationDirectionController(AnimationDirectionDictionary);
        }
        public static IFiniteTypeStorer<ThemeType,ThemeData> CreateThemeUnlocker()
        {
            return new AppSettingsFiniteStorer<ThemeType, ThemeData>(x=> string.Format("{0}_{1}", x.GetType().Name, x.ToString()),ThemeInitialDataDictionary);
        }
        public static IFiniteTypeStorer<AnimationDirectionType, AnimationDirectionData> CreateAnimationDirectionUnlocker()
        {
            return
                new AppSettingsFiniteStorer<AnimationDirectionType, AnimationDirectionData>(
                    x => string.Format("{0}_{1}", x.GetType().Name, x.ToString()), AnimationDirectionDataDictionary);
        }
        public static AchievementsExecutor GetAchievementsController()
        {
            return new AchievementsExecutor(AchviementsList);
        }
        public static IPeriodicAnimationFactory GetPeriodicAnimationFactory()
        {
            return new NullPeriodicAnimationFactory();
        }
        public static ICurrentAnimationDelayProvider CreateCurrentAnimationDelayProvider()
        {
            return new CurrentAnimationDelayProvier(CreateAnimationDirectionController());
        }

        private static IPlayerStatsProvider _instance;
        public static IPlayerStatsProvider CreatePlayerStatsProvier()
        {
            return _instance ??
                   (_instance =
                       new CalculatedPlayerStats(CreateLevelProgressStorer(), CreateLevelProvider(),
                           CreatePointsCalculator()));            
        }
        public static CompositeGameWonAction CreateGameWonAction()
        {
            return new CompositeGameWonAction(new AssignStarsToWonLevel(CreateLevelProgressStorer()),
                new UnlockNextLevel(CreateLevelProvider(),CreateLevelProgressStorer()),
                new AssignSolveTimeAction(CreateLevelProgressStorer()),
                new UpdateLiveTileGameWonAction(CreatePlayerStatsProvier()),
                new SendTelemetryInformation(CreatePlayerStatsProvier(), Container.Resolve<ICurrentLevelDataProvider>(),
                    Container.Resolve<TelemetryClient>()));
        }
        public static ThemeStarsToUnlockProvider CreateThemeConditionDescriptionProvier()
        {
            return new ThemeStarsToUnlockProvider(AchviementsList);
        }

        public static LevelsController CreateLevelController()
        {
            return new LevelsController(CreateLevelProvider(),CreateLevelProgressStorer(),CreatePointsCalculator(),LevelPackInAppPurchaseInfoProvider);
        }
        public static ILevelProgressStorer CreateLevelProgressStorer()
        {
            return new AppSettingsLevelProgressStorer();
        }
        public static VibrationController CreateVibrationController()
        {
            return new VibrationController();
        }
        public static ProgressResetter GetProgressResetter()
        {
            return new ProgressResetter(CreateThemeController(),AchievementsProvider);
        }
        public static ITutorialController CreateTutorialController()
        {
            return new AlwaysShowedTutorialController();
        }
        public static PlayerStatsController CreatePlayerStatsController()
        {
            return new PlayerStatsController(CreatePlayerStatsProvier());
        }
        public static NewItemUnlockedStorer CreateNewItemUnlockedStorer()
        {
            return new NewItemUnlockedStorer();
        }
        public static IAreaStateTransitionManagerFactory CreateAreaStateTransitionManagerFactory()
        {
            return new AreaStateTransitionManagerFactory();
        }
        public static TutorialControlDisplayer CreateTutorialWindowProvider()
        {
            return new TutorialControlDisplayer(CreateTutorialController(), CreateLevelProvider());
        }
        public static IFullScreenAdDisplayer CreateAppExitAdDisplayer()
        {

//            return new DisableableFullScreenAdDisplayer(new SmatooAdDisplayer(
//                () => Application.Current.Terminate(),
//                () => Application.Current.Terminate()),
//                     () => !AdsRemovalProvider.AreRemoved(), () => Application.Current.Terminate());

            return new DisableableFullScreenAdDisplayer(new GoogleFullScreenAdDisplayer(Constants.FullScreenGoogleAdId,
                    () => Application.Current.Terminate(),
                    () => Application.Current.Terminate()),
                            () => !AdsRemovalProvider.AreRemoved(), () => Application.Current.Terminate());


        }
        public static InGameFullSceenAdDisplayerFactory CreateInGameFullScreenAdsDisplayerFactory()
        {           
           return new InGameFullSceenAdDisplayerFactory(new AllEnabled(new FuncAdsEnabler(() => !AdsRemovalProvider.AreRemoved()),
                                                                      Container.Resolve<IAdsEnabler>()));
        }
        private static IAdsRemovalProvider CreateAdsRemovalProvider()
        {
            return new Any(new AdsRemovedWithAnyHintsPack(CreateHintsPackPurchasedMarker()),
                           new AdsRemovedWithUnlockedLevelPack(LevelPackInAppPurchaseInfoProvider, levelPackId: 12));
            // return new AdsRemovedWithUnlockedLevelPack(LevelPackInAppPurchaseInfoProvider, levelPackId: 12);
        }
        public static ChallengeLoginController CreateChallengeLoginController(MobileServiceClient invertItServiceClient)
        {
            return new ChallengeLoginController( new CustomAuthenticator(invertItServiceClient), 
                new AppSettingsAuthorizationCache( 
                    new SecureValueStorer<string, string>(
                        new ApsSettingsValueStorer<string, byte[]>(), s => s)), invertItServiceClient);
        }
        public static ChallengesController CreateChallengesController(MobileServiceClient invertItServiceClient)
        {
            return new ChallengesController(invertItServiceClient);
        }
    }
}