using System;
using GameLogic.Areas;
using Infrastructure;

namespace GameLogic.Board
{
    public static class BoardHelpers
    {
        public static void ForEachArea(this GameBoard gameBoard,Action<Area> action)
        {
            ForEachArea(gameBoard,(area, i, j) => action(area));            
        }
        public static void ForEachArea(this GameBoard gameBoard, Action<Area,int,int> action)
        {
            gameBoard.AreaMatrix.Areas.ForEach(action);
        }
    }
}