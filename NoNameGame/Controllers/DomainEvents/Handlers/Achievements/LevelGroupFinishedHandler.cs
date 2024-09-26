using System;
using System.Linq;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class LevelGroupFinishedHandler : AchievementHandlerBase<GameWon,AppStarted>
    {
        private readonly int _levelGroup;
        private readonly ILevelProvider _levelProvider;
        private readonly ILevelProgressStorer _levelProgressStorer;
        private bool IsLevelGroupFinished()
        {
            bool allLevelsFinished = _levelProvider.GetLevelsForLevelGroup(_levelGroup)
                .All(level => _levelProgressStorer.Load(level.Id).IsFinished);
            
            return allLevelsFinished;
        }
        public LevelGroupFinishedHandler(IEventsBus eventsBus, NewAchievement achievement,int levelGroup,
            ILevelProvider levelProvider,ILevelProgressStorer levelProgressStorer) : base(eventsBus, achievement)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (levelProgressStorer == null) throw new ArgumentNullException("levelProgressStorer");

            _levelGroup = levelGroup;
            _levelProvider = levelProvider;
            _levelProgressStorer = levelProgressStorer;
        }
        public override void Handle(GameWon domainEvent)
        {
            int currentLevelGroupId = _levelProvider.GetLevel(domainEvent.PlayedLevelId).LevelGroupId;
            if (currentLevelGroupId != _levelGroup)
                return;

            if (!IsLevelGroupFinished())
                return;

            Achievement.ReportProgress(1);
        }
        // in order to unlock achievement for users who finished groups before achievements were introtuced in update
        public override void Handle(AppStarted domainEvent)
        {

            if (Achievement.IsUnlocked)
                return;
            
            if (!IsLevelGroupFinished())
                 return;

            Achievement.ReportProgress(1,true);
            
        }
    }
}