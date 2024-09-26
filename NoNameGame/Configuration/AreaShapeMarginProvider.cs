using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Board;

namespace NoNameGame.Configuration
{
    public class AreaShapeMarginProvider
    {
        private readonly int _boardSize;
        public AreaShapeMarginProvider(int boardSize)
        {
            _boardSize = boardSize;
        }
        public int Get()
        {
            if (_boardSize == 7)
                return 3;
            else
                return 2;
        }
    }
}
