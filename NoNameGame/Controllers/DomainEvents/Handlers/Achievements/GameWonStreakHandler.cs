using System;
using Infrastructure.Storage;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Helpers;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class GameWonStreakHandler : AchievementHandlerBase<GameWon,GameReset,GameLeft>
    {        
        private readonly int _goalStreakCount;
        private readonly IGameWonCondition _eachGameWonCondition;

        //shitty temporal code - in order to maintain state between game sent to background
        protected int StreakLenght
        {
            get
            {
                return
                    AppSettingsAccessor.GetValueOrDefault(
                        string.Format("GameWonStreakHandler_{0}", Achievement.AchievementKey), 0);
            }
            set
            {
                AppSettingsAccessor.AddOrUpdateValue(string.Format("GameWonStreakHandler_{0}", Achievement.AchievementKey),value);
            }
        }
        private void ResetStreak()
        {
            StreakLenght = 0;
        }
        public GameWonStreakHandler(IEventsBus eventsBus, NewAchievement achievement,int goalStreakCount,IGameWonCondition eachGameWonCondition) 
            : base(eventsBus, achievement)
        {
            if (eachGameWonCondition == null) throw new ArgumentNullException("eachGameWonCondition");
            _goalStreakCount = goalStreakCount;
            _eachGameWonCondition = eachGameWonCondition;
        }
        public override void Handle(GameWon domainEvent)
        {
            if (_eachGameWonCondition.IsTrue(domainEvent))
            {
                StreakLenght++;
                if (StreakLenght == _goalStreakCount)
                {
                    StreakLenght = 0;
                    Achievement.ReportProgress(1);
                }
            }
            else
            {
                ResetStreak();
            }
        }
        public override void Handle(GameReset domainEvent)
        {
            ResetStreak();
        }
        public override void Handle(GameLeft domainEvent)
        {
            ResetStreak();
        }
    }
}
