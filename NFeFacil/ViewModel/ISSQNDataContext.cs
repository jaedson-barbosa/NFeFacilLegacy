using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace NFeFacil.ViewModel
{
    public class ISSQNDataContext : INotifyPropertyChanged, IImpostoDataContext
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public ISSQN Imposto { get; } = new ISSQN();

        public int exigISS
        {
            get
            {
                return int.Parse(Imposto.indISS != null ? Imposto.indISS : "0") - 1;
            }
            set
            {
                Imposto.indISS = (value + 1).ToString();
            }
        }

        public int incent
        {
            get
            {
                return int.Parse(Imposto.indIncentivo != null ? Imposto.indIncentivo : "0") - 1;
            }
            set
            {
                Imposto.indIncentivo = (value + 1).ToString();
            }
        }

        private IEnumerable<Estado> _UFs = Estados.Buscar();
        public ObservableCollection<Estado> UFs
        {
            get
            {
                return new ObservableCollection<Estado>(_UFs);
            }
        }

        private Estado ufEscolhida;
        public Estado UFEscolhida
        {
            get
            {
                return ufEscolhida;
            }
            set
            {
                ufEscolhida = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Municipios)));
            }
        }

        private Estado ufIncidEscolhida;
        public Estado UFIncidEscolhida
        {
            get { return ufIncidEscolhida; }
            set
            {
                ufIncidEscolhida = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MunicipiosIncid)));
            }
        }

        public ObservableCollection<Municipio> Municipios
        {
            get
            {
                if (UFEscolhida != null)
                    return IBGE.Municipios.Buscar(UFEscolhida).GerarObs();
                return new ObservableCollection<Municipio>();
            }
        }
        public ObservableCollection<Municipio> MunicipiosIncid
        {
            get
            {
                if (UFIncidEscolhida != null)
                    return IBGE.Municipios.Buscar(UFIncidEscolhida).GerarObs();
                return new ObservableCollection<Municipio>();
            }
        }

        public Municipio MunicipioEscolhido
        {
            get
            {
                var nulo = string.IsNullOrEmpty(Imposto.cMunFG);
                return !nulo ? Municipios.Single(x => x.CodigoMunicípio.ToString() == Imposto.cMunFG) : null;
            }
            set
            {
                Imposto.cMunFG = value.CodigoMunicípio.ToString();
            }
        }

        public Municipio MunicipioIncidEscolhido
        {
            get
            {
                var nulo = string.IsNullOrEmpty(Imposto.cMun);
                return !nulo ? Municipios.Single(x => x.CodigoMunicípio.ToString() == Imposto.cMun) : null;
            }
            set
            {
                Imposto.cMun = value.CodigoMunicípio.ToString();
            }
        }

        private bool exterior;
        public bool Exterior
        {
            get { return exterior; }
            set
            {
                exterior = value;
                if (value)
                {
                    VisibilidadeCodigoPais = Visibility.Visible;
                    VisibilidadeMunicipioUFIncidencia = Visibility.Collapsed;
                    Imposto.cMun = "9999999";
                }
                else
                {
                    VisibilidadeCodigoPais = Visibility.Collapsed;
                    VisibilidadeMunicipioUFIncidencia = Visibility.Visible;
                    Imposto.cPais = null;
                    Imposto.cMun = null;
                }
                OnPropertyChanged(nameof(VisibilidadeCodigoPais), nameof(VisibilidadeMunicipioUFIncidencia));
            }
        }

        public Visibility VisibilidadeCodigoPais { get; private set; } = Visibility.Collapsed;
        public Visibility VisibilidadeMunicipioUFIncidencia { get; private set; }

        public Imposto ImpostoBruto => Imposto;
    }
}
