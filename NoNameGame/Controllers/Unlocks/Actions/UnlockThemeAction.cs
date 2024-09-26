using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Controllers.Themes;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Unlocks.Actions
{
    class UnlockThemeAction :IAction
    {
        private readonly ThemeType _themeType;
        private readonly IFiniteTypeStorer<ThemeType, ThemeData> _unlocker;

        public UnlockThemeAction(ThemeType themeType, IFiniteTypeStorer<ThemeType, ThemeData> unlocker)
        {
            _themeType = themeType;
            _unlocker = unlocker;            
        }
        public ThemeType ThemeType
        {
            get { return _themeType; }
        }
        public void Perform()
        {
            _unlocker.Write(ThemeType,new ThemeData(false));
        }
    }
}