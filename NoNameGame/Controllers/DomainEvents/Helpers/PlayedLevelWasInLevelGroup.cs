using System;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.DomainEvents.Helpers
{
    public class PlayedLevelWasInLevelGroup : IGameWonCondition
    {
        private readonly ILevelProvider _levelProvider;
        private readonly int _levelGroupId;
        public PlayedLevelWasInLevelGroup(ILevelProvider levelProvider, int levelGroupId)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            _levelProvider = levelProvider;
            _levelGroupId = levelGroupId;
        }
        public bool IsTrue(GameWon gameWon)
        {
            return _levelProvider.GetLevel(gameWon.PlayedLevelId).LevelGroupId == _levelGroupId;
        }
    }
}