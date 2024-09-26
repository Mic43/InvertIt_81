using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Base
{
    class GameInterruptedHandler : IDomainEventHandler<GamePaused>,IDomainEventHandler<GameLeft>,IDomainEventHandler<GameWon>
    {
        private readonly IEventsBus _eventsBus;
        private void FireGameInterrupted()
        {
            _eventsBus.Publish(new GameInterrupted());
        }
        public GameInterruptedHandler(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }
        public void Handle(GameLeft domainEvent)
        {
            FireGameInterrupted();
        }
        public void Handle(GamePaused domainEvent)
        {
            FireGameInterrupted();
        }
        public void Handle(GameWon domainEvent)
        {
            FireGameInterrupted();
        }
    }
}
