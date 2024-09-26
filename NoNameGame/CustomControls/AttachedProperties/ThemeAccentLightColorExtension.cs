using System;
using System.Windows;
using System.Windows.Media;

namespace NoNameGame.CustomControls.AttachedProperties
{
    public class ThemeAccentLightColorExtension
    {
        public static DependencyProperty ThemeAccentLightColorProperty =
        DependencyProperty.RegisterAttached("ThemeAccentLightColor",
                                          typeof(Color),
                                          typeof(ThemeAccentLightColorExtension),
                                          new PropertyMetadata(ColorPropertyChangedCallBack));
        private static void ColorPropertyChangedCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetThemeAccentLightBrush(dependencyObject,new SolidColorBrush((Color)dependencyPropertyChangedEventArgs.NewValue));
            if (ThemeAccentLightColorChanged != null)
                ThemeAccentLightColorChanged(dependencyObject,EventArgs.Empty);
        }

        public static event EventHandler ThemeAccentLightColorChanged;

        public static DependencyProperty ThemeAccentLightBrushProperty =
        DependencyProperty.RegisterAttached("ThemeAccentLightBrush",
                                       typeof(Brush),
                                       typeof(ThemeAccentLightColorExtension),
                                       new PropertyMetadata(null));
        public static Color GetThemeAccentLightColor(DependencyObject target)
        {
            return (Color)target.GetValue(ThemeAccentLightColorProperty);
        }
        public static Brush GetThemeAccentLightBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(ThemeAccentLightBrushProperty);
        }
        public static void SetThemeAccentLightBrush(DependencyObject target,Brush value)
        {
            target.SetValue(ThemeAccentLightBrushProperty, value);
        }
        public static void SetThemeAccentLightColor(DependencyObject target, Color value)
        {
            target.SetValue(ThemeAccentLightColorProperty, value);
        }    

    }
}