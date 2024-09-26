using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame
{
    public partial class CreditsPage : BasePage
    {           
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly Random _random = new Random( );
        void SetupOverlayBackground()
        {
//            Overlay.AnimationCreator = new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(Constants.OverlayShapeSize,
//                                                                 TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));
            Overlay.AnimationCreator =
                AnimationsRepository.CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(300),TimeSpan.FromMilliseconds(800), 10);

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

        public CreditsPage()
        {
            InitializeComponent();
            Loaded+=OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FadeTextStoryboard.Begin();
            FadeTextStoryboard2.Begin();
            FadeTextStoryboard3.Begin();
            SetupOverlayBackground();
        }
    }
}