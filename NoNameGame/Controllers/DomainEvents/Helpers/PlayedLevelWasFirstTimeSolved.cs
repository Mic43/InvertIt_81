using System;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    public class PlayedLevelWasFirstTimeSolved : IGameWonCondition
    {           
        public bool IsTrue(GameWon gameWon)
        {
            return gameWon.WasFirstlySolved;
        }
    }
}