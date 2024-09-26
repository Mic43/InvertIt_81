using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GameLogic.Board;
using GameLogic.BoardSolver;
using GameLogic.Game;
using GameLogic.WinPointsCalculators;
using GameLogic.WinVerifiers;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Controllers.GameLogic.GameWonActions;
using NoNameGame.Models;
using NoNameGame.Models.ChallengeGame;

namespace NoNameGame.Controllers.GameLogic.Challenges
{
    public class ChallangeGameController
    {        
        private readonly IGameWonAction _gameWonAction;
        private readonly IWinPointsCalculator _winPointsCalculator;
        private readonly IWinVerifier _winVerifier;
        private readonly IBoardSolver _solver;
        private readonly IEventsBus _eventsBus;                
        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();

        private Game _game;
        private ChallengeFinishedModel _lastChallengeFinishedModel;
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
            _lastChallengeFinishedModel = new ChallengeFinishedModel(gameWonData.LastPlayerMove);
//            bool wasFirstlySolved = !_levelProgressStorer.Load(gameWonData.PlayedLevelId).IsFinished;
//            //bool wasFirstlySolved = false;
//            _gameWonAction.Execute(gameWonData);
//            _achievementsController.Execute();
//
//             _lastWonGameModel = new GameWonModel(_achievementsController.LastlyExecutedAchievements
//                                                                         .Any(x => x.AchievementType == AchievementType.UnlockTheme),
//                                                  gameWonData.LastPlayerMove);
//             if (_lastWonGameModel.IsThemeUnlocked)
//                _newItemUnlockedStorer.SetNewItemUnlocked();      
//
//             _eventsBus.Publish(new GameWon
//             (
//                   IsGamePerfect(gameWonData.WonGameStats),
//                  gameWonData.PlayedLevelId,
//                  gameWonData.WonGameStats.SolveTime,
//                  wasFirstlySolved
//             ));
        }

        private void SetupGameTimer()
        {
            _gameTimer.Tick += GameTimerOnTick;
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _gameTimer.Start();        
        }
        private void GameTimerOnTick(object sender, EventArgs eventArgs)
        {
            //  _eventsBus.Publish(new GameTimeTick());
        }
        public ChallangeGameController(IGameWonAction gameWonAction,IWinPointsCalculator winPointsCalculator,
            IWinVerifier winVerifier,IBoardSolver solver,
            IEventsBus eventsBus)            
        {        
            if (gameWonAction == null) throw new ArgumentNullException("gameWonAction");
            if (winPointsCalculator == null) throw new ArgumentNullException("winPointsCalculator");
            if (winVerifier == null) throw new ArgumentNullException("winVerifier");
            if (solver == null) throw new ArgumentNullException("solver");
            if (eventsBus == null) throw new ArgumentNullException("eventsBus");                        

            _gameWonAction = gameWonAction;
            _winPointsCalculator = winPointsCalculator;
            _winVerifier = winVerifier;
            _solver = solver;

            _eventsBus = eventsBus;            

            SetupGameTimer();           
        }
        public void StartNewChallenge(Challenge challenge)
        {
            _game = new Game(challenge.Level, _winPointsCalculator, _winVerifier, _solver);
            _game.GameWon += OnGameWon;   
            // _eventsBus.Publish(new NewGameStarted(levelToPlay));         
        }

        public CurrentPlayedChallengeModel GetCurrentChallengeModel()
        {
            ThrowIfGameNotStarted();
            return new CurrentPlayedChallengeModel()
            {
                PlayerMovesCount = Game.PlayerMovesCount ,
                PerfectMovesCount = Game.LevelToPlay.WinningMovesSequention.Count,
                PlayTime = Game.GetGameTime()
            };
        }
        public ChallengeFinishedModel GetChallengeFinishedModel()
        {
            ThrowIfGameNotStarted();
            if (_lastChallengeFinishedModel == null)
                throw new InvalidOperationException("Challenge is not finished yet!");
            return _lastChallengeFinishedModel;
        }

        public IWinVerifier GetCurrentWinVerifier()
        {
            ThrowIfGameNotStarted();
            return Game.WinVerifier;
        }        
//        public void RestoreGame(AppRestoreData gameData)
//        {            
//            _game = Game.Restore(gameData.GameData,
//                GameConfiguration.CreatePointsCalculator(),
//                GameConfiguration.CreateWinVerifier(GetDisabledAreasCoordinates(gameData.GameData.BoardData.LevelId)), 
//                GameConfiguration.CreateSolver());
//            _game.GameWon += OnGameWon;            
//        }
        public bool CanSerialize()
        {
            return Game != null;
        }
//        public AppRestoreData SerializeGame()
//        {
//            ThrowIfGameNotStarted();
//            return new AppRestoreData() {GameData = Game.Serialize(), GameWonModel = _lastWonGameModel};
//        }
                
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
        public AppBarButonsModelChallenge GetCurrentAppBarButonsModel()
        {
            return new AppBarButonsModelChallenge()
            {
                UndoButtonEnabled = Game.CanUndoLastMove(),
                RedoButtonEnabled = Game.CanRedoLastMove(),
                RefreshButtonEnabled = Game.State == GameState.Started,                
                IsMenuEnabled = Game.State == GameState.Started,                
            };
        }

        public void ResetGame()
        {
            ThrowIfGameNotStarted();  
            Game.Reset();
          //  _eventsBus.Publish(new GameReset());
        }
        public void MakeMove(BoardCoordinate boardCoordinateModel)
        {
            ThrowIfGameNotStarted();
            BoardModel boardBeforeMove = BoardModel.FromBoardGrid(Game.GameBoard.AreaMatrix);

            Game.MakeMove(boardCoordinateModel);

         //   _eventsBus.Publish(new MoveMade(boardBeforeMove,BoardModel.FromBoardGrid(Game.GameBoard.AreaMatrix)));
        }
        public void ResumeGame()
        {
            ThrowIfGameNotStarted();
            //Game.Resume();
          //  _eventsBus.Publish(new GameResumed());
        }
        public void PauseGame()
        {
            ThrowIfGameNotStarted();
            //Game.Pause();
          //  _eventsBus.Publish(new GamePaused());
        }
        public void LeaveGame()
        {
         //   _eventsBus.Publish(new GameLeft(Game.LevelToPlay.Id,Game.PlayerMovesCount,Game.GetGameTime()));
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
           // _eventsBus.Publish(new MoveUndone());
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
    }
}