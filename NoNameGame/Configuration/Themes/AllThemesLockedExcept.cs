using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Themes
{
    public class AllThemesLockedExcept : ThemeInitialDataDictionaryBase
    {
        public AllThemesLockedExcept(ThemeType themeUnlocked,ThemesDictionary dictionary)
        {
            foreach (Theme value in dictionary.Values)
            {
                if (value.ThemeType == themeUnlocked)
                    Add(value.ThemeType,new ThemeData(false));
                else
                    Add(value.ThemeType,new ThemeData(true));
            }
        }
    }
}