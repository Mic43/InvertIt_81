using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.AttachedProperties;

namespace NoNameGame.Helpers
{
    public static class Themer
    {
        public static void EnableThemesForControls(params FrameworkElement[] frameworkElements)
        {
            foreach (var frameworkElement in frameworkElements)
            {
                var binding = new Binding()
                {
                    Source = GameResources.Instance,
                    Path = new PropertyPath("UnCheckedColor")
                };
                frameworkElement.SetBinding(ThemeMainColorExtension.ThemeMainColorProperty, binding);

                binding = new Binding()
                {
                    Source = GameResources.Instance,
                    Path = new PropertyPath("CheckedColor")
                };
                frameworkElement.SetBinding(ThemeSecondColorExtension.ThemeSecondColorProperty, binding);

                binding = new Binding()
                {
                    Source = GameResources.Instance,
                    Path = new PropertyPath("AccentLightColor")
                };
                frameworkElement.SetBinding(ThemeAccentLightColorExtension.ThemeAccentLightColorProperty,binding);

                binding = new Binding()
                {
                    Source = GameResources.Instance,
                    Path = new PropertyPath("AccentDarkColor")
                };
                frameworkElement.SetBinding(ThemeAccentDarkColorExtension.ThemeAccentDarkColorProperty, binding);
            }            
        }


    }
}