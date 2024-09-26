using System;
using System.Windows;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls.Maps.Overlays;
using Microsoft.WindowsAzure.MobileServices;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame.ChallengePages
{
    public partial class DashboardPage : BasePage
    {
        private readonly Random _random = new Random();
        private const int ShapeSize = Constants.OverlayShapeSize;        
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly ShapeCreator _shapeCreator;
        private ChallengesController _controller;
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
        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetupOverlayBackground();
            ChallengesControl.CurrentChallengesModel = await _controller.GetCurrentChallenges();
        }

        public DashboardPage()
        {
            InitializeComponent();
            Loaded+=OnLoaded;
            _shapeCreator = new ShapeCreator();
            _controller = this.CurrentApp().ChallengesController;

        }
      
    }
}