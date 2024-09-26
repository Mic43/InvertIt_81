using System.Diagnostics;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class TotalGameTimeInMinutesHandler : AchievementHandlerBase<NewGameStarted, GameResumed, GameTimeTick, GameInterrupted>       

    {
        private readonly Stopwatch _stopwatch;
        public TotalGameTimeInMinutesHandler(IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement)
        {
            _stopwatch = new Stopwatch();
        }
        
        public override void Handle(NewGameStarted domainEvent)
        {
           _stopwatch.Restart();
        }
        public override void Handle(GameResumed domainEvent)
        {
            _stopwatch.Restart();
        }       
        public override void Handle(GameTimeTick domainEvent)
        {
           Achievement.ReportProgress(_stopwatch.Elapsed.TotalMinutes);
            if (_stopwatch.IsRunning)            
                _stopwatch.Restart();            
            else           
                _stopwatch.Reset();                                       
        }
        public override void Handle(GameInterrupted domainEvent)
        {
            _stopwatch.Stop();
        }
    }
}