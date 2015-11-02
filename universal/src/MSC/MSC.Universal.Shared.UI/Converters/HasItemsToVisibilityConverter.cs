using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MSC.Universal.Shared.UI.Converters
{
    public class HasItemsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            if (parameter != null && ((string) parameter) == "inverted")
            {
                return (value as Array).Length == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (value as Array).Length > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }

            if (parameter != null && ((string)parameter) == "inverted")
            {
                return (value as Array).Length == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (value as Array).Length > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}