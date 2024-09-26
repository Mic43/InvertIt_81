using System;

namespace NoNameGame.Controllers.PlayerStats
{
    class CurrentPlayerStarsEquals : IPlayerStatsProvider
    {
        private readonly IPlayerStatsProvider _maxPlayerStatsProvider;
        private readonly int _currentPlayerStars;
        public CurrentPlayerStarsEquals(IPlayerStatsProvider maxPlayerStatsProvider,int currentPlayerStars)
        {
            if (maxPlayerStatsProvider == null) throw new ArgumentNullException("maxPlayerStatsProvider");
            _maxPlayerStatsProvider = maxPlayerStatsProvider;

            if (currentPlayerStars < 0 || currentPlayerStars > maxPlayerStatsProvider.GetAllLevelsStarsCount())
                throw new ArgumentOutOfRangeException("currentPlayerStars",currentPlayerStars, "currentPlayerStars must be grater than - and less than value returned by maxPlayerStatsProvider");
            _currentPlayerStars = currentPlayerStars;
        }
        public int GetCurrentPlayerStarsCount()
        {
            return _currentPlayerStars;
        }
        public int GetAllLevelsStarsCount()
        {
            return _maxPlayerStatsProvider.GetAllLevelsStarsCount();
        }
    }
}