using System;
using System.Linq;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class LevelPackFinishedHandler : AchievementHandlerBase<GameWon,AppStarted>
    {
        private readonly int _levelPackId;
        private readonly ILevelProvider _levelProvider;
        private readonly ILevelProgressStorer _levelProgressStorer;
        private bool IsLevelPackFinished()
        {
           return _levelProvider.GetLevelGroupsForLevelPack(_levelPackId)
                                .All(lp => _levelProvider.GetLevelsForLevelGroup(lp.Id)
                                                         .All(level => _levelProgressStorer.Load(level.Id).IsFinished));            
        }
        public LevelPackFinishedHandler(IEventsBus eventsBus, NewAchievement achievement, int levelPackId,
            ILevelProvider levelProvider,ILevelProgressStorer levelProgressStorer) : base(eventsBus, achievement)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            if (levelProgressStorer == null) throw new ArgumentNullException("levelProgressStorer");

            _levelPackId = levelPackId;
            _levelProvider = levelProvider;
            _levelProgressStorer = levelProgressStorer;
        }
        public override void Handle(GameWon domainEvent)
        {
            
            int currentLevelPack = _levelProvider.GetLevelGroup(_levelProvider.GetLevel(domainEvent.PlayedLevelId).LevelGroupId).LevelPackId;
            if (currentLevelPack != _levelPackId)
                return;

            if (!IsLevelPackFinished())
                return;

            Achievement.ReportProgress(1);
        }
        // in order to unlock achievement for users who finished groups before achievements were introtuced in update
        public override void Handle(AppStarted domainEvent)
        {

            if (Achievement.IsUnlocked)
                return;

            if (!IsLevelPackFinished())
                 return;

            Achievement.ReportProgress(1,true);
            
        }
    }
}