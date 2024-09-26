using System;
using Infrastructure.Storage;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;

namespace NoNameGame.Configuration.NewAchievements.ProgressStorer
{
    public class AppSettingsProgressStorer : ApsSettingsValueStorer<AchievementKey, NewAchievementDto>
    {
        public AppSettingsProgressStorer() : base(key => string.Format("NewAchievement_{0}", key))
        {
            
        }                 
    }

    public class AllUnlockedProgressStorer : IValueStorer<AchievementKey, NewAchievementDto>
    {
        public void Write(AchievementKey id, NewAchievementDto value)
        {
            ;
        }
        public NewAchievementDto Read(AchievementKey id, NewAchievementDto defaultValue)
        {
            return new NewAchievementDto(id,true,1.0,DateTime.MinValue);
        }
    }

}