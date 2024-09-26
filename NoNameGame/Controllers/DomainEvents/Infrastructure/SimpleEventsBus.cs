using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;

namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public class SimpleEventsBus : IEventsBus
    {
        protected IEnumerable<IDomainEventHandler> DomainEventHandlers;
               
        public void RegisterEvents(IEnumerable<IDomainEventHandler> domainEventHandlers)
        {
            if (domainEventHandlers == null) throw new ArgumentNullException("domainEventHandlers");
            DomainEventHandlers = domainEventHandlers;
        }
        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            if (domainEvent == null) throw new ArgumentNullException("domainEvent");          
            foreach (var eventHandler in DomainEventHandlers)
            {
                eventHandler.Handle(domainEvent);
            }            
        }
    }
}