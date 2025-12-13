using System.Globalization;
using System.Windows.Data;

namespace ILS_WPF.Converters
{
    internal class NumberShortenerConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float num = System.Convert.ToSingle(value);
            return num switch
            {
                >= 1000000 => (num / 1000000).ToString() + " млн.",
                >= 1000 => (num / 1000).ToString() + " тыс.",
                _ => num
            };
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
