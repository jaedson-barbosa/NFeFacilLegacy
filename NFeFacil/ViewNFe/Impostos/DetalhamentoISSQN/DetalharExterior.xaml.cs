using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoISSQN
{
    [DetalhePagina("ISSQN")]
    public sealed partial class DetalharExterior : Page, IDadosISSQN
    {
        public ISSQN Imposto { get; } = new ISSQN()
        {
            cMun = "9999999"
        };
        ObservableCollection<Municipio> MunicipiosISSQN { get; } = new ObservableCollection<Municipio>();

        Estado ufissqn;
        Estado UFISSQN
        {
            get => ufissqn;
            set
            {
                ufissqn = value;
                MunicipiosISSQN.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    MunicipiosISSQN.Add(item);
                }
            }
        }

        public DetalharExterior()
        {
            InitializeComponent();
        }
    }
}
