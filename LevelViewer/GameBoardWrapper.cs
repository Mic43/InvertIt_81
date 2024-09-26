using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Common;
using GameLogic;
using GameLogic.Board;
using GameLogic.Game;
using GameLogic.WinVerifiers;
using LevelsDataBase;

namespace LevelViewer
{
    public class GameBoardWrapper
    {
        private readonly Panel _parent;
        private GameBoard _gameBoard;
        private BoardGrid _boardGrid;
        private List<BoardCoordinate> _movesMade = new List<BoardCoordinate>();
        private int _boardSize;
        public bool IsReadonly { get { return _boardGrid.IsHitTestVisible; } set
        {
           // _boardGrid.IsHitTestVisible = !(value);
        } }

        public Action<List<BoardCoordinate>> OnMovesChanged { get; set; }

        public GameBoardWrapper(Panel parent)
        {
            _parent = parent;
            _boardSize = 7;
           Clear(null);

        }
        public void Clear(DisabledAreas disabledAreas)
        {
            IWinVerifier winVerifier;
            if (disabledAreas == null)
                winVerifier = new AllAreasMustBeChecked();
            else
            {
                winVerifier = new WinVerifierWithDisabledAreas(new AllAreasMustBeChecked(),
                    DisabledAreasToList(disabledAreas));
            }
            _gameBoard = new GameBoard(new Level(Enumerable.Empty<BoardCoordinate>().ToList(), _boardSize, 0),winVerifier);
            _boardGrid = new BoardGrid(_gameBoard);
            _boardGrid.AreaTapped += boardGrid_AreaTapped;
            _parent.Children.Remove(_boardGrid);
            _parent.Children.Add(_boardGrid);
            _boardGrid.Refresh();
            _movesMade.Clear();
            if (OnMovesChanged!=null)
                OnMovesChanged(_movesMade);
        }
        private static List<BoardCoordinate> DisabledAreasToList(DisabledAreas disabledAreas)
        {
            return MovesCollectionFormatter.ParseString(disabledAreas.Coordinates)
                .Select(BoardCooridnateConvetert.FromSingleMove)
                .ToList();
        }
        public void UpdateMoves(LevelData data,DisabledAreas disabledAreas)
        {
            if (disabledAreas == null)
                disabledAreas = new DisabledAreas();

            _parent.Children.Remove(_boardGrid);
            _boardSize = data.BoardSize;
            _gameBoard = new GameBoard(new Level(MovesCollectionFormatter.ParseString(data.Moves).
                Select(BoardCooridnateConvetert.FromSingleMove).ToList(), _boardSize, 0),
                    new WinVerifierWithDisabledAreas(new AllAreasMustBeChecked(),
                        DisabledAreasToList(disabledAreas)));
            _boardGrid = new BoardGrid(_gameBoard);
            _boardGrid.AreaTapped += boardGrid_AreaTapped;

            _parent.Children.Add(_boardGrid);
            _boardGrid.Refresh();
            _movesMade = new List<BoardCoordinate>(MovesCollectionFormatter.ParseString(data.Moves).Select(BoardCooridnateConvetert.FromSingleMove).ToList());
        }
        void boardGrid_AreaTapped(object sender, BoardCoordinate e)
        {
            if (_movesMade.Contains(e))
            {
                _movesMade.Remove(e);
            }
            else
            {
                _movesMade.Add(e);                
            }            
            _gameBoard.OnEnter(e);
            _boardGrid.Refresh();
            if ( OnMovesChanged!= null)
                OnMovesChanged(_movesMade);
        }
        
    }
}