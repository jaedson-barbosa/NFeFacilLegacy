using System;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class TextoComLabel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"{parameter}: {value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            return str.Substring(str.IndexOf(':') + 1);
        }
    }
}
