using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class ConverterContextoEmitente : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Emitente emitenteSimples)
            {
                return new EmitenteDataContext(emitenteSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (EmitenteDataContext)value;
            if (targetType == typeof(Emitente))
            {
                return contexto.Emit;
            }
            throw new ArgumentException();
        }
    }
}
