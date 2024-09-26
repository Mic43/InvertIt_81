using System.Linq;
using GameLogic.Board;
using GameLogic.MovesSequentionGenerators;
using GameLogic.WinVerifiers;

namespace GameLogic.BoardSetuper
{
    public interface IBoardSetuper
    {
        AreaMatrix SetupBoard(IWinVerifier verifier, BoardSize size);
    }

    public class MovesSequentionReverseSetuper : IBoardSetuper
    {
        private readonly IMovesSequentionGenerator _movesSequentionGenerator;
        private readonly int _movesCount;

        public MovesSequentionReverseSetuper(IMovesSequentionGenerator movesSequentionGenerator,int movesCount)
        {
            _movesSequentionGenerator = movesSequentionGenerator;
            _movesCount = movesCount;
        }

        public AreaMatrix SetupBoard(IWinVerifier verifier, BoardSize size)
        {
            AreaMatrix board = verifier.CreateWinningBoard(size);
            var moves = _movesSequentionGenerator.Generate(size,_movesCount).ToList();
            moves.AsEnumerable().Reverse().ToList().ForEach(
                boardCoordinate => board.Areas[boardCoordinate.X, boardCoordinate.Y].OnEnter());
            return board;
        }
    }
}