using System;
using Windows.UI.Xaml.Data;

namespace BaseGeral.View
{
    public sealed class MascaraDocumento : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var original = (string)value;
            return ExtensoesPrincipal.AplicarMáscaraDocumento(original);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
