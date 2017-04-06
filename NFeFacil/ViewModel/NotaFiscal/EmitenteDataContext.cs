using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class EmitenteDataContext : INotifyPropertyChanged
    {
        public Emitente Emit { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void AttTudo()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Emit)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ConjuntoMunicipio)));
        }

        public string EstadoSelecionado
        {
            get => Emit.endereco.SiglaUF;
            set
            {
                Emit.endereco.SiglaUF = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
            }
        }

        public Municipio ConjuntoMunicipio
        {
            get
            {
                if (Emit.endereco.CodigoMunicipio > 0)
                {
                    return Municipios.Get(EstadoSelecionado).First(x => x.CodigoMunicípio == Emit.endereco.CodigoMunicipio);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Emit.endereco.NomeMunicipio = value?.Nome;
                Emit.endereco.CodigoMunicipio = value?.CodigoMunicípio ?? 0;
            }
        }

        public EmitenteDataContext() => Emit = new Emitente();
        public EmitenteDataContext(ref Emitente emit) => Emit = emit;
        public EmitenteDataContext(ref EmitenteDI emit) => Emit = emit;
    }
}
