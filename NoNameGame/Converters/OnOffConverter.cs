using System;
using System.Globalization;
using System.Windows.Data;
using NoNameGame.Resources;

namespace NoNameGame.Converters
{
    
    public class OnOffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool) value;
            if (val)
                return AppResources.OnOffConverter_Convert_On;
            else
                return AppResources.OnOffConverter_Convert_Off;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}