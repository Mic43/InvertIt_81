using System;
using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    public class GameTimeLessThan : IGameWonCondition
    {
        private readonly TimeSpan _goalDuration;
        public GameTimeLessThan(TimeSpan goalDuration)
        {
            _goalDuration = goalDuration;
        }
        public bool IsTrue(GameWon gameWon)
        {
            return gameWon.Duration.CompareTo(_goalDuration) <= 0;
        }
    }
}