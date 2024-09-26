using System;
using System.Collections.Generic;
using GameLogic.Areas;
using GameLogic.Board;

namespace GameLogic.WinVerifiers
{
    public class WinVerifierWithDisabledAreas : IWinVerifier
    {
        private readonly IEnumerable<BoardCoordinate> _disabledAreas;
        private readonly IWinVerifier _winVerifier;
        public WinVerifierWithDisabledAreas(IWinVerifier winVerifier, IEnumerable<BoardCoordinate> disabledAreas)
        {
            if (disabledAreas == null) throw new ArgumentNullException("disabledAreas");
            if (winVerifier == null) throw new ArgumentNullException("winVerifier");
            _disabledAreas = disabledAreas;
            _winVerifier = winVerifier;
        }
        public bool IsBoardWinning(AreaMatrix areaMatrix)
        {
            return CreateWinningBoard(areaMatrix.Size).Equals(areaMatrix);
        }
        public AreaMatrix CreateWinningBoard(BoardSize boardSize)
        {
            var winningBoard = _winVerifier.CreateWinningBoard(boardSize);
            foreach (var boardCoordinate in _disabledAreas)
            {
                winningBoard.Areas[boardCoordinate.X, boardCoordinate.Y].AreaState = AreaState.Disabled;
            }
            return winningBoard;
        }
    }
}