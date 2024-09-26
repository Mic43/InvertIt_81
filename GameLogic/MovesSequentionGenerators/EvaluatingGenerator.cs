using System;
using System.Collections.ObjectModel;
using GameLogic.Board;
using GameLogic.MovesSequentionGenerators.Evaluators;

namespace GameLogic.MovesSequentionGenerators
{
    public class EvaluatingGenerator : IMovesSequentionGenerator
    {
        private readonly IMovesSequentionGenerator _movesGenerator;
        private readonly IMovesSequentionEvaluator _movesSequentionEvaluator;

        public EvaluatingGenerator(IMovesSequentionGenerator movesGenerator,IMovesSequentionEvaluator movesSequentionEvaluator)
        {
            if (movesGenerator == null) throw new ArgumentNullException("movesGenerator");
            if (movesSequentionEvaluator == null) throw new ArgumentNullException("movesSequentionEvaluator");

            _movesGenerator = movesGenerator;
            _movesSequentionEvaluator = movesSequentionEvaluator;           
        }

        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {
            Collection<BoardCoordinate> generatedMoves;
            do
            {
                generatedMoves = _movesGenerator.Generate(boardSize,movesCount);
            } while (!_movesSequentionEvaluator.Evaluate(generatedMoves));
            return generatedMoves;
        }
    }
}