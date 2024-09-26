using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.ClickSound;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class RateAppControl : UserControl
    {        
        public RateAppControl()
        {            
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton,CancelButton);

//            GoToMenuButton.Foreground = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            ResumeButton.Foreground = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            ResumeButton.BorderBrush = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//            GoToMenuButton.BorderBrush = new SolidColorBrush(GameAccentColorProvider.GetDarker());
//
//            GoToMenuButton.Background = new SolidColorBrush(GameAccentColorProvider.GetLighter());
//            ResumeButton.Background = new SolidColorBrush(GameAccentColorProvider.GetLighter());
        }     
    }
}
