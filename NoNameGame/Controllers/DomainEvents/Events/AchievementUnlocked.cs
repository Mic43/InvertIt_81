using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Events
{
    public class AchievementUnlocked : IDomainEvent
    {
        private readonly NewAchievement _achievement;
        public AchievementUnlocked(NewAchievement achievement)
        {
            _achievement = achievement;
        }
        public NewAchievement Achievement
        {
            get { return _achievement; }
        }
    }
}