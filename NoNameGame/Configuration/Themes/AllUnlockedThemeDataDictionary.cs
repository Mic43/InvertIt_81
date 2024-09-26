using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Themes
{
    public class AllUnlockedThemeInitialDataDictionary : ThemeInitialDataDictionaryBase
    {
        public AllUnlockedThemeInitialDataDictionary(ThemeInitialDataDictionaryBase source)
        {
            foreach (var keyValuePair in source)
            {
                Add(keyValuePair.Key,new ThemeData(false));
            }
        }
    }
}