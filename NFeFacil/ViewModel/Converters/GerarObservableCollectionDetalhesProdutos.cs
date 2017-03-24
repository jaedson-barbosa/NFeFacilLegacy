using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace NFeFacil.ViewModel.Converters
{
    public sealed class GerarObservableCollectionDetalhesProdutos : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as List<DetalhesProdutos>).GerarObs();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
