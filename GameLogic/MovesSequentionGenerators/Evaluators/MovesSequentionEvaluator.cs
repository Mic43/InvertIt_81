using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.Evaluators
{
    public class MovesSequentionEvaluator : IMovesSequentionEvaluator
    {
        private Func<HashSet<BoardCoordinate>, double> _singleMoveHeuristic;
        private Func<Dictionary<int, IEnumerable<double>>, bool> _boardSetupEvalutionFunc;

        public Func<HashSet<BoardCoordinate>, double> SingleMoveHeuristic
        {
            get { return _singleMoveHeuristic; }
        }
        public Func<Dictionary<int, IEnumerable<double>>, bool> BoardSetupEvalutionFunc
        {
            get { return _boardSetupEvalutionFunc; }
        }

        public MovesSequentionEvaluator(Func<HashSet<BoardCoordinate>, double> singleMoveHeuristic,
            Func<Dictionary<int, IEnumerable<double>>, bool> boardSetupEvalutionFunc)
        {
            _singleMoveHeuristic = singleMoveHeuristic;
            _boardSetupEvalutionFunc = boardSetupEvalutionFunc;
        }

        public bool Evaluate(ICollection<BoardCoordinate> generatedMoves)
        {
            if (generatedMoves == null) throw new ArgumentNullException("generatedMoves");

            if (generatedMoves.Count < 2)
                return true;

            var combinationSet = new Dictionary<int, IEnumerable<double>>();

            for (int lowerIndex = 2;lowerIndex <generatedMoves.Count + 1;lowerIndex++)
            {
                var combinations = new Combinations<BoardCoordinate>(generatedMoves.ToList(), lowerIndex);
                combinationSet.Add(lowerIndex,combinations.Select(set => SingleMoveHeuristic(new HashSet<BoardCoordinate>(set))).ToList());
            }
            bool result = BoardSetupEvalutionFunc(combinationSet);

            // Debug.WriteLineIf(result,combinationsHeuristics.Select(x=>x.ToString()).Aggregate((str,val) => str + " " +val));
            return result;
        }
    }
}