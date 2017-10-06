using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarICMSSN : ContentDialog
    {
        public AdicionarICMSSN(int csosn)
        {
            InitializeComponent();

            var visibilidade = VisibilidadesSimplesNacional.Buscar(csosn);
            SimplesGrupoInicio = visibilidade.GrupoInicio;
            SimplesICMSST = visibilidade.ICMSST;
            SimplesGrupoFim = visibilidade.GrupoFim;
        }

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
    }
}
