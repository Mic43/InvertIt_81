using System;
using System.Windows;
using System.Windows.Media;

namespace NoNameGame.CustomControls.AttachedProperties
{
    public class ThemeSecondColorExtension
    {
        public static DependencyProperty ThemeSecondColorProperty =
            DependencyProperty.RegisterAttached("ThemeSecondColor",
                typeof(Color),
                typeof(ThemeSecondColorExtension),
                new PropertyMetadata(ColorPropertyChangedCallBack));
        private static void ColorPropertyChangedCallBack(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetThemeSecondBrush(dependencyObject, new SolidColorBrush((Color)dependencyPropertyChangedEventArgs.NewValue));
            if (ThemeSecondColorChanged != null)
                ThemeSecondColorChanged(dependencyObject, EventArgs.Empty);
        }

        public static event EventHandler ThemeSecondColorChanged;

        public static DependencyProperty ThemeSecondBrushProperty =
            DependencyProperty.RegisterAttached("ThemeSecondBrush",
                typeof(Brush),
                typeof(ThemeSecondColorExtension),
                new PropertyMetadata(null));
        public static Color GetThemeSecondColor(DependencyObject target)
        {
            return (Color)target.GetValue(ThemeSecondColorProperty);
        }
        public static Brush GetThemeSecondBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(ThemeSecondBrushProperty);
        }
        public static void SetThemeSecondBrush(DependencyObject target, Brush value)
        {
            target.SetValue(ThemeSecondBrushProperty, value);
        }
        public static void SetThemeSecondColor(DependencyObject target, Color value)
        {
            target.SetValue(ThemeSecondColorProperty, value);
        }


    }
}