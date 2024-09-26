using System.Collections.Generic;
using System.Linq;
using Infrastructure.Storage;
using NoNameGame.Configuration.Themes;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Controllers.Themes;
using NoNameGame.Controllers.Unlocks;
using NoNameGame.Controllers.Unlocks.Actions;
using NoNameGame.Controllers.Unlocks.Conditions;

namespace NoNameGame.Configuration.Achievements
{
    public sealed class AutomaticAchievementsCreator : IAchievementsCreator
    {
        private readonly IPlayerStatsProvider _provider;
        private readonly IFiniteTypeStorer<ThemeType, ThemeData> _themeUnlocker;
        private readonly ThemesDictionary _themesDictionary;
        private readonly int _firstUnlockStarsCount;
        private readonly int _lastUnlockStarsCount;
        private double a = 1;

        public List<Achievement> Get()
        {
            return new List<Achievement>(_themesDictionary.Values.Where(theme => theme.ThemeType != ThemesDictionary.DefaultTheme)
                .Select((theme, i) =>
                    new ExecuteOnceAchievment(i, AchievementType.UnlockTheme,
                        new PlayerStarsGreaterEqualThan(GetStarsCount(i,a), _provider),
                        new UnlockThemeAction(theme.ThemeType, _themeUnlocker))).ToList());
        }
        private int GetStarsCount(int i,double a)
        {
            int allThemesToUnlockCount = _themesDictionary.Values.Count - 1;

            int x1 = 0;
            int y1 = _firstUnlockStarsCount;
            int x2 = allThemesToUnlockCount;
            int y2 = _lastUnlockStarsCount;

            double b = (double)(y1 - y2)/(x1 - x2) - a*(x1 + x2);
            double c = y1 - a*x1*x1 - b*x1;                 

            return (int)(a*(i*i) + b*i + c);
        }
        public AutomaticAchievementsCreator(IPlayerStatsProvider provider,
            IFiniteTypeStorer<ThemeType, ThemeData> themeUnlocker,
            ThemesDictionary themesDictionary,
            int firstUnlockStarsCount, int lastUnlockStatsCount)
        {
            _provider = provider;
            _themeUnlocker = themeUnlocker;
            _themesDictionary = themesDictionary;
            _firstUnlockStarsCount = firstUnlockStarsCount;
            _lastUnlockStarsCount = lastUnlockStatsCount;
        }
    }
}