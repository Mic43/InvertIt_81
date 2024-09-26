using System.Collections.Generic;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.Evaluators
{
    public class AlwaysTrueEvaluator : IMovesSequentionEvaluator
    {
        public bool Evaluate(ICollection<BoardCoordinate> generatedMoves)
        {
            return true;
        }
    }
}