using System;
using System.Windows;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame
{
    public partial class HelpPage : BasePage
    {
        private readonly Random _random = new Random();
        private const int ShapeSize = Constants.OverlayShapeSize;
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private void SetupOverlayBackground()
        {
            Overlay.AnimationCreator = new SingleAnimationCreator(AnimationsRepository.CreateGrowPopAnimation(ShapeSize,
                TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(50)));

            Overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(100));
            Overlay.ShapeCreator = CreateShape;
            Overlay.NewShapePosition = _positionRandomizer.RandomizePosition;
        }

        private Shape CreateShape()
        {
            var ellipse = new Ellipse();
            ellipse.Visibility = Visibility.Collapsed;
            ellipse.Width = ellipse.Height = 1; //shapeSize;

            var colors = new[]
            {
                GameResources.Instance.OverlayGradientRed,
                GameResources.Instance.OverlayGradientBlue
            };
            ellipse.Fill = colors[_random.Next(colors.Length)];
            //ellipse.CacheMode = new BitmapCache();
            return ellipse;
        }

        public HelpPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
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