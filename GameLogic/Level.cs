using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Board;

namespace GameLogic
{
    public class Level
    {
        private readonly List<BoardCoordinate> _winningMovesSequention;
        private readonly BoardSize _boardSize;        
        public int Id { get; private set; }

        public Level(List<BoardCoordinate> winningMovesSequention, BoardSize boardSize,int id)
        {
            if (winningMovesSequention == null) throw new ArgumentNullException("winningMovesSequention");
            if (!winningMovesSequention.All(move =>move.IsValidOnBoard(boardSize))) 
                throw new ArgumentException("winningMovesSequention","winningMovesSequention not valid ob boardSize");
            
            _winningMovesSequention = winningMovesSequention;
            _boardSize = boardSize;
            Id = id;
        }

        public ReadOnlyCollection<BoardCoordinate> WinningMovesSequention
        {
            get { return new ReadOnlyCollection<BoardCoordinate>(_winningMovesSequention); }            
        }

        public BoardSize BoardSize
        {
            get { return _boardSize; }
        }
    }
}
