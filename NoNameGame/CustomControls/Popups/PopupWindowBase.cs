using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Infrastructure;
using Microsoft.Xna.Framework.Input;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame.CustomControls.Popups
{
    [TemplatePart(Name = "Overlay", Type = typeof(OverlayAnimatedBackgroundControl))]
    public class PopupWindowBase : ContentControl
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (PopupWindowBase), null);
        private PositionRandomizer _positionRandomizer;
        private const int ShapeSize = 15;
        private ShapeCreator _shapeCreator;
        private OverlayAnimatedBackgroundControl _overlay;

        public PopupWindowBase()
        {          
            DefaultStyleKey = typeof (PopupWindowBase);
            _shapeCreator = new ShapeCreator();     
            Themer.EnableThemesForControls(this);          
            HeaderBackgroundColor = new SolidColorBrush(Helpers.GameAccentColorProvider.GetLighter());
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();            
            //_overlay = (OverlayAnimatedBackgroundControl)GetTemplateChild("Overlay");
            //var layoutRoot = (Grid)GetTemplateChild("LayoutRoot");
            //layoutRoot.Background = new SolidColorBrush(ColorManipulation.LightenColor(GameResources.Instance.CheckedColor, 0.8f));

            //SetupOverlayBackground();
        }    

        public string Header
        {
            get { return base.GetValue(HeaderProperty) as string; }
            set { base.SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty ShowTitleAnimationProperty = DependencyProperty.Register(
            "ShowTitleAnimation", typeof (bool), typeof (PopupWindowBase), new PropertyMetadata(default(bool)));

        public bool ShowTitleAnimation
        {
            get { return (bool) GetValue(ShowTitleAnimationProperty); }
            set { SetValue(ShowTitleAnimationProperty, value); }
        }


        public static readonly DependencyProperty HeaderBackgroundColorProperty = DependencyProperty.Register(
            "HeaderBackgroundColor", typeof(Brush), typeof(PopupWindowBase), new PropertyMetadata(default(Brush)));

        public Brush HeaderBackgroundColor
        {
            get { return (Brush)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundBrushProperty = DependencyProperty.Register(
            "HeaderForegroundBrush", typeof (Brush), typeof (PopupWindowBase), new PropertyMetadata(default(Brush)));

        public Brush HeaderForegroundBrush
        {
            get { return (Brush) GetValue(HeaderForegroundBrushProperty); }
            set { SetValue(HeaderForegroundBrushProperty, value); }
        }
        private void SetupOverlayBackground()
        {
           
            _positionRandomizer = new PositionRandomizer(ShapeSize);
            //_overlay.Height = 100;
            _overlay.AnimationCreator =
                AnimationsRepository.CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(1),
                    TimeSpan.FromMilliseconds(1000), 3);

            _overlay.NewShapeAppearanceTime = new NewShapeAppearanceTime(TimeSpan.FromMilliseconds(50),
                TimeSpan.FromMilliseconds(100));
            _overlay.ShapeCreator = CreateShape;
            _overlay.NewShapePosition = _positionRandomizer.RandomizePosition;
        }
        private Shape CreateShape()
        {
            return _shapeCreator.CreateMainColorsVisibleEllipse(ShapeSize);
        }     
    }
}
