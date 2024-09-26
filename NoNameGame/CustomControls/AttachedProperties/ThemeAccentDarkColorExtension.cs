using System;
using System.Windows;
using System.Windows.Media;

namespace NoNameGame.CustomControls.AttachedProperties
{
    public class ThemeAccentDarkColorExtension
    {
        public static DependencyProperty ThemeAccentDarkColorProperty =
        DependencyProperty.RegisterAttached("ThemeAccentDarkColor",
                                          typeof(Color),
                                          typeof(ThemeAccentDarkColorExtension),
                                          new PropertyMetadata(ColorPropertyChangedCallBack));
        private static void ColorPropertyChangedCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetThemeAccentDarkBrush(dependencyObject,new SolidColorBrush((Color)dependencyPropertyChangedEventArgs.NewValue));
            if (ThemeAccentDarkColorChanged != null)
                ThemeAccentDarkColorChanged(dependencyObject,EventArgs.Empty);
        }

        public static event EventHandler ThemeAccentDarkColorChanged;

        public static DependencyProperty ThemeAccentDarkBrushProperty =
        DependencyProperty.RegisterAttached("ThemeAccentDarkBrush",
                                       typeof(Brush),
                                       typeof(ThemeAccentDarkColorExtension),
                                       new PropertyMetadata(null));
        public static Color GetThemeAccentDarkColor(DependencyObject target)
        {
            return (Color)target.GetValue(ThemeAccentDarkColorProperty);
        }
        public static Brush GetThemeAccentDarkBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(ThemeAccentDarkBrushProperty);
        }
        public static void SetThemeAccentDarkBrush(DependencyObject target,Brush value)
        {
            target.SetValue(ThemeAccentDarkBrushProperty, value);
        }
        public static void SetThemeAccentDarkColor(DependencyObject target, Color value)
        {
            target.SetValue(ThemeAccentDarkColorProperty, value);
        }    

    }
}