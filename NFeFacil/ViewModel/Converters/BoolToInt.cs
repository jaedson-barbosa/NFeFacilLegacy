using System;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public class BoolToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool valor)
            {
                return valor ? 1 : 0;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int inteiro)
            {
                return inteiro == 1;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
