using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.OverlayAnimatedBackground;
using NoNameGame.Helpers;
using NoNameGame.Helpers.OverlayBackground;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls.Popups
{
    public class GameWonControlModel
    {
        public int Points { get; private set; }
        public bool CanStartNextLevel { get;private set; }
        public int MovesCount { get; private set; }
        public bool NewItemUnlocked{ get; private set; }

        public bool ForceDisableAnimation { get; private set; }
        public bool IsPerfect { get; set; }
        public string MovesCountCaption
        {
            get
            {                
                return string.Format("{0} {1} {2}.", AppResources.GameWonControl_MovesDescriptionTextBlock, MovesCount, (MovesCount > 1
                        ? AppResources.GameWonControl_MovesCaptionMany
                        : AppResources.GameWonControlModel_MovesCountCaption_Single));
            } 
        }

        public GameWonControlModel(int points, bool canStartNextLevel, int movesCount, bool newItemUnlocked,bool isPerfect,bool forceDisableAnimation = false)
        {
            Points = points;
            CanStartNextLevel = canStartNextLevel;
            MovesCount = movesCount;
            NewItemUnlocked = newItemUnlocked;
            this.IsPerfect = isPerfect;
            ForceDisableAnimation = forceDisableAnimation;
        }
    }

    public partial class GameWonControl : UserControl
    {
        private readonly SingleAnimationCreator _newItemUnlockedAnimation;
        private SingleAnimationCreator _nextLevelAnimation;

        private GameWonControlModel _gameWonControlModel;        

        public GameWonControlModel GameWonControlModel
        {
            get { return _gameWonControlModel; }
            set
            {
                _gameWonControlModel = value;
                DataContext = value;
            }
        }
     
        public GameWonControl()
        {
            InitializeComponent();
            _newItemUnlockedAnimation =
                new SingleAnimationCreator(
                    (AnimationBuilder.Scale()
                        .Uniform()
                        .WithEasingFunction(new BackEase() {EasingMode = EasingMode.EaseIn})
                        .From(1.7)
                        .To(1.0).WithDuration(1000).Build()));

            new SingleAnimationCreator(AnimationBuilder.Scale().Uniform()
                .WithEasingFunction(new QuadraticEase() { EasingMode = EasingMode.EaseOut }).From(1.05)
                .To(1.2).AutoReverse().RepeatForever().WithDuration(500).Build()).Create(NextLevelButton).Begin();

            Themer.EnableThemesForControls(RestartButto, NextLevelButton, NewItemUnlockedTextBox,ThemesButton,LevelsButton);
            Loaded+=OnLoaded;
            
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        //   _nextLevelAnimation.
        }   
        private void NewItemUnlockedTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!_gameWonControlModel.ForceDisableAnimation)                
                _newItemUnlockedAnimation.Create(NewItemUnlockedTextBox).Begin();                      
        }
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}