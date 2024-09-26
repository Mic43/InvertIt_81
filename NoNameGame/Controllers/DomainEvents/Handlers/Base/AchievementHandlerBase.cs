using System;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Base
{

    public abstract class AchievementHandlerBase<TEvent> : IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        protected readonly IEventsBus EventsBus;
        protected readonly NewAchievement Achievement;
        public AchievementHandlerBase(IEventsBus eventsBus, NewAchievement achievement)
        {
            EventsBus = eventsBus;
            Achievement = achievement;
            Achievement.Unlocked += AchievementOnUnlocked;
        }
        private void AchievementOnUnlocked(object sender, EventArgs eventArgs)
        {
            EventsBus.Publish(new AchievementUnlocked(Achievement));
        }
        public abstract void Handle(TEvent domainEvent);
    }

    public abstract class  AchievementHandlerBase<TEvent1, TEvent2> : AchievementHandlerBase<TEvent1>, IDomainEventHandler<TEvent2> 
        where TEvent1 : IDomainEvent where TEvent2 : IDomainEvent
    {
        protected AchievementHandlerBase(IEventsBus eventsBus, NewAchievement achievement) : base(eventsBus, achievement)
        {
        }
        public override abstract void Handle(TEvent1 domainEvent);
        public abstract void Handle(TEvent2 domainEvent);

    }
    public abstract class AchievementHandlerBase<TEvent1, TEvent2, TEvent3> : AchievementHandlerBase<TEvent1, TEvent2>, IDomainEventHandler<TEvent3>
        where TEvent1 : IDomainEvent
        where TEvent2 : IDomainEvent
        where TEvent3 : IDomainEvent
    {
        protected AchievementHandlerBase(IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement)
        {
        }
        public override abstract void Handle(TEvent1 domainEvent);
        public override abstract void Handle(TEvent2 domainEvent);
        public abstract void Handle(TEvent3 domainEvent);    
    }
    public abstract class AchievementHandlerBase<TEvent1, TEvent2, TEvent3, TEvent4> : AchievementHandlerBase<TEvent1, TEvent2, TEvent3>, IDomainEventHandler<TEvent4>
        where TEvent1 : IDomainEvent
        where TEvent2 : IDomainEvent
        where TEvent3 : IDomainEvent
        where TEvent4 : IDomainEvent
    {
        protected AchievementHandlerBase(IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement)
        {
        }
        public override abstract void Handle(TEvent1 domainEvent);
        public override abstract void Handle(TEvent2 domainEvent);
        public override abstract void Handle(TEvent3 domainEvent);
        public abstract void Handle(TEvent4 domainEvent);    
    } 
}