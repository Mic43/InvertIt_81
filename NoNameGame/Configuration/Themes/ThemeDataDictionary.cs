using System.Collections.ObjectModel;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Themes
{
    public class ThemeInitialDataDictionary : ThemeInitialDataDictionaryBase
    {
        public ThemeInitialDataDictionary()
        {            
            Add(ThemeType.BlueRed, new ThemeData(false));
            Add(ThemeType.Cobalt, new ThemeData(true));
            Add(ThemeType.FlatAmber, new ThemeData( true));
            Add(ThemeType.FlatLime,new ThemeData( true));
            Add(ThemeType.FlatMauve, new ThemeData( true));
            Add(ThemeType.FlatPink, new ThemeData( true));            
            Add(ThemeType.GreenOrange,new ThemeData( true));            
            Add(ThemeType.Pink, new ThemeData( true));
            Add(ThemeType.Steel, new ThemeData( true));
            Add(ThemeType.Teal,new ThemeData( true));
            Add(ThemeType.FlatSpike,new ThemeData( true));
            Add(ThemeType.RetroCopper, new ThemeData(true));
            Add(ThemeType.RetroKoi, new ThemeData(true));
            Add(ThemeType.RetroGroove, new ThemeData(true));
            Add(ThemeType.RetroBartman, new ThemeData(true));            
            //Add(ThemeType.New, new ThemeData(false));
            Add(ThemeType.RetroTriste, new ThemeData(true));
            Add(ThemeType.Humano, new ThemeData(true));
            Add(ThemeType.Alizarin, new ThemeData(true));
            Add(ThemeType.YoungAlice, new ThemeData(true));
            Add(ThemeType.Peach, new ThemeData(true));
            Add(ThemeType.Lipstick, new ThemeData(true));
            Add(ThemeType.Alien, new ThemeData(true));
            Add(ThemeType.Desiderata, new ThemeData(true));
            Add(ThemeType.Sky, new ThemeData(true));
            Add(ThemeType.Nature, new ThemeData(true));
            Add(ThemeType.Yellow, new ThemeData(true));
        }
    }
}