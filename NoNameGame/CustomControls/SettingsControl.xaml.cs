using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Audio;
using NoNameGame.Controllers.Sound;
using NoNameGame.Helpers;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls
{
    public class SettingsModel
    {
        private int _soundVolume;
        public bool IsSoundOn { get; set; }
        public bool IsMusicOn { get; set; }
        public int SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                if ( value < 0 || value > MaxVolume)
                    throw new ArgumentOutOfRangeException("value");
                _soundVolume = value;
            }
        }
        public static readonly int MaxVolume = 10;

        public bool IsVibrationOn { get; set; }
        public bool CanResetProgress { get; set; }
    }

    partial class SettingsControl : UserControl
    {
        private SettingsModel _settingsModel;
        public event ResetProgresInvoked ResetProgressInvoked;

        public SettingsModel SettingsModel
        {
            get { return _settingsModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                isFirstTime = true;
                DataContext = _settingsModel = value;
            }
        }

        public SettingsControl()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(SoundSwitch,MusicSwitch,VolumeSlider,VibrationSwitch,ResetProgressButton);                      

            SettingsModel = new SettingsModel()
            {
                IsSoundOn = true,
                SoundVolume = SettingsModel.MaxVolume,
                IsVibrationOn = true,
                CanResetProgress = true
            };
        }

        bool isFirstTime;
        private void VolumeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (!isFirstTime && (int) e.NewValue != (int) e.OldValue)
            {
                var newVolumeCandidate = (float) ((int) e.NewValue)/10;
                SoundEffect.MasterVolume = newVolumeCandidate;
                MusicPlayer.SetVolume(newVolumeCandidate);
                SoundEffectsPlayer.Current.ClickEffect.Play();

            }
            isFirstTime = false;
        }
        private void ResetProgressButton_OnClick(object sender, RoutedEventArgs e)
        {
            var msg = new CustomMessageBox()
            {
                Style =  Application.Current.Resources["CustomMessageBoxDynamicThemes"] as Style,
                Caption = AppResources.SettingsControl_ResetProgressMessageBox_Caption,
                Message =
                    AppResources.SettingsControl_ResetProgressMessageBox_Mesage,
                LeftButtonContent = AppResources.SettingsControl_ResetProgressMessageBox_ButtonYes,
                RightButtonContent = AppResources.SettingsControl_ResetProgressMessageBox_ButtonNo,
            };
            msg.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        if (ResetProgressInvoked != null)
                        {
                            ResetProgressInvoked.Invoke(sender);
                        }
                        break;
                }
            };
            msg.Show();           
        }
    }

    public delegate void ResetProgresInvoked(object sender);
}
