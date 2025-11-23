using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ILS_WPF.Converters
{
    internal class FullNameAbbreviationConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return value;
            string[] fullNameParts = ((string)value).Split();
            return $"{fullNameParts[0]} {fullNameParts[1][0]}.{fullNameParts[2][0]}.";
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
