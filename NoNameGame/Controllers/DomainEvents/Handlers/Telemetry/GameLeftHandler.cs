using System;
using Microsoft.ApplicationInsights;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Helpers.Telemetry;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Telemetry
{
    public class GameLeftHandler : IDomainEventHandler<GameLeft>
    {
        private readonly ICurrentLevelDataProvider _currentLevelDataProvider;
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly TelemetryClient _telemetryClient;
        public GameLeftHandler(ICurrentLevelDataProvider currentLevelDataProvider, IPlayerStatsProvider playerStatsProvider,TelemetryClient telemetryClient)
        {
            if (currentLevelDataProvider == null) throw new ArgumentNullException("currentLevelDataProvider");
            if (playerStatsProvider == null) throw new ArgumentNullException("playerStatsProvider");
            if (telemetryClient == null) throw new ArgumentNullException("telemetryClient");

            _currentLevelDataProvider = currentLevelDataProvider;
            _playerStatsProvider = playerStatsProvider;
            _telemetryClient = telemetryClient;
        }
        public void Handle(GameLeft domainEvent)
        {
            var levelData = _currentLevelDataProvider.Get(domainEvent.LevelId);
            var metrics = new LevelTelemetryMetrics(domainEvent.PlayerMovesCount, domainEvent.GameTime,
                                new GameProgress(_playerStatsProvider.GetCurrentPlayerStarsCount(),
                                    _playerStatsProvider.GetAllLevelsStarsCount()));
            _telemetryClient.TrackEvent("GameLeft", levelData.ToDictionary(), metrics.ToDictionary());
        }
    }
}