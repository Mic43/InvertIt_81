using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GameLogic.Areas;
using GameLogic.Board;
using GameLogic.BoardSolver;
using GameLogic.WinPointsCalculators;
using GameLogic.WinVerifiers;
using Infrastructure;

namespace GameLogic.Game
{
    public class Game
    {
        public delegate void GameWonEventHandler(object sender, GameWonData gameWonData);

        public Level LevelToPlay { get { return GameBoard.Level; }}
        public IWinPointsCalculator PointsCalculator { get; private set; }
        public IWinVerifier WinVerifier { get; private set; }
        public IBoardSolver Solver { get; private set; }
        public Board.GameBoard GameBoard { get; private set; }
        public GameState State { get; private set; }
        public bool IsRestored { get; private set; }
        public int PlayerMovesCount { get { return _movesCollection.Elements.Count; } }
        public int PerfectMovesCount { get { return LevelToPlay.WinningMovesSequention.Count; } }
        
        public event GameWonEventHandler GameWon;
        
        private UndoRedoCollection<BoardCoordinate> _movesCollection = new UndoRedoCollection<BoardCoordinate>();

        private Stopwatch _gameTimeWatch = new Stopwatch();
        private long _restoredGameTimeOffsetInMs =  0;

        private void OnGameWon()
        {
            State = GameState.Won;
            _gameTimeWatch.Stop();
            if(GameWon!=null)
                GameWon(this, new GameWonData(GetWonGameStats(), _movesCollection.Elements.Last(),LevelToPlay.Id));
        }

        public Game(Level levelToPlay, IWinPointsCalculator pointsCalculator,
                    IWinVerifier winVerifier, IBoardSolver solver)
            : this(pointsCalculator, winVerifier, solver)
        {
            if (levelToPlay == null) throw new ArgumentNullException("levelToPlay");

            GameBoard = new Board.GameBoard(levelToPlay, winVerifier);
            _gameTimeWatch.Start();            
        }

        private Game(IWinPointsCalculator pointsCalculator,IWinVerifier winVerifier, IBoardSolver solver)
        {            
            if (pointsCalculator == null) throw new ArgumentNullException("pointsCalculator");            
            if (winVerifier == null) throw new ArgumentNullException("winVerifier");
            if (solver == null) throw new ArgumentNullException("solver");

            PointsCalculator = pointsCalculator;
            WinVerifier = winVerifier;
            Solver = solver;
        }

        public static Game Restore(GameData gameData, IWinPointsCalculator pointsCalculator, IWinVerifier verifier, IBoardSolver solver)
        {
            var game = new Game(pointsCalculator, verifier, solver)
            {
                IsRestored = true,
                GameBoard = GameLogic.Board.GameBoard.Restore(verifier, gameData.BoardData),             
                State = gameData.State,                
                _restoredGameTimeOffsetInMs = gameData.ElapsedGameTimeInMs
            };
            gameData.UndoMoves.Reverse();
            gameData.RedoMoves.Reverse();
            game._movesCollection = new UndoRedoCollection<BoardCoordinate>(gameData.UndoMoves, gameData.RedoMoves);

            game.Resume();

            return game;
        }

        public GameData Serialize()
        {
            _gameTimeWatch.Stop();
            var gameData = new GameData() { BoardData = GameBoard.Serialize()};         
            gameData.State = State;
            gameData.RedoMoves = _movesCollection.RedoStack.ToList();
            gameData.UndoMoves = _movesCollection.UndoStack.ToList();
            gameData.ElapsedGameTimeInMs = (long)GetGameTime().TotalMilliseconds;
            return gameData;
        }

        public void MakeMove(BoardCoordinate targetAreaCoordiante)
        {
            if (State == GameState.Won)
                throw new InvalidOperationException("Game is ended! Cannnot make move");
            if (!targetAreaCoordiante.IsValidOnBoard(GameBoard.Size))
                throw new ArgumentOutOfRangeException("targetAreaCoordiante", targetAreaCoordiante, "coordinate must be less than size of the board");

            GameBoard.OnEnter(targetAreaCoordiante);
           _movesCollection.Add(targetAreaCoordiante);

            if (WinVerifier.IsBoardWinning(GameBoard.AreaMatrix))
                OnGameWon();          
        }

        public BoardCoordinate GetNextMoveHint()
        {

            return Solver.GetSolvingMovesSequence(GameBoard, _movesCollection.Elements);
        }

        public void Reset()
        {
            _movesCollection.Elements.Reverse().ToList().ForEach(MakeMove);            
            _movesCollection.Clear();
            GameBoard.ClearRecordedAffectedAreas();
            _gameTimeWatch.Restart();
        }
        public ReadOnlyCollection<Area> GetLastMoveAffectedAreas()
        {
            var list = new ReadOnlyCollection<Area>(GameBoard.GetRecordedAffectedAreas().Select(GameBoard.GetArea).ToList());
            GameBoard.ClearRecordedAffectedAreas();
            return list;
        }
        public BoardCoordinate UndoLastMove()
        {
            var lastMoveCoord = _movesCollection.Undo();
            GameBoard.OnEnter(lastMoveCoord);           
            return lastMoveCoord;
        }
        public BoardCoordinate RedoMove()
        {            
            var redoMoveCoord = _movesCollection.Redo();         
            GameBoard.OnEnter(redoMoveCoord);
            return redoMoveCoord;
        }
        public bool CanUndoLastMove()
        {            
            return _movesCollection.CanUndo() && State == GameState.Started;
        }
        public bool CanRedoLastMove()
        {
            return  _movesCollection.CanRedo() && State == GameState.Started;
        }
        public TimeSpan GetGameTime()
        {
            return _gameTimeWatch.Elapsed.Add(TimeSpan.FromMilliseconds(_restoredGameTimeOffsetInMs));
        }
        public WonGameStats GetWonGameStats()
        {
            if(State != GameState.Won)
                throw new InvalidOperationException("Game has not been ended yet!");
            return new WonGameStats(PointsCalculator.CalculateFor(this),PlayerMovesCount,_gameTimeWatch.Elapsed.Duration());
        }
        public void Pause()
        {
            _gameTimeWatch.Stop();
        }
        public void Resume()
        {
            _gameTimeWatch.Start();
        }
    }
}
