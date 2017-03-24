using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel
{
    public sealed class ClienteDataContext : INotifyPropertyChanged
    {
        public Destinatário Cliente { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void AttTudo()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cliente)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsentoICMS)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(IndicadorIE)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(TipoOperação)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFs)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Municipios)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(MunicipioEscolhido)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Nacional)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(TipoDocumento)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Documento)));
        }

        [XmlIgnore]
        public bool IsentoICMS
        {
            get { return IndicadorIE != 1; }
            set { IndicadorIE = value ? 2 : 1; }
        }
        [XmlIgnore]
        public int IndicadorIE
        {
            get { return Cliente.indicadorIE - 1; }
            set { Cliente.indicadorIE = value + 1; }
        }
        [XmlIgnore]
        public int TipoOperação
        {
            get { return (Cliente.endereço.xPais == "Brasil") ? 0 : 1; }
        }

        [XmlIgnore]
        public IEnumerable<Estado> _UFs { get; } = Estados.Buscar();
        [XmlIgnore]
        public ObservableCollection<string> UFs
        {
            get
            {
                return (from uf in _UFs select uf.Sigla).GerarObs();
            }
        }
        [XmlIgnore]
        public IEnumerable<Municipio> _Municipios
        {
            get
            {
                if (UFEscolhida != null)
                    return Informacoes.IBGE.Municipios.Buscar(_UFs.First(x => x.Sigla == UFEscolhida));
                else
                    return new List<Municipio>();
            }
        }
        [XmlIgnore]
        public ObservableCollection<string> Municipios
        {
            get
            {
                return (from mun in _Municipios select mun.Nome).GerarObs();
            }
        }
        [XmlIgnore]
        public string UFEscolhida
        {
            get
            {
                return Cliente.endereço.siglaUF;
            }
            set
            {
                Cliente.endereço.siglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }
        [XmlIgnore]
        public string MunicipioEscolhido
        {
            get
            {
                if (!Municipios.Contains(Cliente.endereço.nomeMunicipio) && Municipios.Count(x => RemoverAcentuacao(x) == Cliente.endereço.nomeMunicipio) > 0)
                    Cliente.endereço.nomeMunicipio = Municipios.First(x => RemoverAcentuacao(x) == Cliente.endereço.nomeMunicipio);
                return Cliente.endereço.nomeMunicipio;
            }
            set
            {
                if (_Municipios.Count() != 0)
                {
                    Cliente.endereço.nomeMunicipio = value;
                    Cliente.endereço.codigoMunicipio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
                }
            }
        }

        private bool nacional = false;
        [XmlIgnore]
        public bool Nacional
        {
            get
            {
                nacional = Cliente.endereço.xPais.ToLower() == "brasil" || string.IsNullOrEmpty(Cliente.endereço.xPais);
                return nacional;
            }
            set
            {
                nacional = value;
            }
        }
        [XmlIgnore]
        public int TipoDocumento { get; set; }
        [XmlIgnore]
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

        public ClienteDataContext() : base() { }
        public ClienteDataContext(ref Destinatário dest)
        {
            TipoDocumento = (int)dest.obterTipoDocumento;
            Cliente = dest;
        }

        private static string RemoverAcentuacao(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(x => char.IsLetter(x) || x == ' ')
                .ToArray());
        }
    }
}
