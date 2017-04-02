using NFeFacil.IBGE;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class EstadoToMunicipios : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return new ObservableCollection<Municipio>();
            }
            if (value is string str)
            {
                return Municipios.Get(str);
            }
            else if (value is Estado est)
            {
                return Municipios.Get(est);
            }
            else
            {
                throw new ArgumentException("Tipo não cadastrado.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
