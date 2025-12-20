using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ILS_WPF.Converters
{
    internal class EnumLocaleConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is null ? null : Application.Current.FindResource(value.ToString());

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
