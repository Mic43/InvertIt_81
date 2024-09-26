using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator;
using Infrastructure;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.CustomControls.Popups.Challenge;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame.ChallengePages
{
    public partial class LoginPage : BasePage
    {
        private readonly Random _random = new Random();
        private const int ShapeSize = Constants.OverlayShapeSize;        
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly ShapeCreator _shapeCreator;
        private readonly PopupWindowService _popupWindowService;
        private ChallengeLoginController _controller;
        private void SetupOverlayBackground()
        {
            Overlay.AnimationCreator =
                new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(ShapeSize,
                    TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));

            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(300));
            Overlay.ShapeCreator = CreateShape;
            Overlay.NewShapePosition = _positionRandomizer.RandomizePosition;
        }
        private Shape CreateShape()
        {
            return _shapeCreator.CreateMainColorsCollapsedEllipse(1);
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetupOverlayBackground();
        }

        public LoginPage()
        {
            InitializeComponent();
            _shapeCreator = new ShapeCreator();
            _popupWindowService = new PopupWindowService(this, new BusyControl());
            _controller = this.CurrentApp().ChallengeLoginController;
            Loaded+=OnLoaded;
            
        }
        private async Task<T> PrerformBusyAction<T>(Func<Task<T>> action) 
        {
            _popupWindowService.Show();
            var result = await action();           
            _popupWindowService.Close();
            return result;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _popupWindowService.Show();
                var reslt = await _controller.TryRegisterAsync(UserName.Text, PasswordBox.Password);            

                if (reslt.IsSuccess)
                {
                    await _controller.TryLoginAsync(UserName.Text, PasswordBox.Password);
                    _popupWindowService.Close();
                    this.ShowMessageBox("OK", "Sucessfully logged");                

                }
                else
                {
                    _popupWindowService.Close();
                    this.ShowMessageBox("ERROR", reslt.RegistrationResult.ToString());                
                }
//            string message;
//            try
//            {
//
//                var result = await this.CurrentApp().InvertItServiceClient.
//                    InvokeApiAsync<RegistrationRequest, string>("customRegistration",
//                        new RegistrationRequest(UserName.Text, PasswordBox.Password));
//
//               
//
//            }
//            catch (MobileServiceInvalidOperationException ex)
//            {
//                this.ShowMessageBox("ERROR", ex.Message);
//            }                  
//            finally
//            {
//                _popupWindowService.Close();
//            }            
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (!_popupWindowService.IsShown) return;

            e.Cancel = true;
            _popupWindowService.Close();
        }


        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await PrerformBusyAction(async () => await _controller.TryLoginAsync(UserName.Text, PasswordBox.Password));

            if (result)
            {
                this.ShowMessageBox("OK", "Sucessfully logged");
                NavigationService.Navigate(new Uri(@"/ChallengePages/DashboardPage.xaml", UriKind.Relative));
            }
            else
            {
                this.ShowMessageBox("ERROR", "Wrong user name or password");
            }
        }
    }
}