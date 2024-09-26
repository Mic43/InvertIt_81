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

namespace GameLogic
{
    public class Game
    {
        public Level LevelToPlay { get { return Board.Level; }}
        public IWinPointsCalculator PointsCalculator { get; private set; }
        public IWinVerifier WinVerifier { get; private set; }
        public IBoardSolver Solver { get; private set; }
        public Board.Board Board { get; private set; }
        public GameState State { get; private set; }
        public bool IsRestored { get; private set; }
        public int PlayerMovesCount { get { return _movesCollection.Elements.Count; } }

        public int PerfectMovesCount { get { return LevelToPlay.WinningMovesSequention.Count; } }

        private UndoRedoCollection<BoardCoordinate> _movesCollection = new UndoRedoCollection<BoardCoordinate>();

        private Stopwatch _gameTimeWatch = new Stopwatch();
        private long _restoredGameTimeOffsetInMs =  0;

        private void OnGameWon()
        {
            State = GameState.Won;
            _gameTimeWatch.Stop();
        }

        public Game(Level levelToPlay, IWinPointsCalculator pointsCalculator,
                    IWinVerifier winVerifier, IBoardSolver solver)
            : this(pointsCalculator, winVerifier, solver)
        {
            if (levelToPlay == null) throw new ArgumentNullException("levelToPlay");

            Board = new Board.Board(levelToPlay, winVerifier);
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

        public static Game Restore(IWinPointsCalculator pointsCalculator, IWinVerifier verifier, IBoardSolver solver, GameData gameData)
        {
            var game = new Game(pointsCalculator, verifier, solver)
            {
                IsRestored = true,
                Board = GameLogic.Board.Board.Restore(verifier, gameData.BoardData),             
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
            var gameData = new GameData() { BoardData = Board.Serialize()};         
            gameData.State = State;
            gameData.RedoMoves = _movesCollection.RedoStack.ToList();
            gameData.UndoMoves = _movesCollection.UndoStack.ToList();
            gameData.ElapsedGameTimeInMs = (long)GetGameTime().TotalMilliseconds;
            return gameData;
        }

        public bool MakeMove(BoardCoordinate targetAreaCoordiante)
        {
            if (State == GameState.Won)
                throw new InvalidOperationException("Game is ended! Cannnot make move");
            if (!targetAreaCoordiante.IsValidOnBoard(Board.Size))
                throw new ArgumentOutOfRangeException("targetAreaCoordiante", targetAreaCoordiante, "coordinate must be less than size of the board");

            Board.OnEnter(targetAreaCoordiante);
           _movesCollection.Add(targetAreaCoordiante);

            if (WinVerifier.IsBoardWinning(Board.AreaMatrix))
                OnGameWon();
            return State == GameState.Won;
        }

        public BoardCoordinate GetNextMoveHint()
        {
           // Solver = new BoardSolver.BoardSolver(WinVerifier);
            return Solver.GetSolvingMovesSequence(Board, _movesCollection.Elements);
        }

        public void Restart()
        {
            _movesCollection.Elements.Reverse().ToList().ForEach(coord => MakeMove(coord));            
            _movesCollection.Clear();
            Board.ClearRecordedAffectedAreas();
            _gameTimeWatch.Restart();
        }
        public ReadOnlyCollection<Area> GetLastMoveAffectedAreas()
        {
            var list = new ReadOnlyCollection<Area>(Board.GetRecordedAffectedAreas().Select(Board.GetArea).ToList());
            Board.ClearRecordedAffectedAreas();
            return list;
        }
        public BoardCoordinate UndoLastMove()
        {
            var lastMoveCoord = _movesCollection.Undo();
            Board.OnEnter(lastMoveCoord);           
            return lastMoveCoord;
        }
        public BoardCoordinate RedoMove()
        {            
            var redoMoveCoord = _movesCollection.Redo();         
            Board.OnEnter(redoMoveCoord);
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
            return new WonGameStats() {Points = PointsCalculator.CalculateFor(this),PlayerMovesCount = PlayerMovesCount};
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
