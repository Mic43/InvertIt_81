using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Base
{
    public interface IDomainEventHandler<in  TEvent> where TEvent : IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }
//    public interface IDomainEventHandler<in TEvent1, in TEvent2> :IDomainEventHandler<TEvent1> ,
//                    IDomainEventHandler<TEvent2> where TEvent1 : IDomainEvent 
//                        where TEvent2 : IDomainEvent
//    {
//        void Handle(TEvent2 domainEvent);
//    }
//    public interface IDomainEventHandler<in TEvent1, in TEvent2, in TEvent3> : IDomainEventHandler<TEvent1,TEvent2>
//        where TEvent1 : IDomainEvent
//        where TEvent2 : IDomainEvent
//        where TEvent3 : IDomainEvent
//    {
//        void Handle(TEvent3 domainEvent);
//    }
//    public interface IDomainEventHandler<in TEvent1, in TEvent2, in TEvent3,in TEvent4> : IDomainEventHandler<TEvent1, TEvent2,TEvent3>
//        where TEvent1 : IDomainEvent
//        where TEvent2 : IDomainEvent
//        where TEvent3 : IDomainEvent
//        where TEvent4 : IDomainEvent
//    {
//        void Handle(TEvent4 domainEvent);
//    }

    public interface IDomainEventHandler
    {
        void Handle(object domainEvent);
    }
}