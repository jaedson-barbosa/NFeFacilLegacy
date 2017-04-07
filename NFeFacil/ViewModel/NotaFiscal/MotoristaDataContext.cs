using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System.ComponentModel;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class MotoristaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Motorista motorista;
        public Motorista Motorista
        {
            get => motorista;
            set
            {
                motorista = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipoDocumento)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documento)));
            }
        }

        public string UFEscolhida
        {
            get => Motorista.UF;
            set
            {
                Motorista.UF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            }
        }

        public bool IsentoICMS
        {
            get => Motorista.InscricaoEstadual == "ISENTO";
            set
            {
                Motorista.InscricaoEstadual = value ? "ISENTO" : null;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InscricaoEstadual)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            }
        }

        public string InscricaoEstadual
        {
            get => Motorista.InscricaoEstadual;
            set => Motorista.InscricaoEstadual = value;
        }

        public int TipoDocumento { get; set; }
        public string Documento
        {
            get => Motorista.Documento; set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Motorista.CPF = tipo == TiposDocumento.CPF ? value : null;
                Motorista.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
            }
        }

        public MotoristaDataContext() => Motorista = new Motorista();
        public MotoristaDataContext(ref Motorista motorista)
        {
            Motorista = motorista;
            TipoDocumento = (int)motorista.TipoDocumento;
        }
        public MotoristaDataContext(ref MotoristaDI motorista)
        {
            Motorista = motorista;
            TipoDocumento = (int)motorista.TipoDocumento;
        }
    }
}
