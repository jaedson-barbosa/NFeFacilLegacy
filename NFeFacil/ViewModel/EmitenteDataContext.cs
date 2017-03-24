using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace NFeFacil.ViewModel
{
    public sealed class EmitenteDataContext : INotifyPropertyChanged
    {
        public Emitente Emit { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void AttTudo()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Emit)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFs)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Municipios)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UFEscolhida)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(MunicipioEscolhido)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(RegimeTributario)));
        }

        [XmlIgnore]
        public IEnumerable<Estado> _UFs { get; } = Estados.Buscar();

        [XmlIgnore]
        public ObservableCollection<string> UFs
        {
            get
            {
                return (from uf in _UFs
                        select uf.Sigla).GerarObs();
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
                return (from mun in _Municipios
                        select mun.Nome).GerarObs();
            }
        }

        [XmlIgnore]
        public string UFEscolhida
        {
            get
            {
                return Emit.endereço.siglaUF;
            }
            set
            {
                Emit.endereço.siglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        [XmlIgnore]
        public string MunicipioEscolhido
        {
            get
            {
                if (Emit.endereço.nomeMunicipio != null)
                    return _Municipios.First(x => x.Nome == Emit.endereço.nomeMunicipio).Nome;
                else
                    return null;
            }
            set
            {
                if (_Municipios.Count() != 0)
                {
                    Emit.endereço.nomeMunicipio = value;
                    Emit.endereço.codigoMunicipio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
                }
            }
        }

        [XmlIgnore]
        public int RegimeTributario
        {
            get { return Emit.regimeTributario - 1; }
            set { Emit.regimeTributario = value + 1; }
        }

        public EmitenteDataContext() : base() { }
        public EmitenteDataContext(ref Emitente emit)
        {
            Emit = emit;
        }
    }
}
