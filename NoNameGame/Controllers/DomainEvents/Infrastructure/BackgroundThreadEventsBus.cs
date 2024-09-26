using System;
using System.Threading.Tasks;
using NoNameGame.Controllers.DomainEvents.Events;

namespace NoNameGame.Controllers.DomainEvents.Infrastructure
{
    public class BackgroundThreadEventsBus : IEventsBus
    {
        private readonly IEventsBus _eventsBus;
        private readonly object _padlock = new object();
        public BackgroundThreadEventsBus(IEventsBus eventsBus)
        {
            if (eventsBus == null) throw new ArgumentNullException("eventsBus");
            _eventsBus = eventsBus;
        }
        public void Publish<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            //_eventsBus.Publish(domainEvent);
            
            Task.Run(() => _eventsBus.Publish(domainEvent));   
//            lock (_padlock)
//            {
//                Task.Run(() => _eventsBus.Publish(domainEvent));   
//            }                
        }
    }
}