using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Themes
{
    public sealed class ThemesDictionary : ReadOnlyDictionary<ThemeType, Theme>
    {
        public static ThemeType DefaultTheme = ThemeType.Alizarin;

        private static readonly List<Theme> _values = new List<Theme>
        {
//            new Theme("blue-red", "BlueRed.xaml", false, ThemeType.BlueRed),
//            new Theme("green-orange", "GreenOrange.xaml", false, ThemeType.GreenOrange),
//            new Theme("teal", "Teal.xaml", false, ThemeType.Teal),
            new Theme("pink", "PinkFlat.xaml", true, ThemeType.FlatPink),
//            new Theme("pink", "Pink.xaml", false, ThemeType.Pink),
            new Theme("cobalt", "Cobalt.xaml", true, ThemeType.Cobalt),
            new Theme("koi - retro", "KoiRetro.xaml", true, ThemeType.RetroKoi),
            new Theme("amber", "AmberFlat.xaml", true, ThemeType.FlatAmber),
            new Theme("lipstick", "Lipstick.xaml", true, ThemeType.Lipstick),
            new Theme("lime", "LimeFlat.xaml", true, ThemeType.FlatLime),
//            new Theme("steel", "Steel.xaml", false, ThemeType.Steel),
            new Theme("spike", "SpikeFlat.xaml", true, ThemeType.FlatSpike),
            new Theme("copper - retro", "CopperRetro.xaml", true, ThemeType.RetroCopper),                                                   
            new Theme("humano", "Humano.xaml", true, ThemeType.Humano),
            new Theme("alizarin", "Alizarin.xaml", true, ThemeType.Alizarin),
            new Theme("bartman - retro", "BartmanRetro.xaml", true, ThemeType.RetroBartman),
            new Theme("young alice", "YoungAlice.xaml", true, ThemeType.YoungAlice),
            new Theme("peach", "Peach.xaml", true, ThemeType.Peach),
            new Theme("groove - retro", "GrooveRetro.xaml", true, ThemeType.RetroGroove),
          
            new Theme("triste - retro", "TristeRetro.xaml", true, ThemeType.RetroTriste),
            new Theme("alien", "Alien.xaml", true, ThemeType.Alien),
            new Theme("desiderata", "Desiderata.xaml", true, ThemeType.Desiderata),      
            new Theme("sky", "Sky.xaml", true, ThemeType.Sky),
//            new Theme("new", "New.xaml", true, ThemeType.New),
            new Theme("nature", "Nature.xaml", true, ThemeType.Nature),
             new Theme("yellow", "Yellow.xaml", true, ThemeType.Yellow),


        };
        private static Dictionary<ThemeType, Theme> CreateThemes()
        {
            return _values.ToDictionary(x => x.ThemeType);
        }
        public ThemesDictionary()
            : base(CreateThemes())
        {
        }
    }
}