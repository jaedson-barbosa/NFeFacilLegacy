using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.GerenciamentoProdutos
{
    public sealed partial class CadastroIPI : ContentDialog
    {
        public ImpSimplesArmazenado.XMLIPIArmazenado Dados
        {
            get
            {
                var page = (Impostos.DetalhamentoIPI.DetalharSimples)frmPrincipal.Content;
                var ipi = page.Conjunto;
                return new ImpSimplesArmazenado.XMLIPIArmazenado
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
            frmPrincipal.Navigate(typeof(Impostos.DetalhamentoIPI.DetalharSimples));
        }
    }
}
