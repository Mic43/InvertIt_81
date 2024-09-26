using System;

namespace NoNameGame.Controllers.PlayerStats
{
    public class AllFinishedPlayerStatsProvider : IPlayerStatsProvider
    {
        private readonly IPlayerStatsProvider _playerStatsProvider;        
        public AllFinishedPlayerStatsProvider(IPlayerStatsProvider playerStatsProvider)
        {
            if (playerStatsProvider == null) throw new ArgumentNullException("playerStatsProvider");
            _playerStatsProvider = playerStatsProvider;            
        }
        public int GetCurrentPlayerStarsCount()
        {
            return GetAllLevelsStarsCount();
        }
        public int GetAllLevelsStarsCount()
        {
            return _playerStatsProvider.GetAllLevelsStarsCount();
        }
    }
}