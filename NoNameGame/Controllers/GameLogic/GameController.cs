using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using GameLogic;
using GameLogic.Board;
using GameLogic.Game;
using GameLogic.WinVerifiers;
using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Configuration;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Controllers.GameLogic.Challenges.Login;
using NoNameGame.Controllers.GameLogic.GameWonActions;
using NoNameGame.Controllers.Hints.HintsCount;
using NoNameGame.Controllers.Unlocks;
using NoNameGame.CustomControls.Popups;
using NoNameGame.Levels;
using NoNameGame.Levels.Entities;
using NoNameGame.Models;



namespace NoNameGame.Controllers.GameLogic
{
    public class GameController
    {
        private readonly ILevelProvider _levelProvider;        
        
        private readonly AchievementsExecutor _achievementsController;
        private readonly IGameWonAction _gameWonAction;
        private readonly NewItemUnlockedStorer _newItemUnlockedStorer;
        private readonly IEventsBus _eventsBus;
        private readonly ILevelProgressStorer _levelProgressStorer;
        private readonly IHintsCountDecreaser _hintsCountDecreaser;
        private readonly IHintsCountProvider _hintsCountProvider;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private GameWonModel _lastWonGameModel;
        private DispatcherTimer _gameTimer = new DispatcherTimer();

        private Game _game;
        protected Game Game
        {
            get { return _game; }
        }
        public bool IsGameFinished
        {
            get { return  _game==null || _game.State == GameState.Won; }
        }

        public bool IsGameRestored
        {
            get { return Game.IsRestored; }
        }
        public BoardSize CurrentLevelBoardSize
        {
            get
            {
                ThrowIfGameNotStarted();
                return _game.LevelToPlay.BoardSize;
            }
        }
        //    public event EventHandler<GameWonModel> GameWon;
        private void ThrowIfGameNotStarted()
        {
            if (Game == null)
                throw new InvalidOperationException("Game is not started. Start it first by using StartNewGame function");
        }
        private bool IsGamePerfect(WonGameStats wonGameStats)
        {
            return wonGameStats.Points == 3;
        }
        private void OnGameWon(object sender, GameWonData gameWonData)
        {
            bool wasFirstlySolved = !_levelProgressStorer.Load(gameWonData.PlayedLevelId).IsFinished;
            //bool wasFirstlySolved = false;
            _gameWonAction.Execute(gameWonData);
            _achievementsController.Execute();

             _lastWonGameModel = new GameWonModel(_achievementsController.LastlyExecutedAchievements
                                                                         .Any(x => x.AchievementType == AchievementType.UnlockTheme),
                                                  gameWonData.LastPlayerMove);
             if (_lastWonGameModel.IsThemeUnlocked)
                _newItemUnlockedStorer.SetNewItemUnlocked();      

             _eventsBus.Publish(new GameWon
             (
                   IsGamePerfect(gameWonData.WonGameStats),
                  gameWonData.PlayedLevelId,
                  gameWonData.WonGameStats.SolveTime,
                  wasFirstlySolved
             ));
        }

        public GameController(ILevelProvider levelProvider, AchievementsExecutor achievementsController,
            IGameWonAction gameWonAction, NewItemUnlockedStorer newItemUnlockedStorer,IEventsBus eventsBus,
            ILevelProgressStorer levelProgressStorer,IHintsCountDecreaser hintsCountDecreaser,IHintsCountProvider hintsCountProvider,
            ICurrentLevelDataProvider currentLevelDataProvider)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (achievementsController == null) throw new ArgumentNullException("achievementsController");
            if (gameWonAction == null) throw new ArgumentNullException("gameWonAction");
            if (newItemUnlockedStorer == null) throw new ArgumentNullException("newItemUnlockedStorer");
            if (eventsBus == null) throw new ArgumentNullException("eventsBus");
            if (levelProgressStorer == null) throw new ArgumentNullException("levelProgressStorer");
            if (hintsCountDecreaser == null) throw new ArgumentNullException("hintsCountDecreaser");
            if (hintsCountProvider == null) throw new ArgumentNullException("hintsCountProvider");
            if (currentLevelDataProvider == null) throw new ArgumentNullException("currentLevelDataProvider");

            _levelProvider = levelProvider;
            _achievementsController = achievementsController;
            _gameWonAction = gameWonAction;
            _newItemUnlockedStorer = newItemUnlockedStorer;
            _eventsBus = eventsBus;
            _levelProgressStorer = levelProgressStorer;
            _hintsCountDecreaser = hintsCountDecreaser;
            _hintsCountProvider = hintsCountProvider;
            _currentLevelDataProvider = currentLevelDataProvider;

            SetupGameTimer();
            SyncNewAchievements();
        }
        private void SetupGameTimer()
        {
            _gameTimer.Tick += GameTimerOnTick;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _gameTimer.Start();        
        }        
        private void SyncNewAchievements()
        {
            // in order to unlock themes that were added in the application update
            _achievementsController.Execute();
        }
        private void GameTimerOnTick(object sender, EventArgs eventArgs)
        {
            _eventsBus.Publish(new GameTimeTick());
        }

        private IEnumerable<BoardCoordinate> GetDisabledAreasCoordinates(int levelId)
        {
            return _levelProvider.GetDisabledAreas(levelId).AreasList.Select(x=>new BoardCoordinate(x.X,x.Y));
        }
        public GameWonModel GetLastWonGameModel()
        {
            if (_lastWonGameModel == null)
                throw new InvalidOperationException("There is no last game! This is the first game played");
            return _lastWonGameModel;
        }
        public CurrentLevelDataModel GetCurrentLevelData()
        {
            ThrowIfGameNotStarted();
            var currentLevelData = _currentLevelDataProvider.Get(Game.LevelToPlay.Id);
            return new CurrentLevelDataModel(currentLevelData.LevelDisplayName,
                currentLevelData.LevelPackName, currentLevelData.LevelGroupName, Game.PlayerMovesCount,
                Game.LevelToPlay.WinningMovesSequention.Count,_levelProgressStorer.Load(Game.LevelToPlay.Id).Stars);
        }
        public bool NextLevelExists()
        {
            return _levelProvider.GetNextLevel(Game.LevelToPlay.Id).HasValue;
        }
        private LevelDataEntity GetNextLevel()
        {
            Maybe<LevelDataEntity> levelDataEntities = _levelProvider.GetNextLevel(Game.LevelToPlay.Id);
            if (!levelDataEntities.HasValue)
                throw new InvalidOperationException("There is not next level. Use NextLevelExists function to check");
            return levelDataEntities.Single();
        }
        public bool CanStartNextLevel()
        {
            return NextLevelExists() && (_levelProvider.GetLevelGroup(_levelProvider.GetLevel(CurrentLevelId).LevelGroupId).AllLevelsInitiallyUnlocked 
                                         || _levelProgressStorer.Load(GetNextLevel().Id).IsAvailable);
            
        }
        public void StartNextLevel()
        {
            ThrowIfGameNotStarted();
            var nextLevel = GetNextLevel();
            
            StartNewGame(nextLevel.Id);
        }
        public IWinVerifier GetCurrentWinVerifier()
        {
            ThrowIfGameNotStarted();
            return Game.WinVerifier;
        }
        public void StartNewGame(int levelId)
        {
            var currentLevel = _levelProvider.GetLevel(levelId);
         
            var levelToPlay =
                    new Level(currentLevel.Moves.Select(move => new BoardCoordinate(move.X, move.Y)).ToList(),
                    new BoardSize(currentLevel.BoardSize),levelId);
        
            _game = new Game(levelToPlay, 
                             GameConfiguration.CreatePointsCalculator(),
                             GameConfiguration.CreateWinVerifier(GetDisabledAreasCoordinates(levelId)),
                             GameConfiguration.CreateSolver());
            _game.GameWon += OnGameWon;   
            _eventsBus.Publish(new NewGameStarted(levelToPlay));         
        }
        public void RestartGame()
        {
            ThrowIfGameNotStarted();
            StartNewGame(_game.LevelToPlay.Id);
        }
        public void RestoreGame(AppRestoreData gameData)
        {            
            _game = Game.Restore(gameData.GameData,
                GameConfiguration.CreatePointsCalculator(),
                GameConfiguration.CreateWinVerifier(GetDisabledAreasCoordinates(gameData.GameData.BoardData.LevelId)), 
                GameConfiguration.CreateSolver());
            _game.GameWon += OnGameWon;
            _lastWonGameModel = gameData.GameWonModel;            
        }
        public bool CanSerialize()
        {
            return Game != null;
        }
        public AppRestoreData SerializeGame()
        {
            ThrowIfGameNotStarted();
            return new AppRestoreData() {GameData = Game.Serialize(), GameWonModel = _lastWonGameModel};
        }
                
        public List<AreaModel> GetLastMoveAffectedAreas()
        {
            ThrowIfGameNotStarted();
            var lastMoveAffectedAreas = Game.GetLastMoveAffectedAreas();
            return lastMoveAffectedAreas.Select(x => new AreaModel()
            {
                AreaState = x.AreaState,
                Coordinates = x.BoardCoordinate
            }).ToList();
        }
        public BoardModel GetCurrentBoardModel()
        {
            ThrowIfGameNotStarted();
            return BoardModel.FromBoardGrid(Game.GameBoard.AreaMatrix);            
        }
        public int CurrentLevelId
        {
            get
            {
                ThrowIfGameNotStarted();
                return Game.LevelToPlay.Id;
            }
        }
        public GameWonControlModel GetGameWonControlModel()
        {
            ThrowIfGameNotStarted();

            var wonGameStats = Game.GetWonGameStats();
            return new GameWonControlModel(wonGameStats.Points,
                NextLevelExists(), 
                wonGameStats.PlayerMovesCount, 
                _lastWonGameModel.IsThemeUnlocked,
                IsGamePerfect(wonGameStats));
        }
        public AppBarButonsModel GetCurrentAppBarButonsModel()
        {
            return new AppBarButonsModel()
            {
                UndoButtonEnabled = Game.CanUndoLastMove(),
                RedoButtonEnabled = Game.CanRedoLastMove(),
                RefreshButtonEnabled = Game.State == GameState.Started,
                UnlocksButtonEbaled = Game.State == GameState.Started,
                IsMenuEnabled = Game.State == GameState.Started,
                NextLevelMenuItemEnabled = CanStartNextLevel(),
                GetHintButtonEnabled = Game.State == GameState.Started
            };
        }

        public void ResetGame()
        {
            ThrowIfGameNotStarted();  
            Game.Reset();
            _eventsBus.Publish(new GameReset());
        }
        public void MakeMove(BoardCoordinate boardCoordinateModel)
        {
            ThrowIfGameNotStarted();
            BoardModel boardBeforeMove = BoardModel.FromBoardGrid(Game.GameBoard.AreaMatrix);

            Game.MakeMove(boardCoordinateModel);

            _eventsBus.Publish(new MoveMade(boardBeforeMove,BoardModel.FromBoardGrid(Game.GameBoard.AreaMatrix)));
        }
        public void ResumeGame()
        {
            ThrowIfGameNotStarted();
            Game.Resume();
            _eventsBus.Publish(new GameResumed());
        }
        public void PauseGame()
        {
            ThrowIfGameNotStarted();
            Game.Pause();
            _eventsBus.Publish(new GamePaused());
        }
        public void LeaveGame()
        {
            _eventsBus.Publish(new GameLeft(Game.LevelToPlay.Id,Game.PlayerMovesCount,Game.GetGameTime()));
        }
        public bool CanUndoMove()
        {
            ThrowIfGameNotStarted();
            return Game.CanUndoLastMove();
        }
        public bool CanRedoMove()
        {
            ThrowIfGameNotStarted();
            return Game.CanRedoLastMove();
        }
        public BoardCoordinate UndoMove()
        {
            ThrowIfGameNotStarted();
            var undoLastMove = Game.UndoLastMove();
            _eventsBus.Publish(new MoveUndone());
            return undoLastMove;
        }
        public BoardCoordinate RedoMove()
        {
            ThrowIfGameNotStarted();
            var boardCoordinate = Game.RedoMove();
            return boardCoordinate;
        }

        public float GetMoveSoundPitch()
        {
            ThrowIfGameNotStarted();
            return Game.GameBoard.GetCompletenessRatio();
        }
        public BoardCoordinate GetNextMoveHint()
        {
            ThrowIfGameNotStarted();            
            if (_hintsCountProvider.Get()  < 1 )
                throw  new InvalidOperationException("No more honts available!");

            _hintsCountDecreaser.Decrement();

            var nextMoveHint = Game.GetNextMoveHint();            
            return new BoardCoordinate() {X = nextMoveHint.X, Y = nextMoveHint.Y};
        }
        public HintsCountModel GetHintsCountModel()
        {
            return new HintsCountModel(_hintsCountProvider.Get());
        }
      
    }
}