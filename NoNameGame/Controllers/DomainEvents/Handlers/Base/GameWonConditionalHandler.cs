using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Helpers;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Base
{
    public class GameWonConditionalHandler : ConditionalHandler<GameWon> 
    {
        public GameWonConditionalHandler(IEventsBus eventsBus, NewAchievement achievement,IGameWonCondition gameWonCondition)
            : base(eventsBus, achievement, gameWonCondition.IsTrue)
        {
        }
    }
}