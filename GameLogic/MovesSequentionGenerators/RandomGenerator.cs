using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators
{
    public class RandomGenerator : IMovesSequentionGenerator
    {        
        readonly Random _randomizer = new Random();
        private int GetRandomCoordinate(BoardSize board)
        {
            return _randomizer.Next(0, board.Value);
        }

        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {
            var generatedMoves = new List<BoardCoordinate>();
            for (int i = 0; i < movesCount; i++)
            {
                generatedMoves.Add(new BoardCoordinate(GetRandomCoordinate(boardSize),GetRandomCoordinate(boardSize)));                
            }
            return new Collection<BoardCoordinate>(generatedMoves);
        }
    }
}