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
using NoNameGame.Controllers.GameLogic;
using NoNameGame.Controllers.Sound;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Popups
{
    public partial class SplashScreen : UserControl
    {
        private Storyboard _storyboard;
        private Storyboard _storyboard2;

        public event EventHandler AnimationFinished;
        protected virtual void OnAnimationfinished()
        {

            var handler = AnimationFinished;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        public SplashScreen()
        {
            InitializeComponent();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;

          //  var gameLoadingEffect = SoundEffectsPlayer.Current.GameLoadingEffect.CreateInstance();           

            var x= new AllAreasMustBeChecked().CreateWinningBoard(7);
            var boardGrid = new BoardGrid(BoardModel.FromBoardGrid(x), new EmptyAreaStateTransitionsManager(), new EmptyMultiAnimationCreator(),
               new EmptyResetAnimationFactory(), new EllipseAreaVisualisationFactory(0,2.5f));           
            boardGrid.Width = boardGrid.Height = 200;
            LayoutRoot.Children.Add(boardGrid);
            int animTime = 400;

            var delay = SteppingAnimationDelayFuncion.CreateArc(TimeSpan.FromMilliseconds(150), new Point(3, 3));
            delay.SetInitialDelay(TimeSpan.FromMilliseconds(1000));

             var anim1 =  AnimationsRepository.CreateFillColorDiscreteAnimation(GameResources.Instance.UnCheckedColor,
                 TimeSpan.FromMilliseconds(animTime/2));
//             var anim2 = AnimationsRepository.CreateFillColorDiscreteAnimation(GameResources.Instance.CheckedColor,
//                 TimeSpan.FromMilliseconds(animTime/2));

             var simultanousAnimationsCreator = new SimultanousAnimationsCreator(
                             new SingleAnimationCreator(
                                 AnimationsRepository.CreateRotationProjectionAnimation(
                                     TimeSpan.FromMilliseconds(animTime))),
                             new SingleAnimationCreator(anim1));
            _storyboard =
                new GenericMultiAnimationCreator<BoardCoordinate>(shape => boardGrid.GetBoardCoordinate((Shape) shape),
                     bc => delay.ComputeDelay(bc.X, bc.Y),
                     bc => simultanousAnimationsCreator).Create(
                                        boardGrid.AreaVisualizations.OfType<UIElement>());
//            _storyboard2 =
//               new GenericMultiAnimationCreator<BoardCoordinate>(shape => boardGrid.GetBoardCoordinate((Shape)shape),
//                   bc =>
//                   {
//                      // var delay = SteppingAnimationDelayFuncion.CreateArc(TimeSpan.FromMilliseconds(100), new Point(3, 3));
//                     //  delay.SetInitialDelay(TimeSpan.FromMilliseconds(2000));
//                       return delay.ComputeDelay(bc.X, bc.Y);
//                   },
//                   bc =>
//                       new SimultanousAnimationsCreator(
//                           new SingleAnimationCreator(
//                               AnimationsRepository.CreateRotationProjectionAnimation(
//                                   TimeSpan.FromMilliseconds(animTime))),
//                            new SingleAnimationCreator(anim2))
//                                   ).Create(
//                                       boardGrid.AreaVisualizations.OfType<UIElement>());
//                            
          

            Loaded += (sender, args) =>
            {
                //gameLoadingEffect.Play()               
                _storyboard.Begin();                
                _storyboard.Completed += (o, eventArgs) =>
                {
                  //  gameLoadingEffect.Stop();
                    OnAnimationfinished();
                };
//                var dispatcherTimer = new DispatcherTimer();
//                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(800);
//                dispatcherTimer.Tick += (sender2, args2) =>
//                {
//                    _storyboard2.Begin();
//                    dispatcherTimer.Stop();
//                };
//                dispatcherTimer.Start();
                // _storyboard2.Begin();

            };
        
            //dispatcherTimer.Start();
        }
    }
}
