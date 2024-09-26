namespace GameLogic.WinPointsCalculators
{
    public class ZeroWinPointsCalculator : IWinPointsCalculator
    {
        public int CalculateFor(Game.Game game)
        {
            return 0;
        }
        public int GetMaxPossiblePoints()
        {
            return 0;
        }
    }
}