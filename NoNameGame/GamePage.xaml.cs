﻿using System;
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
using NoNameGame.Controllers.GameLogic;
using NoNameGame.Controllers.Hints;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Controllers.Sound;
using NoNameGame.Controllers.Tutorial;
using NoNameGame.Controllers.Vibrator;
using NoNameGame.CustomControls.ClickSound;
using NoNameGame.CustomControls.Popups;
using NoNameGame.Helpers;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Models;
using NoNameGame.Resources;
using GamePausedControl = NoNameGame.CustomControls.Popups.GamePausedControl;


namespace NoNameGame
{
    public partial class GamePage : BasePage
    {       
        private GameController _controller;
        private HintsPurchaseController _hintsPurchaseController;
        private PlayerStatsController _playerStatsController;

        private IPeriodicAnimationFactory _periodicAnimationFactory;
        private NewGameAnimationFactory _newGameAnimationFactory;
        private BoardGrid _boardGrid;
        private DispatcherTimer _unlocksButtonAnimationTimer = new DispatcherTimer();
        private bool _forceReloadOnNextNavigateTo;
        private bool _forceNoStartGameAnimation;
        private BoardGrid _goalPreview;
        private bool _isPopupShown = false;
        private TutorialControlDisplayer _tutorialControlDisplayer;
        private IFullScreenAdDisplayer _inGameFullScreenAdDisplayer;
        
        readonly object _gameWonLocker = new object();

        private ApplicationBarIconButton _undoButton;
        private ApplicationBarIconButton _redoButton;
        private ApplicationBarIconButton _refreshButton;
        private ApplicationBarIconButton _unlocksButton;
        private Storyboard _movesCountStoryboard;
        private Storyboard _infoPanelStoryboard;
        private PopupWindowService _popupService;
        private double HostHeight { get { return this.CurrentApp().Host.Content.ActualHeight; } }

        // Constructor
        public GamePage()
        {
            InitializeComponent();                        
            CreateUnlockButtonAnimationTimer();
            BuildLocalizedAppBar();            

            Unloaded += GamePage_Unloaded;
          
            _controller = this.CurrentApp().GameController;
            _hintsPurchaseController = this.CurrentApp().HintsPurchaseController;
            _playerStatsController = this.CurrentApp().PlayerStatsController;

            _periodicAnimationFactory = this.CurrentApp().PeriodicAnimationFactory;
            _newGameAnimationFactory = new NewGameAnimationFactory();
            _tutorialControlDisplayer = this.CurrentApp().TutorialControlDisplayer;
            _inGameFullScreenAdDisplayer = this.CurrentApp().InGameFullSceenAdDisplayerFactory.Create(ShowEndGamePopup,LayoutRoot);          
            _forceReloadOnNextNavigateTo = true;
            CreateAnimations();
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

            _hintButton =
                new ClickSoundApplicationBarIconButton(new Uri("/Assets/AppBar/questionmark.png", UriKind.Relative))
                {
                    Text = "0"
                };
            _hintButton.Click += HintButtonOnClick;
            ApplicationBar.Buttons.Add(_hintButton);
                        
            _nextLevelMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_ApplicaitonBar_Next_level);
            _nextLevelMenuItem.Click += NextLevelMenuItemOnClick;
            ApplicationBar.MenuItems.Add(_nextLevelMenuItem);

            var achievementsMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_ApplicationBar_Achievements);
            achievementsMenuItem.Click += AchievementsMenuItemOnClick;
            ApplicationBar.MenuItems.Add(achievementsMenuItem);

            var unlocksMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_ApplicationBar_Themes);
            unlocksMenuItem.Click += UnlocksMenuItem_OnClick;
            ApplicationBar.MenuItems.Add(unlocksMenuItem);

            var hintsMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_AppBar_PurchaseHints);
            hintsMenuItem.Click += HintsMenuItemOnClick;
            ApplicationBar.MenuItems.Add(hintsMenuItem);


            var settingsMenuItem = new ClickSoundApplicationBarMenuItem(AppResources.GamePage_ApplicationBar_Settings);
            settingsMenuItem.Click += SettinsgMenuItem_OnClick;
            ApplicationBar.MenuItems.Add(settingsMenuItem);
          

        }
      
        private void RefreshHintsCountButton()
        {
            //
            var currentHintsCount = _controller.GetHintsCountModel();
            _hintButton.Text = currentHintsCount.HintsAvailable ?
                                   string.Format("{0}: {1}", AppResources.GamePage_RefreshHintsCountButton_CurrentHints, currentHintsCount.CurrentHintsCount) 
                                 : AppResources.GamePage_RefreshHintsCountButton_MoreHints;
        }

        void GamePage_Unloaded(object sender, RoutedEventArgs e)
        {
           // _unlocksButtonAnimationTimer.Stop();
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
            _boardGrid.NewGameAnimationFinished += boardGrid_NewGameAnimationFinished;
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
            //TODO: Should nto be here  - move to controller class                         
            CreateBoardGrid();
            CreateMiniGrid();
            UpdateAppBarButtonsAvailibility();
            UpdateGamePanel();
            UpdatePlayerStarsTextBlock();            

            PreloadFullScreenAd();
            RefreshHintsCountButton();
        }
        private void UpdatePlayerStarsTextBlock()
        {
            StarsCounTextBlock.Text = _playerStatsController.GetStarsProgress().CurrentStarsCount.ToString();
        }
        private void PreloadFullScreenAd()
        {            
            _inGameFullScreenAdDisplayer.Preload();
        }
        public void AnimateHintsButton()
        {
            ApplicationBar.Buttons.Remove(_hintButton);
            ApplicationBar.Buttons.Add(_hintButton);
            //_unlocksButtonAnimationTimer.Start();
//            if (this.CurrentApp().NewItemUnlockedStorer.IsNewItemUnlocked())
//                _unlocksButtonAnimationTimer.Start();
//            else
//                _unlocksButtonAnimationTimer.Stop();
        }
        private void ShowPausedGamePopup()
        {
            var gamePausedControl = new GamePausedControl();
            Dispatcher.BeginInvoke(() =>
            {
                _popupService = new PopupWindowService(this, gamePausedControl,
                    new UIElementWithTappedAction(gamePausedControl.GoToMenuButton, element =>
                    {
                        //this.CurrentApp().Stopwatch.Start();
                        NavigationService.GoBack();
                        _controller.LeaveGame();
                        //GoToMainMenu();
                        _isPopupShown = false;
                    }),
                    new UIElementWithTappedAction(gamePausedControl.ResumeButton, element =>
                    {
                        ResumeGame();
                        _isPopupShown = false;
                    }));
                _isPopupShown = true;
                _popupService.Show();
            });
        }
        private void UpdateGamePanel()
        {                      
            var currentLevelDataModel = _controller.GetCurrentLevelData();

            var levelDisplayName = currentLevelDataModel.LevelDisplayName;
            if (levelDisplayName!=LevelTextBoxTextBlock.Text) 
                LevelTextBoxTextBlock.Text = levelDisplayName;

            var levelPackDislayName = currentLevelDataModel.LevelPackDislayName;
            if (levelPackDislayName != LevelPackTextBlock.Text) 
                LevelPackTextBlock.Text = levelPackDislayName;

            var levelGroupDisplayName = currentLevelDataModel.LevelGroupDisplayName;
            if (levelGroupDisplayName != LevelGroupTextBlock.Text) 
                LevelGroupTextBlock.Text = levelGroupDisplayName;

            MovesCounTextBlock.Text = currentLevelDataModel.PlayerMovesCount.ToString();
            PerfectMovesCounTextBlock.Text = currentLevelDataModel.PerfectMovesCount.ToString();
            StarsForThisLevel.StarsCount = currentLevelDataModel.LevelStars;           
        }

        private void UpdateAppBarButtonsAvailibility()
        {
            var currentAppBarButonsModel = _controller.GetCurrentAppBarButonsModel();
            _undoButton.IsEnabled = currentAppBarButonsModel.UndoButtonEnabled;
            _redoButton.IsEnabled = currentAppBarButonsModel.RedoButtonEnabled;
            _hintButton.IsEnabled = currentAppBarButonsModel.GetHintButtonEnabled;
            _refreshButton.IsEnabled = currentAppBarButonsModel.RefreshButtonEnabled;
            //_unlocksButton.IsEnabled = currentAppBarButonsModel.UnlocksButtonEbaled;
            _nextLevelMenuItem.IsEnabled = currentAppBarButonsModel.NextLevelMenuItemEnabled;
            ApplicationBar.IsMenuEnabled = currentAppBarButonsModel.IsMenuEnabled;

          //  ApplicationBar.ForegroundColor = Constants.AppBarForegroundColor;          
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
           // _boardGrid.PerformAreaTappedAnimation(lastMovecoord.X, lastMovecoord.Y);
            UpdateGamePanel();
        }

        public void ResumeGame()
        {
           _controller.ResumeGame();
            _boardGrid.ResumeAnimations();
            _infoPanelStoryboard.Resume();
        }

        public void PauseGame()
        {
            _controller.PauseGame();
            _boardGrid.PauseAnimations();
            _infoPanelStoryboard.Pause();
        }

        private void Reset()
        {
            _controller.ResetGame();
            _boardGrid.PerformResetAnimation();
            UpdateAppBarButtonsAvailibility();
            UpdateGamePanel();
        }
   
        private void OnWinningMoveMade(GameWonModel gameWonData)
        {                     
            _boardGrid.PerformEndGameAnimation(gameWonData.PlayerLastMove);            
        }
        private void OnEndGameAnimaitonFinished(bool silent)
        {
            if (silent)
            {
                ShowEndGamePopup();
                return;
            }

            SoundEffectsPlayer.Current.GameWonEffect.Play();
            UpdateAppBarButtonsAvailibility();
            _isPopupShown = true;
            _inGameFullScreenAdDisplayer.TryShowAsync();
          //  EndGamePopupShow();
        }   
        private void ShowEndGamePopup()
        {
           
            _isPopupShown = true;
            var gameWonControl = CreateWonGameControl();
            _popupService = new PopupWindowService(this, gameWonControl,
                new UIElementWithTappedAction(gameWonControl.RestartButto, element =>
                {
                    _isPopupShown = false;
                    _controller.RestartGame();
                    UpdateUI();
                }),
                new UIElementWithTappedAction(gameWonControl.NextLevelButton, element =>
                {
                    _isPopupShown = false;
                    _controller.StartNextLevel();                    
                    UpdateUI();
                }),             
                new UIElementWithTappedAction(gameWonControl.ThemesButton,            
                    el => GoToThemesPage()),
                new UIElementWithTappedAction(gameWonControl.LevelsButton, element =>
                {
                    _isPopupShown = false;
                    NavigationService.GoBack();
                }));            
            _popupService.Show();            
        }
        private void GetHint()
        {
            if (_boardGrid.IsAnyShapeMarked())
                return;
            if (_controller.GetHintsCountModel().HintsAvailable)
            {
                _boardGrid.Mark(_controller.GetNextMoveHint());
            }
            else
            {
                NavigationService.Navigate(new Uri(@"/PurchaseHintsPage.xaml", UriKind.Relative));
                //_hintsPurchaseController.PurchaseHintPack("HintPack_10");
            }
            RefreshHintsCountButton();
        }
        private void GoToThemesPage()
        {
            _forceReloadOnNextNavigateTo = true;
            _forceNoStartGameAnimation = true;
            NavigationService.Navigate(new Uri(@"/ThemesPage.xaml", UriKind.Relative));
            PauseGame();
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
        private ClickSoundApplicationBarMenuItem _nextLevelMenuItem;
        private ClickSoundApplicationBarIconButton _hintButton;
        private void BoardGridOnAreaTappedAnimationFinished(object sender,BoardCoordinate tappedBoardCoordinate)
        {
            if (tappedBoardCoordinate.Equals(_winningBoardCoordinate) && _controller.IsGameFinished && canEnter)
            {
                canEnter = false;
                OnWinningMoveMade(_controller.GetLastWonGameModel());                
            }
        }
        private void TryShowTutorial()
        {
            if (_controller.IsGameFinished)
                return;
            if (_tutorialControlDisplayer.TryShowForLevel(_controller.CurrentLevelId, this,(el) =>  _isPopupShown = false))
                _isPopupShown = true;

        }
        private GameWonControl  CreateWonGameControl()
        {            
            var gameWonControl = new GameWonControl
            {
                GameWonControlModel = _controller.GetGameWonControlModel()
            };
            return gameWonControl;
        }
        void boardGrid_NewGameAnimationFinished(object sender, EventArgs e)
        {
            TryShowTutorial();            
           // _infoPanelStoryboard.Begin();            
        }
        private void boardGrid_EndGameAnimationFinished(object sender, EventArgs e)
        {
            canEnter = true;
            OnEndGameAnimaitonFinished(false);          
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
            ShowPausedGamePopup();

            e.Cancel = true;
        }
        private void ResetMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void AchievementsMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            //SoundEffectsPlayer.Current.ClickEffect.Play();
            NavigationService.Navigate(new Uri(@"/AchievementsPage.xaml", UriKind.Relative));
            PauseGame();
        }
        private void SettinsgMenuItem_OnClick(object sender, EventArgs e)
        {            
            //SoundEffectsPlayer.Current.ClickEffect.Play();
            NavigationService.Navigate(new Uri(@"/SettingsPage.xaml?CanResetProgress=false", UriKind.Relative));
            PauseGame();
        }
        private void NextLevelMenuItemOnClick(object sender, EventArgs eventArgs)
        {            
            _controller.StartNextLevel();
            UpdateUI();
        }
        private void HintsMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            NavigationService.Navigate(new Uri(@"/PurchaseHintsPage.xaml", UriKind.Relative));
            RefreshHintsCountButton();
        }
        private void HintButtonOnClick(object sender, EventArgs e)
        {
            GetHint();
        }
        private void CreateUnlockButtonAnimationTimer()
        {
            _unlocksButtonAnimationTimer = new DispatcherTimer() {Interval = TimeSpan.FromMilliseconds(2000)};
            _unlocksButtonAnimationTimer.Tick += (o, args) =>
            {               
                ApplicationBar.Buttons.Remove(_unlocksButton);
                ApplicationBar.Buttons.Add(_unlocksButton);
            };
        }
        private void UnlocksMenuItem_OnClick(object sender, EventArgs e)
        {
            GoToThemesPage();
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
                    OnEndGameAnimaitonFinished(true);
            }
            UpdateBackgroundColor();

            UpdateFlagValues();

            MusicPlayer.FadeOut(1000);

            SetAdsRemoval();          
            RefreshHintsCountButton();

            ResumeGame();
        }
        private void UpdateFlagValues()
        {
            _forceReloadOnNextNavigateTo = false;
            _forceNoStartGameAnimation = false;
        }
        private void UpdateBackgroundColor()
        {
                     
            var backgroundColor = GameAccentColorProvider.GetLighter();
            InfoGrid.Background = new SolidColorBrush(backgroundColor);

//            if (_isPopupShown)
//                return;

            ApplicationBar.BackgroundColor = backgroundColor;
            ApplicationBar.ForegroundColor = GameAccentColorProvider.GetDarker();
            if (_popupService != null && _isPopupShown)
                _popupService.DisableApplicationBar();
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