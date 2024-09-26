using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infrastructure;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Controllers.Hints;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.CustomControls.Popups.Challenge;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame
{
    public partial class PurchaseHintsPage : BasePage
    {
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly Random _random = new Random();
        private readonly HintsPurchaseController _hintsPurchaseController;
        private PopupWindowService _popupWindowService;

        void SetupOverlayBackground()
        {
            //            Overlay.AnimationCreator = new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(Constants.OverlayShapeSize,
            //                                                                 TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));
            Overlay.AnimationCreator =
                AnimationsRepository.CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(300), TimeSpan.FromMilliseconds(800), 10);

            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(50),
                                                                         TimeSpan.FromMilliseconds(100));
            Overlay.ShapeCreator = CreateShape;
            Overlay.NewShapePosition = _positionRandomizer.RandomizePosition;
        }
        private Shape CreateShape()
        {
            var ellipse = new Ellipse { };
            ellipse.Visibility = Visibility.Visible;
            ellipse.Width = ellipse.Height = 10;//shapeSize;

            var colors = new[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];
            return ellipse;
        }
        public PurchaseHintsPage()
        {
            InitializeComponent();
            Loaded+=OnLoaded;
            _hintsPurchaseController = this.CurrentApp().HintsPurchaseController;
            _popupWindowService = new PopupWindowService(this,new BusyControl());

            PurchaseHintsControl.WhnenClicked += WhnenClicked;
        }
        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Themer.EnableThemesForControls(AdsTextBlock);           
            SetupOverlayBackground();

            _popupWindowService.Show();
            //ProgressBar.Visibility = Visibility.Visible;
            PurchaseHintsControl.Model = await _hintsPurchaseController.GetHintsPack();
            _popupWindowService.Close();
            //ProgressBar.Visibility = Visibility.Collapsed;       
        }
        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            HintsCountTextBlock.Text = _hintsPurchaseController.GetCurrentHintsCount().ToString();                         
        }
        private void WhnenClicked(string productId)
        {
            _hintsPurchaseController.PurchaseHintPack(productId);
        }
        private async void PurchaseHintsControl_OnLoaded(object sender, RoutedEventArgs e)
        {

        }
      
    }
}