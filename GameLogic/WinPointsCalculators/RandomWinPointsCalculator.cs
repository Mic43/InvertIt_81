using System;

namespace GameLogic.WinPointsCalculators
{
    public class RandomWinPointsCalculator : IWinPointsCalculator
    {
        private readonly Random _random = new Random();
        public int CalculateFor(Game.Game game)
        {
            return _random.Next(1000);
        }
        public int GetMaxPossiblePoints()
        {
            return 1000;
        }
    }
}