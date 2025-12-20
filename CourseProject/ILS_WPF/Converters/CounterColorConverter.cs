using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ILS_WPF.Converters
{
    internal class CounterColorConverter : IValueConverter
    {
        private readonly SolidColorBrush warningBrush = new SolidColorBrush(Color.FromRgb(255,61,61));
        private readonly SolidColorBrush normalBrush = new SolidColorBrush(Color.FromRgb(0,212,146));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int)value;
            return count > 0 ? warningBrush : normalBrush;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
