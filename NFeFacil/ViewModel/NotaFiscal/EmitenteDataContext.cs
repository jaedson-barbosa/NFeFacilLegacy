using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class EmitenteDataContext : INotifyPropertyChanged
    {
        private Emitente emit;

        public event PropertyChangedEventHandler PropertyChanged;

        public Emitente Emit
        {
            get => emit;
            set
            {
                emit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
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

        public EmitenteDataContext() => Emit = new Emitente();
        public EmitenteDataContext(ref Emitente emit) => Emit = emit;
        public EmitenteDataContext(ref EmitenteDI emit) => Emit = emit;
    }
}
