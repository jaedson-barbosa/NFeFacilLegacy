using BibliotecaCentral.ItensBD;
using System.ComponentModel;
using BibliotecaCentral.ModeloXML;

namespace NFeFacil.ViewModel
{
    public sealed class MotoristaDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MotoristaDI Motorista { get; set; }

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
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Motorista)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            }
        }

        public int TipoDocumento { get; set; }
        public string Documento
        {
            get => Motorista.Documento;
            set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Motorista.CPF = tipo == TiposDocumento.CPF ? value : null;
                Motorista.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
            }
        }

        public MotoristaDataContext(ref MotoristaDI motorista)
        {
            Motorista = motorista;
            TipoDocumento = (int)motorista.TipoDocumento;
        }
    }
}
