using System;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    public class GameWon :IDomainEvent
    {
        public bool IsPerfectSolve { get; private set; }
        public int PlayedLevelId { get; private set; }
        public TimeSpan Duration { get; private set; }
        public bool WasFirstlySolved { get;private set; }

        public GameWon(bool isPerfectSolve, int playedLevelId, TimeSpan duration, bool wasFirstlySolved)
        {
            IsPerfectSolve = isPerfectSolve;
            PlayedLevelId = playedLevelId;
            Duration = duration;
            WasFirstlySolved = wasFirstlySolved;
        }
    }
}