using System;
using System.Globalization;
using System.Windows.Data;
using NoNameGame.Resources;

namespace NoNameGame.Converters
{
    public class PointsToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPerfect = (bool)value;
            return isPerfect ? AppResources.PointsToHeaderConverter_Convert_Perfect : AppResources.PointsToHeaderConverter_Convert_Good;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}