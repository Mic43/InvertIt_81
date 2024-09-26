using System.Collections.Generic;
using System.Linq;
using GameLogic.Board;
using Infrastructure;

namespace GameLogic
{
    public class NeiborghoodFinder
    {
        private readonly BoardCoordinate[,] _coordinates;

        public NeiborghoodFinder(int boardSize)
        {
            _coordinates = new BoardCoordinate[boardSize, boardSize];
            for (int i = 0; i < _coordinates.GetLength(0); i++)
            {
                for (int j = 0; j < _coordinates.GetLength(1); j++)
                {
                    _coordinates[i, j] = new BoardCoordinate(i, j);
                }
            }
        }

        public List<BoardCoordinate> GetNeiborghood(BoardCoordinate coordinate)
        {
            return _coordinates.GetNeiborghood(coordinate.X, coordinate.Y).ToList();
        }
    }
}