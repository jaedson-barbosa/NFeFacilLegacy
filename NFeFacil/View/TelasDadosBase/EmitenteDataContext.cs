using BibliotecaCentral.IBGE;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class ConverterContextoEmitente : IValueConverter
    {
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
