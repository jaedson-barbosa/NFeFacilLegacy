using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class BoolToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool valor)
            {
                return valor ? Symbol.Accept : Symbol.Cancel;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Symbol simbolo)
            {
                return simbolo == Symbol.Accept;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
