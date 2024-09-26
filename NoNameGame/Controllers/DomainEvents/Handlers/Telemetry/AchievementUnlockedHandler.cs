using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Telemetry
{
    public class AchievementUnlockedHandler : IDomainEventHandler<AchievementUnlocked>
    {
        private readonly TelemetryClient _telemetryClient;
        public AchievementUnlockedHandler(TelemetryClient telemetryClient)
        {
            if (telemetryClient == null) throw new ArgumentNullException("telemetryClient");
            _telemetryClient = telemetryClient;
        }
        public void Handle(AchievementUnlocked domainEvent)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"AchievementName",domainEvent.Achievement.AchievementKey.ToString()}
            };
            _telemetryClient.TrackEvent("AchievementUnlocked",dict);
        }
    }
}