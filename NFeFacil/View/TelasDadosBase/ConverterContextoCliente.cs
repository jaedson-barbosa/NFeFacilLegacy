using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class ConverterContextoCliente : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Destinatario clienteSimples)
            {
                return new ClienteDataContext(clienteSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (ClienteDataContext)value;
            if (targetType == typeof(Destinatario))
            {
                return contexto.Cliente;
            }
            throw new ArgumentException();
        }
    }
}
