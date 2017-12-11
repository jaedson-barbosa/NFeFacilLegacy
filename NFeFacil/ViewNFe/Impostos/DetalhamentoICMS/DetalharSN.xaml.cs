using NFeFacil.ViewNFe.Impostos;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    [DetalhePagina("ICMS - Simples nacional")]
    public sealed partial class DetalharSN : Page
    {
        bool SimplesGrupoInicio { get; set; }
        bool SimplesICMSST { get; set; }
        bool SimplesGrupoFim { get; set; }

        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }
        public int modBC { get; private set; }
        public string vBC { get; private set; }
        public string pRedBC { get; private set; }
        public string pICMS { get; private set; }
        public string vICMS { get; private set; }
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public string vBCST { get; private set; }
        public string pICMSST { get; private set; }
        public string vICMSST { get; private set; }

        public DetalharSN()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var csosn = (int)e.Parameter;
            var visibilidade = VisibilidadesSimplesNacional.Buscar(csosn);
            SimplesGrupoInicio = visibilidade.GrupoInicio;
            SimplesICMSST = visibilidade.ICMSST;
            SimplesGrupoFim = visibilidade.GrupoFim;
        }
    }
}
