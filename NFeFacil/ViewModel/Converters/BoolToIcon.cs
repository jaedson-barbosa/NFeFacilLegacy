using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class BoolToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                var valor = (bool)value;
                return valor ? Symbol.Accept : Symbol.Cancel;
            }
            else
                throw new ArgumentException($"O valor tem como tipo {value.ToString()}, ou seja, não é bool.", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
