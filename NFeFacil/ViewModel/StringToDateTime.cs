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
                return targetType == typeof(DateTime) ? DefinicoesTemporarias.DateTimeNow : DefinicoesTemporarias.DateTimeOffsetNow;
            }
            else if (value is string valor)
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
                        return DefinicoesTemporarias.DateTimeNow;
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
                        return DefinicoesTemporarias.DateTimeOffsetNow;
                    }
                    else
                    {
                        return DateTimeOffset.ParseExact(valor, formato, CultureInfo.InvariantCulture);
                    }
                }
            }
            else if (value is DateTime dataHora)
            {
                return dataHora.ToString((string)parameter);
            }
            else if (value is DateTimeOffset dataHoraOffset)
            {
                return dataHoraOffset.ToString((string)parameter);
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
            else if (value is DateTime dataHora)
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
