using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Infrastructure;
using Infrastructure.Storage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using NoNameGame.Configuration;
using NoNameGame.Configuration.Themes;
using NoNameGame.CustomControls;
using NoNameGame.Helpers;
using Color = System.Windows.Media.Color;

namespace NoNameGame.Controllers.Themes
{  
    public class ThemeController
    {
        private readonly ThemesDictionary _themesDictionary;

        public Theme CurrentTheme
        {
            get { return _themesDictionary[AppSettingsAccessor.GetValueOrDefault("SelectedThemeName", ThemesDictionary.DefaultTheme)]; }
        }
        public bool AreThemesInverted
        {
            get
            {
                return AppSettingsAccessor.GetValueOrDefault("IsThemeInverted", false);
            }
        }
        private void InternalChangeTheme()
        {
            var resourceDictionary = ResourcesHelper.OpenResourceDictionary(CurrentTheme.FileName);
            Application.Current.Resources.MergedDictionaries.Last().MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Last().MergedDictionaries.Add(resourceDictionary);
            GameResources.Instance.RefreshAfterThemeChange();
            ThemeManager.SetAccentColor(GameResources.Instance.CheckedColor);
            
         
        }
        public ThemeController(ThemesDictionary themesDictionary)
        {
            _themesDictionary = themesDictionary;

            ChangeTheme(CurrentTheme.ThemeType);
            InvertThemes(AreThemesInverted);

            ThemeManager.SetAccentColor(GameResources.Instance.CheckedColor);
            ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.SystemTrayColors;
            ThemeManager.ToLightTheme();

          
            //var fontFamily = new FontFamily("/Assets/Bellota-Regular.otf#Bellota");
//            var fontFamily = new FontFamily("/Assets/Sintony-Regular.otf#Sintony");
            // var fontFamily = new FontFamily("/Assets/gnuolane rg.ttf#gnuolane Rg");
//            var fontFamily = new FontFamily("/Assets/Ballham.ttf#Ballham");
            //var fontFamily = new FontFamily("/Assets/Manteka.ttf#Manteka");
            // var fontFamily = new FontFamily("/Assets/Roboto-Regular.ttf#Roboto");  





//            SetterBaseCollection setterBaseCollection = (App.Current.Resources["PhoneTextBlockBase"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextLargeStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextNormalStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextSubtleStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextTitle1Style"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextTitle3Style"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextTitle2Style"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextExtraLargeStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextGroupHeaderStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));
//
//
//            setterBaseCollection = (App.Current.Resources["PhoneTextHugeStyle"] as Style).Setters;
//            setterBaseCollection.Add(new Setter(TextBlock.FontFamilyProperty, fontFamily));

            //Application.Current.Resources;

            //ThemeManager.SetAccentColor(Colors.Black);
            //  ThemeManager.SetCustomTheme();
        }

        public void ChangeTheme(ThemeType newThemeType)
        {
            if (!_themesDictionary.ContainsKey(newThemeType))
                throw new ArgumentException("Invalid theme, it must be registred with ThemesDictionary", "theme");
          
            AppSettingsAccessor.AddOrUpdateValue("SelectedThemeName", newThemeType);
            AppSettingsAccessor.Save();  

            InternalChangeTheme();          
        }
        public void InvertThemes(bool isThemeInverted)
        {
            AppSettingsAccessor.AddOrUpdateValue("IsThemeInverted", isThemeInverted);
            AppSettingsAccessor.Save();

            GameResources.Instance.InvertColors = isThemeInverted;
        }
        public IEnumerable<Theme> GetAllThemes()
        {
            return _themesDictionary.Values;
        }
   
    }
}