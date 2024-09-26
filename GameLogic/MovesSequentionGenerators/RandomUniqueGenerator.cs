using System;
using System.Collections.ObjectModel;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators
{
    public class RandomUniqueGenerator : IMovesSequentionGenerator
    {
        private readonly Random _random = new Random();
    
        private int GenerateRandomCoordinate(BoardSize boardSize)
        {
            return _random.Next(0, boardSize.Value);
        }

        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {
            var boardCoordinates = new Collection<BoardCoordinate>();
            BoardCoordinate boardCoordinate;
            for (int i=0;i<movesCount;i++)
            {
                do
                {
                    boardCoordinate = new BoardCoordinate(GenerateRandomCoordinate(boardSize),
                        GenerateRandomCoordinate(boardSize));

                } while (boardCoordinates.Contains(boardCoordinate));
                boardCoordinates.Add(boardCoordinate);
            }
            return boardCoordinates;
        }
    }
}