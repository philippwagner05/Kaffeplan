using System.Globalization;
using System.Windows.Data;

namespace Kaffeeplan.App.Converters;

public class NullToIntConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue == 0 ? string.Empty : intValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string _value = value?.ToString() ?? string.Empty;
        if (string.IsNullOrEmpty(_value))
        {
            return 0;
        }
        return int.Parse(_value);
    }
}