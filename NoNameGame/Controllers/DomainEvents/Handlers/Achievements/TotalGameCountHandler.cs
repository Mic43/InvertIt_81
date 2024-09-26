using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class TotalGameCountHandler : ConditionalHandler<GameWon>
    {
        public TotalGameCountHandler(IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement,(gw) => true)
        {
        }        
    }
}