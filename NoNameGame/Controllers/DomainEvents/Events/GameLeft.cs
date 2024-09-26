using System;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    public class GameLeft : IDomainEvent
    {
        public int LevelId { get;  private set; }
        public int PlayerMovesCount { get; private set; }
        public TimeSpan GameTime { get; private set; }
        public GameLeft(int levelId, int playerMovesCount, TimeSpan gameTime)
        {
            LevelId = levelId;
            PlayerMovesCount = playerMovesCount;
            GameTime = gameTime;            
        }
    }
}