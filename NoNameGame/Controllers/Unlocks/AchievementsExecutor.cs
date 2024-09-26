using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Controllers.Themes;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Unlocks
{
    public class AchievementsExecutor 
    {
        private string CreateAchivmentSettingsKey(Achievement ach)
        {
            return ach.AchievementType + ach.Id.ToString();
        }
        private readonly List<Achievement> _achievements;
        private Achievement[] _lastlyExecutedAchievements;
        
        public AchievementsExecutor(List<Achievement> achievements)
        {
            if (achievements == null) throw new ArgumentNullException("achievements");
                    
            _achievements = achievements;
            _lastlyExecutedAchievements = Enumerable.Empty<Achievement>().ToArray();

        }
        public ReadOnlyCollection<Achievement> AllAchievements
        {
            get { return new ReadOnlyCollection<Achievement>(_achievements); }
        }
        public ReadOnlyCollection<Achievement> LastlyExecutedAchievements
        {
            get { return new ReadOnlyCollection<Achievement>(_lastlyExecutedAchievements); }
        }
        public void Execute()
        {
            _achievements.ForEach(
                ach =>
                {
                    string key = CreateAchivmentSettingsKey(ach);
                    ach.IsEnabled = AppSettingsAccessor.GetValueOrDefault(key, true);
                });

            _achievements.ForEach(ach => ach.Execute());

            foreach (var achievement in _achievements.Where(ach=>ach.WasActionPerformed()))
            {
                AppSettingsAccessor.AddOrUpdateValue(CreateAchivmentSettingsKey(achievement), achievement.IsEnabled);
            }
            AppSettingsAccessor.Save();

            _lastlyExecutedAchievements = _achievements.Where(x => x.WasActionPerformed()).ToArray();
        }
        public bool WasActionPerformed()
        {
            return LastlyExecutedAchievements.Any();
        }
    }
}