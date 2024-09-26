using GameLogic.Areas;
using GameLogic.Board;
using NoNameGame.Controllers.GameLogic;

namespace NoNameGame.Models
{
    public class AreaModel
    {
        public AreaState AreaState { get; set; }

        public BoardCoordinate Coordinates { get; set; }
    }
}