using BibliotecaCentral.ItensBD;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BibliotecaCentral.IBGE;
using System.Linq;
using BibliotecaCentral.ModeloXML;

namespace NFeFacil.ViewModel
{
    public sealed class ClienteDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ClienteDI Cliente { get; set; }

        public ObservableCollection<IndicadorIE> IndicadoresIE => BibliotecaCentral.Extensoes.ObterItens<IndicadorIE>();
        public IndicadorIE IndicadorIESelecionado
        {
            get => (IndicadorIE)Cliente.IndicadorIE;
            set
            {
                Cliente.IndicadorIE = (int)value;
                switch (value)
                {
                    case IndicadorIE.Contribuinte:
                        IsentoICMS = false;
                        Cliente.InscricaoEstadual = string.Empty;
                        break;
                    case IndicadorIE.Isento:
                        IsentoICMS = true;
                        Cliente.InscricaoEstadual = null;
                        break;
                    case IndicadorIE.Não_Contribuinte:
                        IsentoICMS = true;
                        Cliente.InscricaoEstadual = null;
                        break;
                }
                PropertyChanged(this, new PropertyChangedEventArgs("IsentoICMS"));
                PropertyChanged(this, new PropertyChangedEventArgs("InscricaoEstadual"));
            }
        }

        public bool IsentoICMS { get; set; }
        public string InscricaoEstadual
        {
            get => Cliente.InscricaoEstadual;
            set => Cliente.InscricaoEstadual = value;
        }

        public string UFEscolhida
        {
            get => Cliente.SiglaUF;
            set
            {
                Cliente.SiglaUF = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            }
        }

        public Municipio ConjuntoMunicipio
        {
            get
            {
                var mun = Municipios.Get(Cliente.SiglaUF).FirstOrDefault(x => x.Codigo == Cliente.CodigoMunicipio);
                return mun;
            }
            set
            {
                Cliente.NomeMunicipio = value?.Nome;
                Cliente.CodigoMunicipio = value?.Codigo ?? 0;
            }
        }

        private bool? nacional = null;
        public bool Nacional
        {
            get
            {
                if (nacional == null)
                {
                    var xpais = Cliente.XPais;
                    nacional = xpais.ToLower() == "brasil" || string.IsNullOrEmpty(xpais);
                }
                if (!nacional.Value)
                {
                    Cliente.CEP = Cliente.SiglaUF = null;
                    ConjuntoMunicipio = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cliente)));
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
            get { return Cliente.Documento; }
            set
            {
                var tipo = (TiposDocumento)TipoDocumento;
                Cliente.CPF = tipo == TiposDocumento.CPF ? value : null;
                Cliente.CNPJ = tipo == TiposDocumento.CNPJ ? value : null;
                Cliente.IdEstrangeiro = tipo == TiposDocumento.idEstrangeiro ? value : null;
            }
        }

        public ClienteDataContext(ref ClienteDI dest)
        {
            TipoDocumento = (int)dest.TipoDocumento;
            Cliente = dest;
        }

        public enum IndicadorIE
        {
            Contribuinte = 1,
            Isento = 2,
            Não_Contribuinte = 9
        }
    }
}
