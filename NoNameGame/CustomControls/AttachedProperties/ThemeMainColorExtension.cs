using System;
using System.Windows;
using System.Windows.Media;

namespace NoNameGame.CustomControls.AttachedProperties
{
    public class ThemeMainColorExtension
    {
        public static DependencyProperty ThemeMainColorProperty =
        DependencyProperty.RegisterAttached("ThemeMainColor",
                                          typeof(Color),
                                          typeof(ThemeMainColorExtension),
                                          new PropertyMetadata(ColorPropertyChangedCallBack));
        private static void ColorPropertyChangedCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetThemeMainBrush(dependencyObject,new SolidColorBrush((Color)dependencyPropertyChangedEventArgs.NewValue));
            if (ThemeMainColorChanged != null)
                ThemeMainColorChanged(dependencyObject,EventArgs.Empty);
        }

        public static event EventHandler ThemeMainColorChanged;

        public static DependencyProperty ThemeMainBrushProperty =
        DependencyProperty.RegisterAttached("ThemeMainBrush",
                                       typeof(Brush),
                                       typeof(ThemeMainColorExtension),
                                       new PropertyMetadata(null));
        public static Color GetThemeMainColor(DependencyObject target)
        {
            return (Color)target.GetValue(ThemeMainColorProperty);
        }
        public static Brush GetThemeMainBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(ThemeMainBrushProperty);
        }
        public static void SetThemeMainBrush(DependencyObject target,Brush value)
        {
            target.SetValue(ThemeMainBrushProperty, value);
        }
        public static void SetThemeMainColor(DependencyObject target, Color value)
        {
            target.SetValue(ThemeMainColorProperty, value);
        }    

    }
}