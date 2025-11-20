using Microsoft.UI.Xaml.Data;

namespace UnoApp1.Converters;

/// <summary>
/// Converter để format số decimal thành chuỗi với dấu phân cách hàng nghìn
/// </summary>
public class DecimalToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue.ToString("N0"); // Format với dấu phân cách hàng nghìn, không số thập phân
        }

        if (value is double doubleValue)
        {
            return doubleValue.ToString("N0");
        }

        if (value is int intValue)
        {
            return intValue.ToString("N0");
        }

        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringValue && decimal.TryParse(stringValue.Replace(",", ""), out var result))
        {
            return result;
        }

        return 0m;
    }
}

/// <summary>
/// Converter để kiểm tra string có rỗng không và trả về Visibility
/// </summary>
public class EmptyToCollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue) 
                ? Microsoft.UI.Xaml.Visibility.Collapsed 
                : Microsoft.UI.Xaml.Visibility.Visible;
        }

        return Microsoft.UI.Xaml.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converter để convert bool thành Visibility (true = Visible)
/// </summary>
public class TrueToVisibleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue 
                ? Microsoft.UI.Xaml.Visibility.Visible 
                : Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        return Microsoft.UI.Xaml.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converter để convert bool thành Visibility (true = Collapsed)
/// </summary>
public class TrueToCollapsedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            return boolValue 
                ? Microsoft.UI.Xaml.Visibility.Collapsed 
                : Microsoft.UI.Xaml.Visibility.Visible;
        }

        return Microsoft.UI.Xaml.Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

