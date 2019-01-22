using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace BaseGeral.View
{
    public sealed class BoolToColor : IValueConverter
    {
        static readonly Brush Ativo = new SolidColorBrush(new BibliotecaCores().Cor1);
        static readonly Brush Inativo = new SolidColorBrush(Windows.UI.Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleano = (bool)value;
            return booleano ? Ativo : Inativo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
