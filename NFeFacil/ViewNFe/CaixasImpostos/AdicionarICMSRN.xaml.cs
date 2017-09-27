using NFeFacil.ViewModel.ImpostosProduto;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarICMSRN : ContentDialog
    {
        public AdicionarICMSRN(int cst)
        {
            this.InitializeComponent();

            var visibilidade = VisibilidadesRegimeNormal.Buscar(cst);
            NormalICMSDesonerado = visibilidade.IcmsDeson;
            NormalGrupoInicio = visibilidade.GrupoInicio;
            NormalPercentualReducaoNormal = visibilidade.PRedBC;
            NormalICMSSTNormal = visibilidade.ICMSST;
            NormalGrupoMeio = visibilidade.GrupoMeio;
            NormalGrupoFim = visibilidade.GrupoFim;
            NormalICMSST = visibilidade.NormalICMSST;
            NormalICMSPart = visibilidade.NormalICMSPart;
        }

        bool NormalICMSDesonerado { get; set; }
        bool NormalGrupoInicio { get; set; }
        bool NormalPercentualReducaoNormal { get; set; }
        bool NormalICMSSTNormal { get; set; }
        bool NormalGrupoMeio { get; set; }
        bool NormalGrupoFim { get; set; }
        bool NormalICMSST { get; set; }
        bool NormalICMSPart { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }
        public string vBCSTDest { get; set; }
        public string vICMSSTDest { get; set; }
        public string vICMSOp { get; set; }
        public string pDif { get; set; }
        public string vICMSDif { get; set; }
        public string pBCOp { get; set; }
        public string UFST { get; set; }
        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }
    }
}
