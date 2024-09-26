using System;
using System.Linq;
using GameLogic.WinPointsCalculators;
using NoNameGame.Levels;


namespace NoNameGame.Controllers.PlayerStats
{
    public class CalculatedPlayerStats : IPlayerStatsProvider
    {
        private readonly ILevelProgressStorer _lp;
        private readonly ILevelProvider _levelProvider;
        private readonly IWinPointsCalculator _pointsCalculator;
        public CalculatedPlayerStats(ILevelProgressStorer lp, ILevelProvider levelProvider, IWinPointsCalculator pointsCalculator)
        {
            if (lp == null) throw new ArgumentNullException("lp");
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (pointsCalculator == null) throw new ArgumentNullException("pointsCalculator");
            _lp = lp;
            _levelProvider = levelProvider;
            _pointsCalculator = pointsCalculator;
        }
        public int GetCurrentPlayerStarsCount()
        {            
            return _levelProvider.GetAllLevels().Sum(x => _lp.Load(x.Id).Stars);
        }
        public int GetAllLevelsStarsCount()
        {
            return _levelProvider.GetAllLevels().Count() * _pointsCalculator.GetMaxPossiblePoints();
        }

    }
}