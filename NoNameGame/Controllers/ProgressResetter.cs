using System;
using System.IO.IsolatedStorage;
using System.Linq;
using Infrastructure.Storage;
using NoNameGame.Configuration.NewAchievements;
using NoNameGame.Configuration.Themes;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Controllers
{
    public class ProgressResetter
    {
        private readonly ThemeController _themeController;
        private readonly NewAchievementsProvider _achievementsProvider;
        public ProgressResetter(ThemeController themeController,NewAchievementsProvider achievementsProvider)
        {
            if (themeController == null) throw new ArgumentNullException("themeController");
            if (achievementsProvider == null) throw new ArgumentNullException("achievementsProvider");

            _themeController = themeController;
            _achievementsProvider = achievementsProvider;
        }
        public void ResetProgress()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
            IsolatedStorageSettings.ApplicationSettings.Save();

            _themeController.ChangeTheme(ThemesDictionary.DefaultTheme);

            foreach (var achievement in _achievementsProvider.Get())
            {
                achievement.ResetProgress();
            }            
        }                
    }
}