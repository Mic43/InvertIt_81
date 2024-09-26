using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public interface IEventsBus
    {
        void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    }
}