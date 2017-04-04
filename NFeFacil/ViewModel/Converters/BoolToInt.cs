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
                int casoTrue = 1;
                if (parameter is string parametro)
                {
                    casoTrue += int.Parse(parametro);
                }
                return valor ? casoTrue : casoTrue - 1;
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
                int casoTrue = 1;
                if (parameter is string parametro)
                {
                    casoTrue += int.Parse(parametro);
                }
                return inteiro == casoTrue;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
