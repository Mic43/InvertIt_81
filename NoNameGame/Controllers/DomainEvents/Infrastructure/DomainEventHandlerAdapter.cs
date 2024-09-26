using System.Diagnostics;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;

namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public class DomainEventHandlerAdapter<TEvent> : IDomainEventHandler where TEvent : IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _domainEventHandler;
        public DomainEventHandlerAdapter(IDomainEventHandler<TEvent> domainEventHandler)
        {
            _domainEventHandler = domainEventHandler;
        }
        public void Handle(object domainEvent)
        {
            if (domainEvent is TEvent)
            {
                Debug.WriteLine("Handled: " + domainEvent);
                _domainEventHandler.Handle((TEvent) domainEvent);
            }
        }
    }
}