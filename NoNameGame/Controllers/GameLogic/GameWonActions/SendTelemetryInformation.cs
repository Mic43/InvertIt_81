using System;
using System.Collections.Generic;
using GameLogic.Game;
using Microsoft.ApplicationInsights;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Helpers.Telemetry;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    class SendTelemetryInformation : IGameWonAction
    {
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly TelemetryClient _telemetryClient;
        public SendTelemetryInformation(IPlayerStatsProvider playerStatsProvider,
            ICurrentLevelDataProvider currentLevelDataProvider, TelemetryClient telemetryClient)
        {
            if (playerStatsProvider == null) throw new ArgumentNullException("playerStatsProvider");
            if (currentLevelDataProvider == null) throw new ArgumentNullException("currentLevelDataProvider");
            if (telemetryClient == null) throw new ArgumentNullException("telemetryClient");

            _playerStatsProvider = playerStatsProvider;
            _currentLevelDataProvider = currentLevelDataProvider;
            _telemetryClient = telemetryClient;
        }
        public void Execute(GameWonData gameWonData)
        {
            var levelTelemetryData = _currentLevelDataProvider.Get(gameWonData.PlayedLevelId);

            ///disbaled due to performance reasons
            //var gp = new GameProgress(_playerStatsProvider.GetCurrentPlayerStarsCount(), _playerStatsProvider.GetAllLevelsStarsCount());
            var gp = new GameProgress(0.0);
            var metrics = new LevelFinishedTelemetryMetrics(new LevelTelemetryMetrics(
                                                                gameWonData.WonGameStats.PlayerMovesCount, gameWonData.WonGameStats.SolveTime, gp),
                                                            gameWonData.WonGameStats.Points);
            _telemetryClient.TrackEvent("GameWon", levelTelemetryData.ToDictionary(), metrics.ToDictionary());
        }
    }

}