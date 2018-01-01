using static NFeFacil.Produto.Impostos.AssociacoesSimples;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.GerenciamentoProdutos
{
    public sealed partial class CadastroICMS : ContentDialog
    {
        public string NomeModelo { get; private set; }
        public bool EdicaoAtivada { get; private set; }
        public bool IsRegimeNormal { get; } = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
        public Page Pagina => frmManipulacao.Content as Page;

        public CadastroICMS(string cst, string csosn)
        {
            InitializeComponent();
            var page = IsRegimeNormal ? ICMSRN[int.Parse(cst)] : ICMSSN[int.Parse(csosn)];
            if (page != null) frmManipulacao.Navigate(page);
        }
    }
}
