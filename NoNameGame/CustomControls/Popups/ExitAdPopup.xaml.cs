using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using GameLogic.WinVerifiers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Popups
{
    public partial class ExitAdPopup : UserControl
    {
        public ExitAdPopup()
        {
            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;
            InitializeComponent();         
            Loaded+=OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {            
//            new GoogleFullScreenAdDisplayer("ca-app-pub-4997101767812389/4623772356",
//                (o, args) => Application.Current.Terminate(),
//                (o, args) => Application.Current.Terminate())
//           .ShowAsync();
        }
    }
}
