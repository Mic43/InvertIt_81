using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.WinVerifiers;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class GameTutorialControlStep3 : UserControl, ITutorialControl
    {
        private SingleAnimationCreator _fingerTapAnimationCreator;
        public GameTutorialControlStep3()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton);
            Loaded+=OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FadeTextStoryboard.Begin();
            FadeTextStoryboard2.Begin();                        
            FadeTextStoryboard3.Begin();
        }
        public Button ClosingButton
        {
            get { return OkButton; }
        }
    }
}