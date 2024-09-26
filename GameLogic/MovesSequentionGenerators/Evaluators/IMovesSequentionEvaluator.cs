using System.Collections.Generic;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.Evaluators
{
    public interface IMovesSequentionEvaluator
    {
        bool Evaluate(ICollection<BoardCoordinate> generatedMoves);
    }
}