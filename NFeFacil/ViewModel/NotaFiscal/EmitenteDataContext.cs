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
                    return IBGE.Municipios.Buscar(_UFs.First(x => x.Sigla == UFEscolhida));
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
                return Emit.endereco.SiglaUF;
            }
            set
            {
                Emit.endereco.SiglaUF = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        [XmlIgnore]
        public string MunicipioEscolhido
        {
            get
            {
                if (Emit.endereco.NomeMunicipio != null)
                    return _Municipios.First(x => x.Nome == Emit.endereco.NomeMunicipio).Nome;
                else
                    return null;
            }
            set
            {
                if (_Municipios.Count() != 0)
                {
                    Emit.endereco.NomeMunicipio = value;
                    Emit.endereco.CodigoMunicipio = _Municipios.First(x => x.Nome == value).CodigoMunicípio;
                }
            }
        }

        [XmlIgnore]
        public int RegimeTributario
        {
            get { return Emit.regimeTributario - 1; }
            set { Emit.regimeTributario = value + 1; }
        }

        public EmitenteDataContext() => Emit = new Emitente();
        public EmitenteDataContext(ref Emitente emit) => Emit = emit;
        public EmitenteDataContext(ref EmitenteDI emit) => Emit = emit;
    }
}
