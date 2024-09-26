using System.Collections.ObjectModel;
using NoNameGame.Controllers.DomainEvents;
using NoNameGame.Controllers.DomainEvents.Achievements;

namespace NoNameGame.Configuration.NewAchievements
{
    public class NewAchievementsCollection : KeyedCollection<AchievementKey, NewAchievement>
    {
        protected override AchievementKey GetKeyForItem(NewAchievement item)
        {
            return item.AchievementKey;
        }
    }
}