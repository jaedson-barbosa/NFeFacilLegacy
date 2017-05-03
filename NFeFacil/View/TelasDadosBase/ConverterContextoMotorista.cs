using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class ConverterContextoMotorista : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Motorista motoristaSimples)
            {
                return new MotoristaDataContext(motoristaSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (MotoristaDataContext)value;
            if (targetType == typeof(Motorista))
            {
                return contexto.Motorista;
            }
            throw new ArgumentException();
        }
    }
}
