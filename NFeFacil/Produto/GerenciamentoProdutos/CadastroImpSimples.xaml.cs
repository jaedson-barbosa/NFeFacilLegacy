using NFeFacil.Produto.Impostos;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.GerenciamentoProdutos
{
    public sealed partial class CadastroImpSimples : ContentDialog
    {
        public string NomeModelo { get; private set; }
        public bool EdicaoAtivada { get; private set; }

        public double Aliquota { get; private set; }
        public double Valor { get; private set; }

        bool UsarAliquota { get; }
        bool UsarValor { get; }

        public CadastroImpSimples(TiposCalculo tipo)
        {
            InitializeComponent();
            UsarAliquota = tipo == TiposCalculo.PorAliquota;
            UsarValor = tipo == TiposCalculo.PorValor;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = string.IsNullOrEmpty(NomeModelo);
        }
    }
}
