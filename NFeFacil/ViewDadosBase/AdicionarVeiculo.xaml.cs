using BaseGeral.ItensBD;
using BaseGeral.Validacao;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class AdicionarVeiculo : ContentDialog
    {
        public VeiculoDI Item { get; }

        string RNTC
        {
            get => Item.RNTC;
            set => Item.RNTC = string.IsNullOrEmpty(value) ? null : value;
        }

        public AdicionarVeiculo(VeiculoDI item = null)
        {
            Item = item ?? new VeiculoDI();
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!new ValidarDados().ValidarTudo(true,
                (string.IsNullOrEmpty(Item.Descricao), "Informe uma breve descrição do veículo."),
                (string.IsNullOrEmpty(Item.Placa), "Informe a placa do veículo"),
                (Item.Placa.Contains("&"), "O caractere & não pode ser usado."),
                (string.IsNullOrEmpty(Item.UF), "Informe a UF da placa do veículo")))
            {
                args.Cancel = true;
            }
        }
    }
}
