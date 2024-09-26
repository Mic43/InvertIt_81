using System;
using System.Windows;

namespace GameLogic.Game
{
    public class WonGameStats
    {
        public int Points { get; private set; }
        public int PlayerMovesCount { get; private set; }
        public TimeSpan SolveTime { get; private set; }

        public WonGameStats(int points, int playerMovesCount, TimeSpan solveTime)
        {
            Points = points;
            PlayerMovesCount = playerMovesCount;
            SolveTime = solveTime;
        }
    }
}