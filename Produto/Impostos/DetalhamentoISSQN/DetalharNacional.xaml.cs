using BaseGeral.IBGE;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoISSQN
{
    [DetalhePagina("ISSQN")]
    public sealed partial class DetalharNacional : Page, IDadosISSQN
    {
        public ISSQN Imposto { get; } = new ISSQN();
        ObservableCollection<Municipio> MunicipiosIncid { get; } = new ObservableCollection<Municipio>();
        ObservableCollection<Municipio> MunicipiosISSQN { get; } = new ObservableCollection<Municipio>();

        Estado ufincidissqn;
        Estado UFIncidISSQN
        {
            get => ufincidissqn;
            set
            {
                ufincidissqn = value;
                MunicipiosIncid.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    MunicipiosIncid.Add(item);
                }
            }
        }

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

        public DetalharNacional()
        {
            InitializeComponent();
        }
    }
}
