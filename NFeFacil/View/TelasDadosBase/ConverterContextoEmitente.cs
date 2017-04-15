using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed class ConverterContextoEmitente : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is EmitenteDI emitente)
            {
                return new EmitenteDataContext(emitente);
            }
            else if (value is Emitente emitenteSimples)
            {
                return new EmitenteDataContext(emitenteSimples);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var contexto = (EmitenteDataContext)value;
            if (targetType == typeof(EmitenteDI))
            {
                return contexto.Emit as EmitenteDI;
            }
            else if (targetType == typeof(Emitente))
            {
                return contexto.Emit;
            }
            throw new ArgumentException();
        }

        private sealed class EmitenteDataContext : INotifyPropertyChanged
        {
            public Emitente Emit { get; set; }

            public string EstadoSelecionado
            {
                get => Emit.endereco.SiglaUF;
                set
                {
                    Emit.endereco.SiglaUF = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
                }
            }

            public Municipio ConjuntoMunicipio
            {
                get => Municipios.Get(Emit.endereco.SiglaUF).FirstOrDefault(x => x.Codigo == Emit.endereco.CodigoMunicipio);
                set
                {
                    Emit.endereco.NomeMunicipio = value?.Nome;
                    Emit.endereco.CodigoMunicipio = value?.Codigo ?? 0;
                }
            }

            public EmitenteDataContext(Emitente emit) => Emit = emit;

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
