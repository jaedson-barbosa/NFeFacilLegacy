using BibliotecaCentral.IBGE;
using BibliotecaCentral.ItensBD;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class EmitenteDataContext : INotifyPropertyChanged
    {
        public EmitenteDI Emit { get; set; }

        public string EstadoSelecionado
        {
            get => Emit.SiglaUF;
            set
            {
                Emit.SiglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
            }
        }

        public Municipio ConjuntoMunicipio
        {
            get => Municipios.Get(Emit.SiglaUF).FirstOrDefault(x => x.Codigo == Emit.CodigoMunicipio);
            set
            {
                Emit.NomeMunicipio = value?.Nome;
                Emit.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        public EmitenteDataContext(ref EmitenteDI emit) => Emit = emit;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
