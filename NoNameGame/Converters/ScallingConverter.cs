using System;
using System.Globalization;
using System.Windows.Data;

namespace NoNameGame.Converters
{
    public class ScallingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueToScale = (double)parameter;
            double scale = (double) value;

            return valueToScale*scale;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueToScale = (double)parameter;
            double scalledValue = (double)value;

            return scalledValue/valueToScale;
        }
    }
}