using static Venda.Impostos.AssociacoesICMS;
using Windows.UI.Xaml.Controls;
using BaseGeral;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    public sealed partial class CadastroICMS : ContentDialog
    {
        public string NomeModelo { get; private set; }
        public bool EdicaoAtivada { get; private set; }
        public bool IsRegimeNormal { get; } = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
        public readonly UserControl Pagina;

        public CadastroICMS(Impostos.DetalhamentoICMS.Detalhamento detalhamento)
        {
            InitializeComponent();
            Pagina = IsRegimeNormal
                ? GetRegimeNormal(int.Parse(detalhamento.TipoICMSRN), detalhamento)
                : GetSimplesNacional(int.Parse(detalhamento.TipoICMSSN), detalhamento);
            if (Pagina != null) ctnManipulacao.Content = Pagina;
        }
    }
}
