using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.Evaluators
{
    public class SimpleMovesSequentionEvaluator : IMovesSequentionEvaluator
    {
        private readonly Func<BoardCoordinate,BoardCoordinate, double> _movePairHeuristic;
        private readonly Func<IEnumerable<double>, bool> _boardSetupEvalutionFunc;

        public Func<BoardCoordinate,BoardCoordinate,double> MovePairHeuristic
        {
            get { return _movePairHeuristic; }
        }
        public Func<IEnumerable<double>, bool> BoardSetupEvalutionFunc
        {
            get { return _boardSetupEvalutionFunc; }
        }

        public SimpleMovesSequentionEvaluator(Func<BoardCoordinate,BoardCoordinate, double> movePairHeuristic,
            Func<IEnumerable<double>, bool> boardSetupEvalutionFunc)
        {
            if (movePairHeuristic == null) throw new ArgumentNullException("movePairHeuristic");
            if (boardSetupEvalutionFunc == null) throw new ArgumentNullException("boardSetupEvalutionFunc");

            _movePairHeuristic = movePairHeuristic;
            _boardSetupEvalutionFunc = boardSetupEvalutionFunc;
        }


        public bool Evaluate(ICollection<BoardCoordinate> generatedMoves)
        {
            if (generatedMoves == null) throw new ArgumentNullException("generatedMoves");

            if (generatedMoves.Count < 2)
                return true;

            const int lowerIndex = 2;
            var combinations = new Combinations<BoardCoordinate>(generatedMoves.ToList(), lowerIndex);
            var heuristicsCollection = combinations.Select(pair => MovePairHeuristic(pair[0], pair[1]));

            return BoardSetupEvalutionFunc(heuristicsCollection);
        }
    }
}