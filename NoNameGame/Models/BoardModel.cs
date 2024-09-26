using System;
using GameLogic.Board;
using Infrastructure;
using NoNameGame.BoardPresentation;
using NoNameGame.Controllers.GameLogic;

namespace NoNameGame.Models
{
    public class BoardModel
    {
        public AreaModel[,] Areas { get;  set; }

        public int Size { get; set; }

        public static BoardModel FromBoardGrid(AreaMatrix bg)
        {
            var currentBoardModel = new BoardModel()
            {
                Areas = new AreaModel[bg.Size, bg.Size],
                Size = bg.Size
            };
            bg.Areas.ForEach(
                (area, i, j) =>
                {
                    currentBoardModel.Areas[i, j] = new AreaModel
                    {
                        AreaState = area.AreaState,
                        Coordinates =area.BoardCoordinate
                    };
                });
            return currentBoardModel;
        }
    }
}