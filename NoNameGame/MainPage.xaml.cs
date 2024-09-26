using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using GameLogic;
using GameLogic.Board;
using Infrastructure.Storage;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.Xna.Framework.Media;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Configuration.InAppPurchase;
using NoNameGame.Controllers;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.Controllers.GameLogic.Challenges.Login.Authenticators;
using NoNameGame.Controllers.GameLogic.Challenges.Login.AuthorizationCache;
using NoNameGame.Controllers.Sound;
using NoNameGame.CustomControls;
using NoNameGame.CustomControls.ClickSound;
using NoNameGame.CustomControls.Levels;
using NoNameGame.CustomControls.Popups;
using NoNameGame.Helpers;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Helpers.Network;
using NoNameGame.Models.ChallengeGame;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

//#if DEBUG
//using MockIAPLib;
//using Store = MockIAPLib;
//#else
using Windows.ApplicationModel.Store;
//#endif

namespace NoNameGame
{   
    public partial class MainPage : BasePage, IAppRunListener
    {
        public event EventHandler AppRan;
        private EasterEggActivator _easterEggActivator;
        private SingleAnimationCreator _hideSplashScreenAnimation;
        private bool _isNewInstance;
        private readonly IFullScreenAdDisplayer _fullScreenAdDisplayer;
        private AskForRatingDisplayer _askForRating;
        private SingleAnimationCreator _contestIconAnimation;
        private PlayEveryDayMessageDisplayer _playEveryDayDisplayer;
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Themer.EnableThemesForControls(PanoramaControl);
         
            StarsProgressBar.StarsProgressModel = this.CurrentApp().PlayerStatsController.GetStarsProgress();
            RemoveAdsButton.Visibility = GameConfiguration.AdsRemovalProvider.AreRemoved()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
        private void PlayContesIcontAnimation()
        {
          //  _contestIconAnimation.Create(ContestIcon).Begin();
        }
        private void PlayAdealdsAnimation()
        {
            new SingleAnimationCreator(AnimationsRepository.CreateBounceAnimation(15,TimeSpan.FromMilliseconds(400))).Create(AdDealsIcon).Begin();
        }
        public MainPage()
        {                    
            InitializeComponent();

            Loaded += OnLoaded;
            PanoramaControl.ManipulationStarted+=PanoramaControlOnManipulationStarted;

            _isNewInstance = true;
            _easterEggActivator = new EasterEggActivator();
            _fullScreenAdDisplayer = this.CurrentApp().AppExitAdDisplayer;
            _hideSplashScreenAnimation = new SingleAnimationCreator(AnimationsRepository.CreateFadeToViewAnimation(0, TimeSpan.FromMilliseconds(300)));
            _contestIconAnimation = new SingleAnimationCreator(AnimationBuilder.Scale().Uniform().To(1.2).AutoReverse().RepeatForever().WithDuration(500).Build());
            _askForRating = new AskForRatingDisplayer(this,this.CurrentApp().EventsBus, runCount => runCount >Constants.AskForRatingInitialDelay && runCount % Constants.AskForRatingPeriodLen == 0);
            _playEveryDayDisplayer = new PlayEveryDayMessageDisplayer(Constants.FreeHintsForAppUsage, Constants.PlayEveryDayStreakLenght, Constants.FreeHintsForAppUsageMessageInitialDelay);

            PlayContesIcontAnimation();
            PlayAdealdsAnimation();

            TiltEffect.TiltableItems.Add(typeof(ClickSoundContentControl));         
        }
        private void PanoramaControlOnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            OverlayTest.PlayMoveAnimation(e.ManipulationContainer
                                           .TransformToVisual(OverlayTest)
                                           .Transform(e.ManipulationOrigin),
                TimeSpan.FromMilliseconds(500));
                                              
        }       
        private  void ShowSplashScreen()
        {            
            var splashScreen = new SplashScreen();
            var popup = new Popup() {IsOpen = true, Child = splashScreen};
            splashScreen.AnimationFinished += (sender, args) =>
            {
                //OverlayTest.RecreateShapes();
                var storyboard = _hideSplashScreenAnimation.Create(splashScreen);
                storyboard.Completed += (o, eventArgs) =>
                {
                    popup.IsOpen = false;
                    OnSplashScreenShown();
                };
                storyboard.Begin();
                _fullScreenAdDisplayer.Preload();
            };

        }
        private void OnSplashScreenShown()
        {
            _askForRating.ShowDialogIfNeeded(this);            
            _playEveryDayDisplayer.ShowDialogIfNeeded(this);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            while (NavigationService.CanGoBack) NavigationService.RemoveBackEntry();

            if (AppNewlyStarted(e))
            {
                RaiseAppRan();
                ShowSplashScreen();
                _isNewInstance = false;
            }
            else             
                _fullScreenAdDisplayer.Preload();
            
            ToggleBackgroundMusic();
            RefreshLevelPacksList();

        }
        private bool AppNewlyStarted(NavigationEventArgs e)
        {
            return e.NavigationMode == NavigationMode.New && _isNewInstance;
        }
        private static bool IsBackWithinTheApp(NavigationEventArgs e)
        {
            return e.NavigationMode == NavigationMode.Back && e.IsNavigationInitiator;
        }
        private static void ToggleBackgroundMusic()
        {
            if (MediaPlayer.GameHasControl)
            {
                if (!MusicPlayer.IsPlaying())
                    MusicPlayer.Play();
            }
            else
            {
                MusicPlayer.Stop();
            }
        }
        private void RaiseAppRan()
        {
            if (AppRan != null)
                AppRan(this, EventArgs.Empty);
        }
        private void LevelPacksListControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            LevelPacksListControl.LevelPackSelected += LevelLPacksListControlLevelPackSelected;
        //    RefreshLevelPacksList();
        }
        private void RefreshLevelPacksList()
        {
            LevelPacksListControl.LevelPacksListControlModel = this.CurrentApp().LevelsController.GetLevelPacksListModel();
        }
        void LevelLPacksListControlLevelPackSelected(object sender, LevelPackModel selectedLevelPackModel)
        {
            if (selectedLevelPackModel.IsLocked)
            {
                TryPerformInAppPurchase(selectedLevelPackModel);
            }
            else                                        
                NavigationService.Navigate(new Uri(string.Format("/NewGamePage.xaml?LevelPackId={0}", selectedLevelPackModel.Id), UriKind.Relative));
        }
        private async void TryPerformInAppPurchase(LevelPackModel selectedLevelPackModel)
        {
            var levelPackInAppPurchaseInfo =
              GameConfiguration.LevelPackInAppPurchaseInfoProvider.Get(selectedLevelPackModel.Id);
              
          
            if (!IsLevelPurchased(levelPackInAppPurchaseInfo))
            {
                try
                {
                    await CurrentApp.RequestProductPurchaseAsync(levelPackInAppPurchaseInfo.ProductId, true);
                    RefreshLevelPacksList();
                }
                catch (Exception)
                {
                    // MessageBox.Show(e.ToString());
                }             
            }            
                
        }
        private  bool IsLevelPurchased(LevelPackInAppPurchaseInfo levelPackInAppPurchaseInfo)
        {
            var productLicenses = CurrentApp.LicenseInformation.ProductLicenses;
            return !productLicenses.ContainsKey(levelPackInAppPurchaseInfo.ProductId) &&
                    productLicenses[levelPackInAppPurchaseInfo.ProductId].IsActive;
        }

        private void PanoramaControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundEffectsPlayer.Current.SwypeEffect.Play();           
        }
        private void DashboardCommandsControl_OnDashboardCommandExecuted(object sender, DashBoardCommandModel executedcommand)
        {
           executedcommand.Command.Invoke(this);
        }
                
        private void StarsProgressBar_OnTap(object sender, GestureEventArgs e)
        {
           _easterEggActivator.Hit();
        }
        private void MainPage_OnBackKeyPress(object sender, CancelEventArgs e)
        {
            _fullScreenAdDisplayer.TryShowAsync();
            e.Cancel = true;
        }
        private void AboutControl_OnCreditsPageRequested(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(@"/CreditsPage.xaml", UriKind.Relative));
        }
        private void AdDealsIcon_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdDealsSDKWP8;component/Views/MoreAdDeals.xaml", UriKind.Relative));            
        }
        private async void RemoveAdsButton_OnTap(object sender, GestureEventArgs e)
        {
            if (!GameConfiguration.AdsRemovalProvider.AreRemoved())
            {
                try
                {
                    await CurrentApp.RequestProductPurchaseAsync(Constants.RemoveAdsAppProductId, true);
                    RefreshLevelPacksList();
                }
                catch (Exception)
                {
                    // MessageBox.Show(e.ToString());
                }
            }
        }
        private  void ChallengeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.CurrentApp().ChallengeLoginController.IsAuthTokenCached())
            {
                this.CurrentApp().ChallengeLoginController.LoginWithCachedAuthToken();
                NavigationService.Navigate(new Uri(@"/ChallengePages/DashboardPage.xaml", UriKind.Relative));
            }
            else
                NavigationService.Navigate(new Uri(@"/ChallengePages/WelcomePage.xaml", UriKind.Relative));

            // NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));            
     

        //    var apsSettingsValueStorer = new ApsSettingsValueStorer<string, AuthorizationCacheData>();
           // new ChallengeLoginController(new FromCacheAuthenticator(new MobileServiceAuthorizationCache(apsSettingsValueStorer),
//            this.CurrentApp().ChallengeGameController.StartNewChallenge(new Challenge(
//                new Level(new List<BoardCoordinate>() {new BoardCoordinate(3,3)},7,1 )));
//
//            NavigationService.Navigate(new Uri(@"/ChallengeGamePage.xaml", UriKind.Relative));

            //MobileServiceClient invertItServiceClient = this.CurrentApp().InvertItServiceClient;

//            var authHandler = new AuthHandler();
//            var invertItServiceClient = new MobileServiceClient(Constants.InvertItServiceAddress, Constants.InvertItServiceAppKey,authHandler);
//
//            var microsoftAuthenticator = new MicrosoftAuthenticator(Constants.MsClientId, invertItServiceClient);
//            var cachingAuthenticator = new FromCacheAuthenticator(microsoftAuthenticator,
//                new MobileServiceAuthorizationCache(
//                    new SecureValueStorer<string, string>(new ApsSettingsValueStorer<string, byte[]>(),input => input )),
//                invertItServiceClient);
//            authHandler.Authenticator = cachingAuthenticator;
//
//            var res = await cachingAuthenticator.Authenticate();
//            MessageBox.Show(res.IsSuccess ? "Success" : "Fail");
//
//            var readAsync = invertItServiceClient.GetTable<TodoItem>().ReadAsync();


            //await Authenticate();
        }

        //private LiveConnectSession session;
        //private static string clientId = Constants.MsClientId;
//        private async System.Threading.Tasks.Task Authenticate()
//        {
//            // Create the authentication client using the client ID of the registration.
//            LiveAuthClient liveIdClient = new LiveAuthClient(clientId);
//
//            while (session == null)
//            {
//                LiveLoginResult result = await liveIdClient.LoginAsync(new[] { "wl.basic" });
//                if (result.Status == LiveConnectSessionStatus.Connected)
//                {
//                    session = result.Session;
//                    LiveConnectClient client = new LiveConnectClient(result.Session);
//                    LiveOperationResult meResult = await client.GetAsync("me");
//                    MobileServiceUser loginResult = await this.CurrentApp().InvertItServiceClient
//                        .LoginWithMicrosoftAccountAsync(result.Session.AuthenticationToken);
//
//                    string title = string.Format("Welcome {0}!", meResult.Result["first_name"]);
//                    var message = string.Format("You are now logged in - {0}", loginResult.UserId);
//                    MessageBox.Show(message, title, MessageBoxButton.OK);                    
//                }
//                else
//                {
//                    session = null;
//                    MessageBox.Show("You must log in.", "Login Required", MessageBoxButton.OK);
//                }
//            }                      
//        }

//        private async System.Threading.Tasks.Task Authenticate()
//        {
//            MobileServiceUser user = null;
//            while (user == null)
//            {
//                string message;
//                try
//                {
//                    user = await this.CurrentApp().InvertItServiceClient
//                        .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
//                    message =
//                        string.Format("You are now logged in - {0}", user.UserId);                    
//                }
//                catch (InvalidOperationException)
//                {
//                    message = "You must log in. Login Required";
//                }
//
//                MessageBox.Show(message);
//               
//            }
//        }
    }
 
}