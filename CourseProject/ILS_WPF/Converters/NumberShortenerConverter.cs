using System.Globalization;
using System.Windows.Data;

namespace ILS_WPF.Converters
{
    internal class NumberShortenerConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int num)
            {
                return num switch
                {
                    >= 1000000 => (((float)num) / 1000000).ToString() + " млн.",
                    >= 1000 => (((float)num) / 1000).ToString() + " тыс.",
                    _ => num
                };
            }
            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
