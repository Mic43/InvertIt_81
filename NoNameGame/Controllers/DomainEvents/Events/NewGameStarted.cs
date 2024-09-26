using System;
using GameLogic;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    public class NewGameStarted : IDomainEvent
    {
        private readonly Level _levelToPlay;
        public NewGameStarted(Level levelToPlay)
        {
            if (levelToPlay == null) throw new ArgumentNullException("levelToPlay");
            _levelToPlay = levelToPlay;            
        }
        public Level LevelToPlay
        {
            get { return _levelToPlay; }
        }
    }
}