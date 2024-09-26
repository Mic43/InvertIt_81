using Common;
using GameLogic.Board;

namespace LevelGenerator
{
    internal class BoardCooridnateConvetert
    {
        public static SingleMove FromBoardCoordinate(BoardCoordinate z)
        {
            return new SingleMove(z.X,z.Y);
        }
    }
}