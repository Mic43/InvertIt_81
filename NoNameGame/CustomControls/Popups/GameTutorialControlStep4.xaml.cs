using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Areas;
using GameLogic.WinVerifiers;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Popups
{
    public partial class GameTutorialControlStep4 : UserControl, ITutorialControl
    {
        private SingleAnimationCreator _fingerTapAnimationCreator;
        public GameTutorialControlStep4()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(OkButton);
            Loaded+=OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FadeTextStoryboard.Begin();
            FadeTextStoryboard2.Begin();
            Shape areaVisualization = new EllipseAreaVisualisationFactory(0,3).CreateAreaVisualization(new AreaModel() {AreaState =AreaState.Disabled });
            areaVisualization.Height = 50;
            areaVisualization.Width = 50;
            DisabledAreaGrid.Children.Add(areaVisualization);
        }
        public Button ClosingButton
        {
            get { return OkButton; }
        }
    }
}