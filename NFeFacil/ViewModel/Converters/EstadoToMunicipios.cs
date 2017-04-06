using NFeFacil.IBGE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class EstadoToMunicipios : IValueConverter
    {
        private int? tamanhoString;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                tamanhoString = null;
                return new ObservableCollection<Municipio>();
            }
            if (value is string str)
            {
                tamanhoString = str.Length;
                return Municipios.Get(str).GerarObs();
            }
            else if (value is Estado est)
            {
                tamanhoString = null;
                return Municipios.Get(est).GerarObs();
            }
            else
            {
                tamanhoString = null;
                throw new ArgumentException("Tipo não cadastrado.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<Municipio> municipios)
            {
                if (targetType == typeof(string))
                {
                    var estado = Estados.EstadosCache.First(x => x.Codigo == municipios.First().CodigoUF);
                    return tamanhoString == 2 ? estado.Sigla : estado.Nome;
                }
                else if (targetType == typeof(Estado))
                {
                    return Estados.EstadosCache.First(x => x.Codigo == municipios.First().CodigoUF);
                }
            }
            throw new ArgumentException();
        }
    }
}
