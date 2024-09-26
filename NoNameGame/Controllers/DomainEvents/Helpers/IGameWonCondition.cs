using System.Collections;
using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    public interface IGameWonCondition
    {
        bool IsTrue(GameWon gameWon);
    }

    public class AlwaysTrue : IGameWonCondition
    {
        public bool IsTrue(GameWon gameWon)
        {
            return true;
        }
    }
}