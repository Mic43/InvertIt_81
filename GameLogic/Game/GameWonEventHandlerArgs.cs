using System;
using System.Windows;
using GameLogic.Board;

namespace GameLogic.Game
{
    public class GameWonData
    {
        public WonGameStats WonGameStats { get; private set; }
        public BoardCoordinate LastPlayerMove { get; private set; }
        public int PlayedLevelId { get; private set; }
      
        public GameWonData(WonGameStats wonGameStats, BoardCoordinate lastPlayerMove,int playedLevelId)
        {
            if (wonGameStats == null) throw new ArgumentNullException("wonGameStats");
        
            WonGameStats = wonGameStats;
            LastPlayerMove = lastPlayerMove;
            PlayedLevelId = playedLevelId;
        }
    }
}