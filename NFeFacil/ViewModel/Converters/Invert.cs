using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class Invert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool booleano)
            {
                return !booleano;
            }
            if (value is Visibility visibilidade)
            {
                return visibilidade == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
