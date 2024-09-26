using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using Infrastructure;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.BoardPresentation;
using NoNameGame.BoardPresentation.AreaVisualisation;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Animations.AreaStateTransition;
using NoNameGame.Configuration.Animations.EndGame;
using NoNameGame.Configuration.Animations.NewGame;
using NoNameGame.Configuration.Animations.Reset;
using NoNameGame.Controllers.GameLogic.Challenges;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.Sound;
using NoNameGame.Controllers.Vibrator;
using NoNameGame.CustomControls.ClickSound;
using NoNameGame.CustomControls.Popups.Challenge;
using NoNameGame.Helpers;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Models;
using NoNameGame.Models.ChallengeGame;
using NoNameGame.Resources;

namespace NoNameGame.ChallengePages
{
    public partial class ChallengeGamePage : BasePage
    {       
        private readonly ChallangeGameController _controller;        
        private IPeriodicAnimationFactory _periodicAnimationFactory;
        private NewGameAnimationFactory _newGameAnimationFactory;
        private BoardGrid _boardGrid;
        private DispatcherTimer _gameTime = new DispatcherTimer() {Interval = TimeSpan.FromSeconds(1)};
        private bool _forceReloadOnNextNavigateTo;
        private bool _forceNoStartGameAnimation;
        private BoardGrid _goalPreview;
        private bool _isPopupShown = false;
        private readonly IFullScreenAdDisplayer _inGameFullScreenAdDisplayer;
        
        readonly object _gameWonLocker = new object();

        private ApplicationBarIconButton _undoButton;
        private ApplicationBarIconButton _redoButton;
        private ApplicationBarIconButton _refreshButton;        
        private Storyboard _movesCountStoryboard;
        private Storyboard _infoPanelStoryboard;
        private double HostHeight { get { return this.CurrentApp().Host.Content.ActualHeight; } }

        // Constructor
        public ChallengeGamePage()
        {
            InitializeComponent();
            _controller = this.CurrentApp().ChallengeGameController;

            BuildLocalizedAppBar();
            _periodicAnimationFactory = this.CurrentApp().PeriodicAnimationFactory;
            _newGameAnimationFactory = new NewGameAnimationFactory();            
            _inGameFullScreenAdDisplayer = this.CurrentApp().InGameFullSceenAdDisplayerFactory.Create(ShowEndGamePopup,LayoutRoot);

            SetupGameTimer();

            _forceReloadOnNextNavigateTo = true;
            CreateAnimations();
        }
        private void SetupGameTimer()
        {
            Unloaded += (sender, args) => _gameTime.Stop();
            _gameTime.Tick += GameTimeOnTick;
            _gameTime.Start();
        }
        private void GameTimeOnTick(object sender, EventArgs eventArgs)
        {
            var currentChallengeModel = _controller.GetCurrentChallengeModel();
            Gametime.Text = currentChallengeModel.PlayTime.ToString("g");
        }
        private void CreateAnimations()
        {
            _movesCountStoryboard = new SingleAnimationCreator(
                AnimationBuilder.Scale()
                    .Uniform()
                    .WithEasingFunction(new QuadraticEase())
                    .From(1.0)
                    .To(1.2)
                    .AutoReverse()
                    .WithDuration(150)
                    .Build()).Create(MovesCountPanel);
       
            _infoPanelStoryboard = new MultiAnimationCreator(
                new SingleAnimationCreator(AnimationBuilder.Translate().Vertical()
                    .WithEasingFunction(new QuadraticEase() {EasingMode = EasingMode.EaseInOut})
                    .To(0).WithDuration(300).Build()),
                SteppingAnimationDelayFuncion.CreateUpDown(TimeSpan.FromMilliseconds(100),(int)(1600 * _controller.CurrentLevelBoardSize / 7.0)))
                 .Create(new UIElement[,] {{InfoGrid, AdGrid}});
        }
        public  void BuildLocalizedAppBar()
        {
            ApplicationBar = ThemeManager.CreateApplicationBar();

            _undoButton = new ClickSoundApplicationBarIconButton(new Uri("/Assets/AppBar/back.png", UriKind.Relative))
            {
                Text = AppResources.GamePage_ApplicationBar_Undo              
            };
            _undoButton.Click += UndoMoveBarIconButton_Click;
            ApplicationBar.Buttons.Add(_undoButton);

            _redoButton = new ClickSoundApplicationBarIconButton(new Uri("/Assets/AppBar/next.png", UriKind.Relative))
            {
                Text = AppResources.GamePage_ApplicationBar_Redo
            };
            _redoButton.Click += RedoMoveBarIconButton_Click;
            ApplicationBar.Buttons.Add(_redoButton);

            _refreshButton = new ClickSoundApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative))
            {
                Text = AppResources.GamePage_ApplicationBar_Restart
            };
            _refreshButton.Click += ResetMenuItem_Click;
            ApplicationBar.Buttons.Add(_refreshButton);
                              
            var settingsMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_ApplicationBar_Settings);
            settingsMenuItem.Click += SettinsgMenuItem_OnClick;
            ApplicationBar.MenuItems.Add(settingsMenuItem);
          
        }
                          
        private void CreateMiniGrid()
        {
            MiniGrid.Children.Remove(_goalPreview);
            var winningBoard = _controller.GetCurrentWinVerifier().CreateWinningBoard(_controller.CurrentLevelBoardSize);
            _goalPreview = new BoardGrid(BoardModel.FromBoardGrid(winningBoard), CreateAreaStateTrasitionAnimation(),
                new EmptyMultiAnimationCreator(), new EmptyResetAnimationFactory()
                , new EllipseAreaVisualisationFactory(0,1,false));
            _goalPreview.Width = _goalPreview.Height = 120;
            _goalPreview.Margin = new Thickness(9);
            _goalPreview.IsHitTestVisible = false;
            _goalPreview.VerticalAlignment = VerticalAlignment.Center;
            MiniGrid.Children.Add(_goalPreview);
        }
        private void CreateBoardGrid()
        {
            ContentPanel.Children.Remove(_boardGrid);
            var areaShapeMarginProvider = new AreaShapeMarginProvider(_controller.CurrentLevelBoardSize);

            var areaStateTrasitionManager = CreateAreaStateTrasitionAnimation();
            _boardGrid = new BoardGrid(_controller.GetCurrentBoardModel(),
                                       areaStateTrasitionManager,
                                       CreateNewGameAnimation(), 
                                       new ResetAnimationFactory(areaStateTrasitionManager,
                                           this.CurrentApp().CurrentAnimationDelayProvider,
                                           bc => _controller.GetCurrentBoardModel().Areas[bc.X,bc.Y],
                                          _controller.CurrentLevelBoardSize),
                                       new EllipseAreaVisualisationFactory(InitialBoardGridTranslation, areaShapeMarginProvider.Get()))
            {
                EndGameAnimationFactory = new EndGameAnimationFactory(),
                PeriodicAnimationCreator = _periodicAnimationFactory.Create(_controller.CurrentLevelBoardSize),
                AreaTappedAnimationCreator = new SingleAnimationCreator(
                    AnimationBuilder.Scale()
                        .Uniform()
                        .WithEasingFunction(new QuadraticEase() {EasingMode = EasingMode.EaseInOut})
                        .From(1.0).To(1.2)
                        .AutoReverse().WithDuration(150).Build()),
                //AreaTappedAnimationCreator = AnimationsRepository.
                  //  CreateExplosionAnimationCreator(TimeSpan.FromMilliseconds(150),TimeSpan.FromMilliseconds(300),5.0),
                 
            };

            _boardGrid.Margin = new Thickness(areaShapeMarginProvider.Get());
            _boardGrid.AreaTapped += boardGrid_AreaTapped;
            _boardGrid.AreaTappedAnimationFinished+=BoardGridOnAreaTappedAnimationFinished;            
            _boardGrid.EndGameAnimationFinished += boardGrid_EndGameAnimationFinished;
            _boardGrid.Background = (Brush) Application.Current.Resources["PhoneBackgroundBrush"];
            
            ContentPanel.Children.Add(_boardGrid);

            if (ShowNewGameAnimation)
            {
                SetupInfoPanelInitialPosition();
                _infoPanelStoryboard.Begin();
            }
        }
        private void SetupInfoPanelInitialPosition()
        {            
            InfoGrid.RenderTransform = new TranslateTransform() { Y = -300  };
            AdGrid.RenderTransform = new TranslateTransform() { Y = -300 };
        }
        private double InitialBoardGridTranslation
        {
            get { return ShowNewGameAnimation ?  -HostHeight:0; }
        }

        private void UpdateUI()
        {            
            CreateBoardGrid();
            CreateMiniGrid();
            UpdateAppBarButtonsAvailibility();
            UpdateGamePanel();                     

            PreloadFullScreenAd();            
        }        
        private void PreloadFullScreenAd()
        {            
            _inGameFullScreenAdDisplayer.Preload();
        }        
        private void ShowLeaveChallengePopup()
        {
            var gamePausedControl = new LeaveChallengeControl();
            Dispatcher.BeginInvoke(() =>
            {
                var window = new PopupWindowService(this, gamePausedControl,
                    new UIElementWithTappedAction(gamePausedControl.OkButton, element =>
                    {
                        NavigationService.GoBack();
                        _controller.LeaveGame();
                        
                        _isPopupShown = false;
                    }),
                    new UIElementWithTappedAction(gamePausedControl.CancelButton, element =>
                    {
                        ResumeGame();
                        _isPopupShown = false;
                    }));
                _isPopupShown = true;
                window.Show();
            });
        }
        private void UpdateGamePanel()
        {                      
            var currentLevelDataModel = _controller.GetCurrentChallengeModel();
          
            MovesCounTextBlock.Text = currentLevelDataModel.PlayerMovesCount.ToString();
            PerfectMovesCounTextBlock.Text = currentLevelDataModel.PerfectMovesCount.ToString();
            Gametime.Text = currentLevelDataModel.PlayTime.ToString("g");
        }

        private void UpdateAppBarButtonsAvailibility()
        {
            var currentAppBarButonsModel = _controller.GetCurrentAppBarButonsModel();
            _undoButton.IsEnabled = currentAppBarButonsModel.UndoButtonEnabled;
            _redoButton.IsEnabled = currentAppBarButonsModel.RedoButtonEnabled;            
            _refreshButton.IsEnabled = currentAppBarButonsModel.RefreshButtonEnabled;
            ApplicationBar.IsMenuEnabled = currentAppBarButonsModel.IsMenuEnabled;
        }     

        #region Animations Setup

        private AreaStateTransitionsManager CreateAreaStateTrasitionAnimation()
        {
            return new AreaStateTransitionManagerFactory().Create(this.CurrentApp().ThemeController.CurrentTheme);
        }
        private IMultiUIElementsAnimationCreator CreateNewGameAnimation()
        {
            return _newGameAnimationFactory.CreateAnimationCreator(ShowNewGameAnimation, HostHeight,
                (_controller.CurrentLevelBoardSize));
        }
        private bool ShowNewGameAnimation
        {
            get { return !_controller.IsGameRestored && !_forceNoStartGameAnimation; }
        }        

        #endregion

        #region Main Actions

        private void MakeMove(BoardCoordinate tappedAreaCoord)
        {            
            _controller.MakeMove(tappedAreaCoord);
            
            SoundEffectsPlayer.Current.NewMoveEffect.Play(1.0f, _controller.GetMoveSoundPitch(), 0.0f);

            _boardGrid.Refresh(_controller.GetLastMoveAffectedAreas());
            UpdateAppBarButtonsAvailibility();
            UpdateGamePanel();
            _movesCountStoryboard.Begin();

        }

        private readonly object padlock = new object();
        private void UndoMove()
        {
            lock (padlock)
            {
                if (_controller.CanUndoMove())
                {
                    var lastMovecoord = _controller.UndoMove();
                    PerformUndoRedoOnUI(lastMovecoord);
                }
            }
        }
        private void RedoMove()
        {
            lock (padlock)
            {
                if (_controller.CanRedoMove())
                {
                    var lastMovecoord = _controller.RedoMove();
                    PerformUndoRedoOnUI(lastMovecoord);
                }
            }
        }

        private void PerformUndoRedoOnUI(BoardCoordinate lastMovecoord)
        {
            UpdateAppBarButtonsAvailibility();
            _boardGrid.Refresh(_controller.GetLastMoveAffectedAreas());           
            UpdateGamePanel();
        }

        public void ResumeGame()
        {
           _controller.ResumeGame();
            _boardGrid.ResumeAnimations();
        }

        public void PauseGame()
        {
            _controller.PauseGame();
            _boardGrid.PauseAnimations();
        }

        private void Reset()
        {
            _controller.ResetGame();
            _boardGrid.PerformResetAnimation();
            UpdateAppBarButtonsAvailibility();
            UpdateGamePanel();
        }

        private void OnWinningMoveMade(ChallengeFinishedModel challengeFinishedModel)
        {
            _boardGrid.PerformEndGameAnimation(challengeFinishedModel.PlayerLastMove);            
        }
        private void OnEndGameAnimaitonFinished()
        {
            SoundEffectsPlayer.Current.GameWonEffect.Play();

            UpdateAppBarButtonsAvailibility();
            _isPopupShown = true;
            _inGameFullScreenAdDisplayer.TryShowAsync();
          //  EndGamePopupShow();
        }   
        private void ShowEndGamePopup()
        {
           
            _isPopupShown = true;
            var challengeFinihedControl = CreateChallengeFinishedControl();
            var window = new PopupWindowService(this, challengeFinihedControl,                                                   
              new UIElementWithTappedAction(challengeFinihedControl.OkButton, element =>
                {
                    _isPopupShown = false;
                     NavigationService.GoBack();
                }));            
            window.Show();            
        }     
        #endregion

        #region Event handlers

        private BoardCoordinate _winningBoardCoordinate;

        private void boardGrid_AreaTapped(object sender, BoardCoordinate tappedAreaCoord)
        {
            PhoneVibrator.Current.Vibrate();

            lock (_gameWonLocker)
            {               

                if (!_controller.IsGameFinished)
                    MakeMove(tappedAreaCoord);
                if (_controller.IsGameFinished)
                    _winningBoardCoordinate = tappedAreaCoord;
            }
        }

        bool canEnter = true;        
        private void BoardGridOnAreaTappedAnimationFinished(object sender,BoardCoordinate tappedBoardCoordinate)
        {
            if (tappedBoardCoordinate.Equals(_winningBoardCoordinate) && _controller.IsGameFinished && canEnter)
            {
                canEnter = false;
                OnWinningMoveMade(_controller.GetChallengeFinishedModel());                
            }
        }      
        private ChallengeFinishedControl CreateChallengeFinishedControl()
        {
            var gameWonControl = new ChallengeFinishedControl
            {
                ChallengeFinishedModel = _controller.GetChallengeFinishedModel()
            };
            return gameWonControl;
        }        
        private void boardGrid_EndGameAnimationFinished(object sender, EventArgs e)
        {
            canEnter = true;
            OnEndGameAnimaitonFinished();          
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (_isPopupShown)
            {
                _controller.LeaveGame();
                AdMediatorControl.Disable();
                return;
            }
            PauseGame();         
            ShowLeaveChallengePopup();

            e.Cancel = true;
        }
        private void ResetMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }        
        private void SettinsgMenuItem_OnClick(object sender, EventArgs e)
        {                        
            NavigationService.Navigate(new Uri(@"/SettingsPage.xaml?CanResetProgress=false", UriKind.Relative));
            PauseGame();
        }         
        private void UndoMoveBarIconButton_Click(object sender, EventArgs e)
        {
            UndoMove();
        }
        private void RedoMoveBarIconButton_Click(object sender, EventArgs e)
        {
            RedoMove();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {                        
            base.OnNavigatedTo(e);                       

            if (_forceReloadOnNextNavigateTo)
            {
                UpdateUI();
                if (_controller.IsGameFinished)
                    OnWinningMoveMade(_controller.GetChallengeFinishedModel());    
            }
            UpdateBackgroundColor();

            UpdateFlagValues();

            MusicPlayer.FadeOut(1000);

            SetAdsRemoval();                      
            ResumeGame();
        }
        private void UpdateFlagValues()
        {
            _forceReloadOnNextNavigateTo = false;
            _forceNoStartGameAnimation = false;
        }
        private void UpdateBackgroundColor()
        {
            
            if (_isPopupShown)
                return;
            
            ApplicationBar.ForegroundColor = GameAccentColorProvider.GetDarker();
            var backgroundColor = GameAccentColorProvider.GetLighter();

            ApplicationBar.BackgroundColor = backgroundColor;
            InfoGrid.Background = new SolidColorBrush(backgroundColor);
           // AdGrid.Background = new SolidColorBrush(backgroundColor);
        }
        private void SetAdsRemoval()
        {
            if (this.CurrentApp().AdsRemovalProvider.AreRemoved())
            {
                GameTitleGrid.Visibility = Visibility.Visible;
                AdMediatorControl.Visibility = Visibility.Collapsed;
                AdMediatorControl.Disable();
            }
            else
            {
                GameTitleGrid.Visibility = Visibility.Collapsed;
                AdMediatorControl.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}