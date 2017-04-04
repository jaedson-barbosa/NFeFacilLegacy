using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool valor)
            {
                return valor ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibilidade)
            {
                return visibilidade == Visibility.Visible;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
