using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Board;
using GameLogic.Infrastructure;
using GameLogic.WinVerifiers;

namespace GameLogic.BoardSolver
{
    public interface IBoardSolver
    {
        BoardCoordinate GetSolvingMovesSequence(Board.GameBoard gameBoard,IEnumerable<BoardCoordinate> playerMoves );
    }

    public class BoardSolver : IBoardSolver
    {
        private readonly IWinVerifier _winVerifier;
//        private Board.Board _board;

        public BoardSolver(IWinVerifier winVerifier)
        {
            _winVerifier = winVerifier;
        }

        public BoardCoordinate GetSolvingMovesSequence(Board.GameBoard gameBoard, IEnumerable<BoardCoordinate> playerMoves)
        {
            if (gameBoard == null) throw new ArgumentNullException("gameBoard");

            var winningMoves = gameBoard.WinningMoves.AsEnumerable().Reverse().ToList();
            var winningBoard = _winVerifier.CreateWinningBoard(gameBoard.Size);

            
            var currentState = gameBoard.AreaMatrix.Copy();
            AreaMatrix boardState = winningBoard;
//            for (int i = 0; i < playerMoves.Count; i++)
//            {
//                boardState = winningBoard;

                int j = winningMoves.Count - 1;
                while (j >= 0)
                {
                    boardState.Areas[winningMoves[j].X, winningMoves[j].Y].OnEnter();
                    if (boardState.Equals(currentState))
                        return winningMoves[j];
                    j--;
                }
                //currentState.Areas[playerMoves[i].X, playerMoves[i].Y].OnEnter();
            //}
            return playerMoves.ToList().Optimize().Last();
        }            
    }
}