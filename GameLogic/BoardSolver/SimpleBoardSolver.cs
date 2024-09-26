using System.Collections.Generic;
using System.Linq;
using GameLogic.Board;
using GameLogic.Infrastructure;

namespace GameLogic.BoardSolver
{
    public class SimpleBoardSolver : IBoardSolver
    {
        public BoardCoordinate GetSolvingMovesSequence(Board.GameBoard gameBoard, IEnumerable<BoardCoordinate> playerMoves)
        {
            IList<BoardCoordinate> playerMovesList = playerMoves.ToList().Optimize2();
            var list =  gameBoard.WinningMoves.Except(playerMovesList).ToList();
            if (list.Count == 0)
            {
                var boardCoordinates = playerMovesList.Except(gameBoard.WinningMoves).ToList();
                return boardCoordinates.Last();
            }
            return list.First();
        }
    }
}