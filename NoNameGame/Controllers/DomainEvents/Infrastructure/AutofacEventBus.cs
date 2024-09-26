using System.Collections.Generic;
using Autofac;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;

namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public class AutofacEventBus : IEventsBus
    {
        private readonly IComponentContext _container;
        public AutofacEventBus(IComponentContext container)
        {
            _container = container;
        }
        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            foreach (var domainEventHandler in _container.Resolve<IEnumerable<IDomainEventHandler<TEvent>>>())
            {
                domainEventHandler.Handle(domainEvent);
            }
        }
    }
}