using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BaseGeral.View
{
    public sealed class MmToPx : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valor = double.Parse((string)parameter);
            valor /= 10;
            var valorPx = ExtensoesPrincipal.CMToPixel(valor);
            if (targetType == typeof(double)) return valorPx;
            else if (targetType == typeof(Thickness)) return new Thickness(valorPx);
            else if (targetType == typeof(GridLength)) return new GridLength(valorPx);
            else throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
