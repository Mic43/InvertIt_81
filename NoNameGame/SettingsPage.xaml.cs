using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.Configuration.NewAchievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers;
using NoNameGame.Controllers.Sound;
using NoNameGame.Controllers.Vibrator;
using NoNameGame.CustomControls;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;

namespace NoNameGame
{
    public partial class SettingsPage : BasePage
    {
        private readonly Random _random = new Random();
        private const int ShapeSize = Constants.OverlayShapeSize;
        private bool _canResetProgress = true;
        private readonly PositionRandomizer _positionRandomizer = new PositionRandomizer(Constants.OverlayShapeSize);
        private readonly ShapeCreator _shapeCreator;
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
       
        public SettingsPage()
        {
            InitializeComponent();
            _shapeCreator = new ShapeCreator();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string canResetProgress;
            _canResetProgress = !NavigationContext.QueryString.TryGetValue("CanResetProgress", out canResetProgress) ||
                                bool.Parse(canResetProgress);
            
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            this.CurrentApp()
                .SoundController.SetSound(new SetSoundParams(SettingsControl.SettingsModel.IsSoundOn,
                    SettingsControl.SettingsModel.IsMusicOn,
                    (float) SettingsControl.SettingsModel.SoundVolume/SettingsModel.MaxVolume));
            this.CurrentApp()
                .VibrationController.SetVibration(new VibrationParams(SettingsControl.SettingsModel.IsVibrationOn));
        }
        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var soundParams = this.CurrentApp().SoundController.SoundParams;
            var vibrationParams = this.CurrentApp().VibrationController.VibrationParams;
            SettingsControl.SettingsModel = new SettingsModel
            {
                IsSoundOn = soundParams.IsEnabled,
                IsMusicOn = soundParams.IsMusicEnabled,
                SoundVolume = (int) ((float) soundParams.Volume*SettingsModel.MaxVolume),
                IsVibrationOn = vibrationParams.IsEnabled,
                CanResetProgress = _canResetProgress
            };
            SettingsControl.ResetProgressInvoked += s => this.CurrentApp().ProgressResetter.ResetProgress();

            SetupOverlayBackground();
         
        }
    }
}