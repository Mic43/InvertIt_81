using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class AppRatedHandler : AchievementHandlerBase<AppRated>
    {
        public AppRatedHandler(IEventsBus eventsBus, NewAchievement achievement) : base(eventsBus, achievement)
        {
        }
        public override void Handle(AppRated domainEvent)
        {
            Achievement.ReportProgress(1);
        }
    }
}