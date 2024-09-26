using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Themes;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Themes
{
    public class ThemeBrushesProvider
    {
        private readonly ThemesDictionary _themesDictionary;
        private readonly Dictionary<ThemeType, BrushPair> _themeBrushesCache = new Dictionary<ThemeType, BrushPair>();

        private class BrushPair
        {
            public Brush MainBrush { get; set; }
            public Brush SecondBrush { get; set; }
            public BrushPair(Brush mainBrush, Brush secondBrush)
            {
                MainBrush = mainBrush;
                SecondBrush = secondBrush;
            }
        }

        public ThemeBrushesProvider(ThemesDictionary themesDictionary)
        {
            _themesDictionary = themesDictionary;
            foreach (var theme in themesDictionary)
            {
                _themeBrushesCache.Add(theme.Key, new BrushPair(GetThemeMainBrushInternal(theme.Key),GetThemeSecondBrushInternal(theme.Key)));
            }
        }
        private Brush GetThemeMainBrushInternal(ThemeType themeType)
        {
            string fileName = _themesDictionary[themeType].FileName;
            ResourceDictionary dict = ResourcesHelper.OpenResourceDictionary(fileName);
            return ((Brush) dict[ThemesResoucesKeyNames.CheckedAreaGradientBrush]);
        }
        public Brush GetThemeSecondBrushInternal(ThemeType themeType)
        {
            string fileName = _themesDictionary[themeType].FileName;
            ResourceDictionary dict = ResourcesHelper.OpenResourceDictionary(fileName);
            return ((Brush)dict[ThemesResoucesKeyNames.UnCheckedAreaGradientBrush]);
        }

        public Brush GetThemeMainBrush(ThemeType themeType)
        {
            return _themeBrushesCache[themeType].MainBrush;
        }
        public Brush GetThemeSecondBrush(ThemeType themeType)
        {
            return _themeBrushesCache[themeType].SecondBrush;
        }
    }
}