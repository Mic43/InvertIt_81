using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Combinatorics.Collections;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators
{
    public interface IMovesSequentionGenerator
    {
        Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount );
    }

    public class TestMovesSequentionGenerator : IMovesSequentionGenerator
    {
        private Random _random;
        private List<IList<BoardCoordinate>> _combinations;
        public  List<T> Create2DRange<T>(int sizeX, int sizeY, Func<int, int, T> singleValueCreator)
        {
            List<T> valuess = new List<T>(sizeX * sizeY);
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    valuess.Add(singleValueCreator(i, j));
                }
            }
            return valuess;
        }
        public TestMovesSequentionGenerator()
        {
           
        }

        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {
            //combinations.
            var allValues = Create2DRange(boardSize, boardSize, (i, j) => new BoardCoordinate(i, j));
            var combinations = new Combinations<BoardCoordinate>(allValues, movesCount);
            //return combinations;
            return null;
        }
        public IEnumerable<BoardCoordinate> Generate2(BoardSize boardSize, int movesCount)
        {
            //combinations.
            var allValues = Create2DRange(boardSize, boardSize, (i, j) => new BoardCoordinate(i, j));
            var combinations = new Combinations<BoardCoordinate>(allValues, movesCount);
            return combinations.GetEnumerator().Current;
        }
        
    }


    public class RandomVariationGenerator : IMovesSequentionGenerator
    {
        private readonly IList<BoardCoordinate> _sequenceToVariate;
        // private readonly IMovesSequentionGenerator _sequenceToVariateGenerator;
        private Random _random;

        public RandomVariationGenerator(IList<BoardCoordinate> sequenceToVariate)
        {
            _sequenceToVariate = sequenceToVariate;
            //_sequenceToVariateGenerator = sequenceToVariateGenerator;
        }

        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {            
            var variations = new Variations<BoardCoordinate>(_sequenceToVariate, movesCount);
            List<IList<BoardCoordinate>> variationsSequences = variations.ToList();
            _random = new Random();
            return new Collection<BoardCoordinate>(variationsSequences[_random.Next(variationsSequences.Count)]);
        }
    }
}