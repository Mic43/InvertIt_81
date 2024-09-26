using GameLogic;

namespace NoNameGame.Models.ChallengeGame
{
    public class Challenge
    {
        public Level Level { get; private set; }

        public Challenge(Level level)
        {
            Level = level;
        }
    }
}