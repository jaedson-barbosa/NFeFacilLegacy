using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class ClienteDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Destinatario cliente;
        public Destinatario Cliente
        {
            get => cliente;
            set
            {
                cliente = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cliente)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InscricaoEstadual)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CEP)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConjuntoMunicipio)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nacional)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TipoDocumento)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documento)));
            }
        }

        public bool IsentoICMS
        {
            get => Cliente.indicadorIE != 1;
            set
            {
                Cliente.indicadorIE = value ? 2 : 1;
                Cliente.inscricaoEstadual = value ? "ISENTO" : null;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InscricaoEstadual)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            }
        }

        public string InscricaoEstadual
        {
            get => Cliente.inscricaoEstadual;
            set => Cliente.inscricaoEstadual = value;
        }

        public string CEP
        {
            get => Cliente.endereco.CEP;
            set => Cliente.endereco.CEP = value;
        }

        public string EstadoSelecionado
        {
            get => Cliente.endereco.SiglaUF;
            set
            {
                Cliente.endereco.SiglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
            }
        }

        public Municipio ConjuntoMunicipio
        {
            get => Municipios.Get(EstadoSelecionado).FirstOrDefault(x => x.Codigo == Cliente.endereco.CodigoMunicipio);
            set
            {
                Cliente.endereco.NomeMunicipio = value?.Nome;
                Cliente.endereco.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        private bool? nacional = null;
        public bool Nacional
        {
            get
            {
                if (nacional == null)
                {
                    var xpais = Cliente.endereco.XPais;
                    nacional = xpais.ToLower() == "brasil" || string.IsNullOrEmpty(xpais);
                }
                if (!nacional.Value)
                {
                    CEP = EstadoSelecionado = null;
                    ConjuntoMunicipio = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CEP)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstadoSelecionado)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConjuntoMunicipio)));
                }
                return nacional.Value;
            }
            set
            {
                nacional = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nacional)));
            }
        }

        public int TipoDocumento { get; set; }

        public string Documento
        {
            get { return Cliente.obterDocumento; }
            set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Cliente.CPF = tipo == TiposDocumento.CPF ? value : null;
                Cliente.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
                Cliente.idEstrangeiro = tipo == TiposDocumento.idEstrangeiro ? value : null;
            }
        }

        public ClienteDataContext() => Cliente = new Destinatario();
        public ClienteDataContext(ref Destinatario dest)
        {
            TipoDocumento = (int)dest.obterTipoDocumento;
            Cliente = dest;
        }
        public ClienteDataContext(ref ClienteDI dest)
        {
            TipoDocumento = (int)dest.obterTipoDocumento;
            Cliente = dest;
        }
    }
}
