namespace GameLogic.WinPointsCalculators
{
    public class StarsWinPointsCalculator : IWinPointsCalculator
    {
        public int CalculateFor(Game.Game game)
        {
            if (game.PlayerMovesCount <= game.PerfectMovesCount)
                return 3;
            if (game.PlayerMovesCount < 1.5*game.PerfectMovesCount)
                return 2;            

            return 1;
        }
        public int GetMaxPossiblePoints()
        {
            return 3;
        }
    }
}