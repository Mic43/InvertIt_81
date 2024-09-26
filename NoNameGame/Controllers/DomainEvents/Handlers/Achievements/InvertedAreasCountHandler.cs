using Infrastructure;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class InvertedAreasCountHandler : AchievementHandlerBase<MoveMade>
    {
        public InvertedAreasCountHandler(IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement)
        {
        }
        public override void Handle(MoveMade domainEvent)
        {
            int invertedCount = 0;
            domainEvent.BeforeMoveBoard.Areas.ForEach((area, i, j) =>
            {
                if (domainEvent.AfterMoveBoard.Areas[i, j].AreaState != area.AreaState)
                    invertedCount++;
            });
           Achievement.ReportProgress(invertedCount);
        }
    }
}