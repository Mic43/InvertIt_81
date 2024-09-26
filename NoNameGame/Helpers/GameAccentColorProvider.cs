using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Infrastructure;
using NoNameGame.Configuration;

namespace NoNameGame.Helpers
{
    static class GameAccentColorProvider
    {
        public static Color GetLighter()
        {
            return (ColorManipulation.LightenColor(GameResources.Instance.CheckedColor, 0.85f));
        }
        public static Color GetDarker()
        {
            return (ColorManipulation.DarkenColor(GameResources.Instance.CheckedColor, 0.55f));
       }
    }
}
