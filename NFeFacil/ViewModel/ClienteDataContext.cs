using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.IBGE;

namespace NFeFacil.ViewModel
{
    public sealed class ClienteDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ClienteDI Cliente { get; set; }

        public ObservableCollection<IndicadorIE> IndicadoresIE => NFeFacil.ExtensoesPrincipal.ObterItens<IndicadorIE>();
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
                        if (Cliente.InscricaoEstadual == null) InscricaoEstadual = string.Empty;
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

        public bool IsentoICMS { get; private set; }
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

        private bool nacional = true;
        public bool Nacional
        {
            get
            {
                var xpais = Cliente.XPais;
                if (string.IsNullOrEmpty(xpais))
                {
                    Cliente.XPais = "BRASIL";
                    Cliente.CPais = 1058;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cliente)));
                }
                else
                {
                    nacional = Cliente.CPais == 1058;
                }
                return nacional;
            }
            set
            {
                nacional = value;
                if (value)
                {
                    Cliente.XPais = "BRASIL";
                    Cliente.CPais = 1058;
                }
                else
                {
                    Cliente.XPais = string.Empty;
                    Cliente.CPais = 0;
                    Cliente.CEP = Cliente.SiglaUF = null;
                    ConjuntoMunicipio = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConjuntoMunicipio)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cliente)));
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
