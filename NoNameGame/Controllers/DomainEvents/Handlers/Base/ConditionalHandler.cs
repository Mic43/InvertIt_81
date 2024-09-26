using System;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Base
{
    public class ConditionalHandler<TEvent> : AchievementHandlerBase<TEvent> where TEvent : IDomainEvent
    {
        private readonly Func<TEvent, bool> _condition;
        public ConditionalHandler(IEventsBus eventsBus, NewAchievement achievement, Func<TEvent, bool> condition)
            : base(eventsBus, achievement)
        {
            _condition = condition;            
        }        

        public override void Handle(TEvent domainEvent)
        {
            if (_condition.Invoke(domainEvent))
                Achievement.ReportProgress(1);
        }
    }
}