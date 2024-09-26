using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using NoNameGame.Controllers.Unlocks;
using NoNameGame.Controllers.Unlocks.Actions;
using NoNameGame.Controllers.Unlocks.Conditions;

namespace NoNameGame.Controllers.Themes
{
    public class ThemeStarsToUnlockProvider
    {
        private readonly IEnumerable<Achievement> _achievments;
        public ThemeStarsToUnlockProvider(IEnumerable<Achievement> achievments )
        {
            _achievments = achievments;
        }
        public int GetStarsToUnlockCount(ThemeType themeType)
        {
            var condition =
                _achievments.Where(
                    x => x.Action is UnlockThemeAction && ((UnlockThemeAction) x.Action).ThemeType == themeType)
                    .Select(x => x.Condition)
                    .SingleOrDefault();
            if (condition == null)
                return 0;
            var graterThannCondition = (condition as PlayerStarsGreaterEqualThan);
            if (graterThannCondition == null)
                throw new ArgumentException("Provided themeType does not have unlocking action of PlayerStarsGreaterEqualThan type. See AchievementsCreator definition");

            return graterThannCondition.StarsCount;
        }
    }
}