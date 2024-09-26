using System;
using System.Linq.Expressions;
using GameLogic.Areas;
using GameLogic.Board;
using Infrastructure;

namespace GameLogic.WinVerifiers
{
   public class AllAreasMustBeChecked : IWinVerifier
    {
       public bool IsBoardWinning(AreaMatrix areaMatrix)
       {
           return CreateWinningBoard(new BoardSize(areaMatrix.Size)).Equals(areaMatrix);
       }

       public AreaMatrix CreateWinningBoard(BoardSize boardSize)
       {
           var areaMatrix = new AreaMatrix(boardSize);
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j <boardSize; j++)
                {
                     areaMatrix.InsertArea(AreaState.Checked, i,j);
                }
            }
           return areaMatrix;
       }
    }
}