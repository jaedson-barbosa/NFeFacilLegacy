using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase.GerenciamentoProdutos
{
    public sealed partial class CadastroIPI : ContentDialog
    {
        public ItensBD.Produto.ImpSimplesArmazenado.XMLIPIArmazenado Dados
        {
            get
            {
                var page = (ViewNFe.Impostos.DetalhamentoIPI.DetalharSimples)frmPrincipal.Content;
                var ipi = page.Conjunto;
                return new ItensBD.Produto.ImpSimplesArmazenado.XMLIPIArmazenado
                {
                    CEnq = ipi.cEnq,
                    ClEnq = ipi.clEnq,
                    CNPJProd = ipi.CNPJProd,
                    CSelo = ipi.cSelo,
                    QSelo = ipi.qSelo
                };
            }
        }

        public CadastroIPI()
        {
            InitializeComponent();
            frmPrincipal.Navigate(typeof(ViewNFe.Impostos.DetalhamentoIPI.DetalharSimples));
        }
    }
}
