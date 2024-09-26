using System;
using System.Collections.Generic;
using NoNameGame.Controllers.PlayerStats;

namespace NoNameGame.Helpers.Telemetry
{
    public class LevelTelemetryMetrics
    {
     
        public int MovesCount { get; private set; }
        public TimeSpan CurrentLevelPlayTime { get; private set; }
        public GameProgress GameProgress { get; private set; }

        public LevelTelemetryMetrics(int movesCount, TimeSpan currentLevelPlayTime, GameProgress gameProgress)
        {
            if (gameProgress == null) throw new ArgumentNullException("gameProgress");

       
            MovesCount = movesCount;
            CurrentLevelPlayTime = currentLevelPlayTime;
            GameProgress = gameProgress;
        }

        public Dictionary<string, double> ToDictionary()
        {
            var metrics = new Dictionary<string, double>()
            {               
                {"SolveTime", CurrentLevelPlayTime.TotalSeconds},
                {"MovesCount",MovesCount},
                {"GameProgress",GameProgress.Value}
            };
            return metrics;
        }
    }
}