using System;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class ModificaValor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value as string)) return 0;
            var valor = int.Parse(value as string);
            return valor += int.Parse(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrEmpty(value as string)) return 0;
            var valor = int.Parse(value as string);
            return valor -= int.Parse(parameter as string);
        }
    }
}
