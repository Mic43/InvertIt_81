using System;

namespace NoNameGame.Models.ChallengeGame
{
    public class CurrentPlayedChallengeModel
    {
        public int PlayerMovesCount { get;  set; }
        public int PerfectMovesCount { get;  set; }
        public TimeSpan PlayTime { get; set; }
    }
}