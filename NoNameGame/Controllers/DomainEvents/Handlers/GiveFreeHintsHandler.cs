using System;
using NoNameGame.Configuration.NewAchievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.Hints.HintsCount;

namespace NoNameGame.Controllers.DomainEvents.Handlers
{
    public class GiveFreeHintsHandler : IDomainEventHandler<AchievementUnlocked>
    {
        private readonly AchievementKey _targetAchievement;
        private readonly int _hintsCountToGive;
        private readonly IHintsCountIncreaser _hintsCountIncreaser;
        public GiveFreeHintsHandler(AchievementKey targetAchievement,int hintsCountToGive,IHintsCountIncreaser hintsCountIncreaser)
        {
            if (hintsCountIncreaser == null) throw new ArgumentNullException("hintsCountIncreaser");
            this._targetAchievement = targetAchievement;
            _hintsCountToGive = hintsCountToGive;
            _hintsCountIncreaser = hintsCountIncreaser;
        }
        public void Handle(AchievementUnlocked domainEvent)
        {
            if (domainEvent.Achievement.AchievementKey != _targetAchievement)
                return;
            _hintsCountIncreaser.Increase(_hintsCountToGive);
        }
    }
}