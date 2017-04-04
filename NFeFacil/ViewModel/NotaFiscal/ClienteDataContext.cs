using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NFeFacil.ViewModel.NotaFiscal
{
    public sealed class ClienteDataContext : INotifyPropertyChanged
    {
        public Destinatario Cliente { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void AttTudo()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cliente)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IndicadorIE)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(TipoOperação)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Municipios)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(MunicipioEscolhido)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Nacional)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(TipoDocumento)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Documento)));
        }


        public bool IsentoICMS
        {
            get => IndicadorIE != 1;
            set => IndicadorIE = value ? 2 : 1;
        }

        public int IndicadorIE
        {
            get => Cliente.indicadorIE - 1;
            set => Cliente.indicadorIE = value + 1;
        }

        public int TipoOperação => (Cliente.endereco.XPais == "Brasil") ? 0 : 1;

        public IEnumerable<Municipio> _Municipios
        {
            get
            {
                if (UFEscolhida != null)
                    return IBGE.Municipios.Get(UFEscolhida);
                else
                    return new List<Municipio>();
            }
        }

        public ObservableCollection<string> Municipios => (from mun in _Municipios select mun.Nome).GerarObs();

        public string UFEscolhida
        {
            get => Cliente.endereco.SiglaUF;
            set
            {
                Cliente.endereco.SiglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        public string MunicipioEscolhido
        {
            get
            {
                if (!Municipios.Contains(Cliente.endereco.NomeMunicipio) && Municipios.Count(x => x == Cliente.endereco.NomeMunicipio) > 0)
                    Cliente.endereco.NomeMunicipio = Municipios.First(x => x == Cliente.endereco.NomeMunicipio);
                return Cliente.endereco.NomeMunicipio;
            }
            set
            {
                if (_Municipios.Count() != 0)
                {
                    Cliente.endereco.NomeMunicipio = value;
                    Cliente.endereco.CodigoMunicipio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
                }
            }
        }

        private bool nacional = false;

        public bool Nacional
        {
            get
            {
                nacional = Cliente.endereco.XPais.ToLower() == "brasil" || string.IsNullOrEmpty(Cliente.endereco.XPais);
                return nacional;
            }
            set => nacional = value;
        }

        public int TipoDocumento { get; set; }

        public string Documento
        {
            get { return Cliente.obterDocumento; }
            set
            {
                switch ((TiposDocumento)TipoDocumento)
                {
                    case TiposDocumento.CPF:
                        Cliente.CPF = value;
                        Cliente.CNPJ = null;
                        Cliente.idEstrangeiro = null;
                        break;
                    case TiposDocumento.CNPJ:
                        Cliente.CPF = null;
                        Cliente.CNPJ = value;
                        Cliente.idEstrangeiro = null;
                        break;
                    case TiposDocumento.idEstrangeiro:
                        Cliente.CPF = null;
                        Cliente.CNPJ = null;
                        Cliente.idEstrangeiro = value;
                        break;
                }
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
