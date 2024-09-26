using System;
using System.Collections.Generic;

namespace NoNameGame.Helpers.Telemetry
{
    public class LevelFinishedTelemetryMetrics
    {
        public LevelTelemetryMetrics LevelTelemetryMetrics { get; private set; }
        public int StarsCount { get; private set; }

        public LevelFinishedTelemetryMetrics(LevelTelemetryMetrics levelTelemetryMetrics, int starsCount)
        {
            if (levelTelemetryMetrics == null) throw new ArgumentNullException("levelTelemetryMetrics");
            LevelTelemetryMetrics = levelTelemetryMetrics;
            StarsCount = starsCount;
        }
        public Dictionary<string, double> ToDictionary()
        {
            var dictionary = LevelTelemetryMetrics.ToDictionary();
            dictionary.Add("StarsCount",StarsCount);
            return dictionary;
        }
    }
}