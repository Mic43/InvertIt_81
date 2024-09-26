using Common;
using GameLogic.Board;

namespace LevelViewer
{
    internal class BoardCooridnateConvetert
    {
        public static SingleMove FromBoardCoordinate(BoardCoordinate bc)
        {
            return new SingleMove(bc.X, bc.Y);
        }
        public static BoardCoordinate FromSingleMove(SingleMove singleMove)
        {
            return new BoardCoordinate(singleMove.X, singleMove.Y);
        }
    }
}