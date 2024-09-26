using System;
using System.Collections.Generic;
using System.Linq;
using NoNameGame.Controllers.Themes;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Unlocks
{
    public class AchievementsController : IAchievement
    {
        private readonly Achievement[] _allAchievements;
        private readonly Unlocker<AchievementType> unlocker;
        public AchievementsController(params Achievement[] allAchievements)
        {
            if (allAchievements == null) throw new ArgumentNullException("allAchievements");

            _allAchievements = allAchievements;
            unlocker = new Unlocker<AchievementType>(_allAchievements.ToDictionary(x => x.AchievementType, x=> true),x=>x.ToString());            

        }
        public IEnumerable<Achievement> GetJustUnlockedAchievments(IEnumerable<Achievement> lockedAchievements)
        {
            return lockedAchievements.Where(x => x.WasActionPerformed());
        }
        private IEnumerable<Achievement> GetLockedAchievements()
        {
            return _allAchievements.Where(achievement => unlocker.IsLocked(achievement.AchievementType));
        }
          
        public void Execute()
        {
            var lockedAchievements = GetLockedAchievements().ToList();
            foreach (var achievement in lockedAchievements)
            {
                achievement.Execute();
            }

            foreach (var achievement in GetJustUnlockedAchievments(lockedAchievements))
            {
               unlocker.Unlock(achievement.AchievementType);
            }
        }
        public bool WasActionPerformed()
        {
            return GetLockedAchievements().Any(x => x.WasActionPerformed());
        }
    }
}