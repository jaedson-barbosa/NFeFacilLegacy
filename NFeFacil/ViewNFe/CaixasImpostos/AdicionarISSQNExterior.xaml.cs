using NFeFacil.IBGE;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarISSQNExterior : ContentDialog
    {
        public AdicionarISSQNExterior()
        {
            this.InitializeComponent();
        }

        public ISSQN Imposto { get; } = new ISSQN()
        {
            cMun = "9999999"
        };
        ObservableCollection<Municipio> MunicipiosISSQN { get; } = new ObservableCollection<Municipio>();

        Estado UFISSQN
        {
            set
            {
                MunicipiosISSQN.Clear();
                foreach (var item in Municipios.Get(value))
                {
                    MunicipiosISSQN.Add(item);
                }
            }
        }
    }
}
