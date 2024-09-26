namespace GameLogic.WinPointsCalculators
{
    public interface IWinPointsCalculator
    {
        int CalculateFor(Game.Game game);
        int GetMaxPossiblePoints();
    }
}