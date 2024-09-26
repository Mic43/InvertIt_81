using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Storage;
using NoNameGame.Controllers;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Controllers.Themes;
using NoNameGame.Controllers.Unlocks;
using NoNameGame.Controllers.Unlocks.Actions;
using NoNameGame.Controllers.Unlocks.Conditions;



namespace NoNameGame.Configuration.Achievements
{
    public sealed class ManualAchievementsCreator : IAchievementsCreator
    {
        private readonly IPlayerStatsProvider _provider;
        private readonly IFiniteTypeStorer<ThemeType, ThemeData> _themeUnlocker;

        private Achievement CreateSingleAchivementEntry(int id, ThemeType themeType, int starsTreshold)
        {
            return 
                new ExecuteOnceAchievment(id, AchievementType.UnlockTheme,
                    new PlayerStarsGreaterEqualThan(starsTreshold, _provider),
                    new UnlockThemeAction(themeType, _themeUnlocker));
        }
        public List<Achievement> Get()
        {
            var achievementsDictionary = new List<Achievement>()
            {
               {CreateSingleAchivementEntry(1,ThemeType.FlatPink, 35)},
               {CreateSingleAchivementEntry(2,ThemeType.Cobalt, 60)},
               {CreateSingleAchivementEntry(3,ThemeType.RetroKoi, 85)},
               {CreateSingleAchivementEntry(4,ThemeType.FlatAmber, 110)},
               {CreateSingleAchivementEntry(5,ThemeType.Lipstick, 140)},
               {CreateSingleAchivementEntry(6,ThemeType.FlatLime, 170)},
               {CreateSingleAchivementEntry(7,ThemeType.FlatSpike, 200)},
               {CreateSingleAchivementEntry(8,ThemeType.RetroCopper, 240)},
               {CreateSingleAchivementEntry(9,ThemeType.Humano, 280)},
               {CreateSingleAchivementEntry(10,ThemeType.RetroBartman, 320)},
               {CreateSingleAchivementEntry(11,ThemeType.YoungAlice, 360)},
               {CreateSingleAchivementEntry(12,ThemeType.Peach, 400)},
               {CreateSingleAchivementEntry(13,ThemeType.RetroGroove, 450)},
               {CreateSingleAchivementEntry(14,ThemeType.RetroTriste, 500)},
               {CreateSingleAchivementEntry(15,ThemeType.Alien, 550)},
               {CreateSingleAchivementEntry(16,ThemeType.Desiderata, 600)},
               {CreateSingleAchivementEntry(17,ThemeType.Sky, 800)},
               {CreateSingleAchivementEntry(18,ThemeType.Nature, 1000)},
               {CreateSingleAchivementEntry(19,ThemeType.Yellow, 1500)},
            };   
            if (achievementsDictionary.Select(x=>x.Id).Distinct().Count() != achievementsDictionary.Count)
                throw new ApplicationException("Error in achivements list. Ids are duplicated");
            return achievementsDictionary;
        }
        public ManualAchievementsCreator(IPlayerStatsProvider provider, IFiniteTypeStorer<ThemeType, ThemeData> themeUnlocker)            
        {
            _provider = provider;
            _themeUnlocker = themeUnlocker;
        }
    }
}