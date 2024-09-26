using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AdDealsSDKWP8;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.WinVerifiers;
using ImageTools.IO.Gif;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Popups
{
    public partial class GameTutorialControl : UserControl,ITutorialControl
    {
        private SingleAnimationCreator _fingerTapAnimationCreator;
        public GameTutorialControl()
        {
            InitializeComponent();

            var source = new DataSource() { ImageSource = new Uri("../../Assets/tap3.gif", UriKind.Relative) };
            this.DataContext = source;
          

            Themer.EnableThemesForControls(OkButton);
            _fingerTapAnimationCreator =
                new SingleAnimationCreator(
                    AnimationBuilder.Translate()
                        .Vertical()
                        .From(85)
                        .WithEasingFunction((new QuadraticEase() {EasingMode = EasingMode.EaseInOut}))
                        .To(40)                        
                        .AutoReverse()
                        .RepeatForever()
                        .WithDuration(750)
                        .Build());            
            Loaded += (sender, args) =>
            {
               // _fingerTapAnimationCreator.Create(FngerImage).Begin();
                FadeImageStoryboard.Begin();
                FadeTextStoryboard.Begin();
                Image.Start();
            };            
            CreateBeforeMiniGrid();
            CreateAfterMiniGrid();
        }
        private void CreateBeforeMiniGrid()
        {
            var winningBoard = new AllAreasMustBeChecked().CreateWinningBoard(3);
            winningBoard.Areas[1, 1].OnEnter();
            var boardImage = new BoardGrid(BoardModel.FromBoardGrid(winningBoard), this.CurrentApp().AreaStateTransitionManager,
                new EmptyMultiAnimationCreator(), new EmptyResetAnimationFactory()
                , new EllipseAreaVisualisationFactory(0, 1));
            boardImage.Width = boardImage.Height = 100;
            ImagePanel.Children.Insert(0, boardImage);
            Grid.SetColumn(boardImage, 0);
        }
        private void CreateAfterMiniGrid()
        {
            var winningBoard = new AllAreasMustBeChecked().CreateWinningBoard(3);
            var boardImage = new BoardGrid(BoardModel.FromBoardGrid(winningBoard), this.CurrentApp().AreaStateTransitionManager,
                new EmptyMultiAnimationCreator(), new EmptyResetAnimationFactory()
                , new EllipseAreaVisualisationFactory(0, 1));
            boardImage.Width = boardImage.Height = 100;
            boardImage.Margin = new Thickness(0, 0, 20, 0);
            Grid.SetColumn(boardImage, 2);
            ImagePanel.Children.Add( boardImage);
          
        }
        public Button ClosingButton
        {
            get { return OkButton; }
        }
    }

    public interface ITutorialControl
    {
        Button ClosingButton { get; }
    }

    public class DataSource
    {

        private Uri _imageSource;

        public Uri ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                }
            }
        }
    } 
}