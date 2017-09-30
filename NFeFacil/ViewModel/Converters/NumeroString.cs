using System;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class NumeroString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(System.Object))
            {
                return value.ToString();
            }
            var retorno = System.Convert.ChangeType(value, targetType);
            if (parameter != null && retorno is string str)
            {
                return str[0] == '0' ? null : retorno;
            }
            return retorno;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(System.Object))
            {
                return value.ToString();
            }
            var retorno = System.Convert.ChangeType(value, targetType);
            if (parameter != null && retorno is string str)
            {
                return str[0] == '0' ? null : retorno;
            }
            return retorno;
        }
    }
}
