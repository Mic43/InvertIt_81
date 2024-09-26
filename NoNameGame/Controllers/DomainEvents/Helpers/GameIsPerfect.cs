using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    class GameIsPerfect : IGameWonCondition
    {
        public bool IsTrue(GameWon gameWon)
        {
            return gameWon.IsPerfectSolve;
        }
    }
}