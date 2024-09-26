using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.WinVerifiers;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class GameTutorialControlStep2 : UserControl, ITutorialControl
    {
        private SingleAnimationCreator _arroAnimationCreator;
        public GameTutorialControlStep2()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton);
            _arroAnimationCreator =
                new SingleAnimationCreator(
                    AnimationBuilder.Translate()
                        .Vertical()
                        .From(0)
                        .WithEasingFunction((new QuadraticEase() {EasingMode = EasingMode.EaseInOut}))
                        .To(-90)                        
                        .AutoReverse()
                        .RepeatForever()
                        .WithDuration(750)
                        .Build());            
            Loaded += (sender, args) =>
            {
                _arroAnimationCreator.Create(Arrow).Begin();
                FadeArrowStoryboard.Begin();
                FadeTextStoryboard.Begin();
            };         
        }
        public Button ClosingButton
        {
            get { return OkButton; }
        }
    }
}