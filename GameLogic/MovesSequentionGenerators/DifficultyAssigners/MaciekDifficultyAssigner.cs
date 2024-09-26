using System.Collections.Generic;
using System.Linq;
using GameLogic.Areas;
using GameLogic.Board;
using GameLogic.WinVerifiers;

namespace GameLogic.MovesSequentionGenerators.DifficultyAssigners
{
    public class MaciekDifficultyAssigner : IDifficultyAssigner
    {
        private readonly BoardSize _boardSize;
        private IWinVerifier _winVerifier;
        public MaciekDifficultyAssigner(BoardSize boardSize, IWinVerifier winVerifier)
        {
            _boardSize = boardSize;
            _winVerifier = winVerifier;
        }

        public double Assign(ICollection<BoardCoordinate> generatedMoves)
        {
            var winningMovesSequention = new List<BoardCoordinate>(generatedMoves);

            int acc = 0;
            while (winningMovesSequention.Count > 0)
            {
                var board = new GameBoard(new Level(winningMovesSequention, _boardSize, 0), _winVerifier);                
                acc+= generatedMoves.Select(move => board.AreaMatrix.GetNeiborghood(move).Count(x => x.AreaState == AreaState.Checked)).Sum();
                winningMovesSequention.RemoveAt(winningMovesSequention.Count-1);
            }
            return acc;
        }
    }
}