using System;
using Infrastructure.Storage;
using NoNameGame.Configuration;
using NoNameGame.Controllers.DomainEvents.Achievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.Helpers.DateTime;

namespace NoNameGame.Controllers.DomainEvents.Handlers.Achievements
{
    public class AppStartedEveryDayHandler :AchievementHandlerBase<AppStarted,AppActivated>
    {
        private readonly IValueStorer<string, DateTime> _lastAppUsageDateStorer;
        private readonly IDateTimeNowProvider _dateTimeNowProvider;
        public AppStartedEveryDayHandler(IValueStorer<string,DateTime> lastAppUsageDateStorer,IDateTimeNowProvider dateTimeNowProvider,IEventsBus eventsBus, NewAchievement achievement)
            : base(eventsBus, achievement)
        {
            if (lastAppUsageDateStorer == null) throw new ArgumentNullException("lastAppUsageDateStorer");
            if (dateTimeNowProvider == null) throw new ArgumentNullException("dateTimeNowProvider");
            _lastAppUsageDateStorer = lastAppUsageDateStorer;
            _dateTimeNowProvider = dateTimeNowProvider;

            //Achievement.ProgressUpChanged+=AchievementOnProgressUpChanged;
        }
        private void AchievementOnProgressUpChanged(object sender, AchievementProgressUpChangedArgs achievementProgressUpChangedArgs)
        {
            //EventsBus.Publish(new AppStartedEveryDayProgress(((int) achievementProgressUpChangedArgs.GoalProgress),
              //                                          (int)achievementProgressUpChangedArgs.CurrentProgress));
        }
        public override void Handle(AppStarted domainEvent)
        {
            Handle();
        }
        public override void Handle(AppActivated domainEvent)
        {
            Handle();
        }
        private void Handle()
        {
            var now = _dateTimeNowProvider.GetNow().Date;
            var yesterday = now.AddDays(-1);
            var lastUsage = _lastAppUsageDateStorer.Read(AppSettingsKeys.LastAppUsageDate, yesterday).Date;

            if (lastUsage < yesterday)
                Achievement.ResetProgress();

            if (lastUsage <= yesterday)
                Achievement.ReportProgress(1.0); // we start from 1 always

            if (lastUsage != now)
                _lastAppUsageDateStorer.Write(AppSettingsKeys.LastAppUsageDate, now);
        }
    }
}