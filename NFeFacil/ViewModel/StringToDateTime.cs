using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel
{
    public sealed class StringToDateTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return targetType == typeof(DateTime) ? Propriedades.DateTimeNow : Propriedades.DateTimeOffsetNow;
            }
            if (value is string valor)
            {
                string formato = "yyyy-MM-dd";
                if (parameter is string parametro)
                {
                    formato = parametro;
                }
                if (targetType == typeof(DateTime))
                {
                    if (string.IsNullOrEmpty(valor))
                    {
                        return Propriedades.DateTimeNow;
                    }
                    else
                    {
                        return DateTime.ParseExact(valor, formato, CultureInfo.InvariantCulture);
                    }
                }
                else if (targetType == typeof(DateTimeOffset))
                {
                    if (string.IsNullOrEmpty(valor))
                    {
                        return Propriedades.DateTimeOffsetNow;
                    }
                    else
                    {
                        return DateTimeOffset.ParseExact(valor, formato, CultureInfo.InvariantCulture);
                    }
                }
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string formato = "yyyy-MM-dd";
            if (parameter is string parametro)
            {
                formato = parametro;
            }
            if (value is DateTime dataHora)
            {
                return dataHora.ToString(formato);
            }
            else if (value is DateTimeOffset dataHoraOffset)
            {
                return dataHoraOffset.ToString(formato);
            }
            throw new ArgumentException();
        }
    }
}
