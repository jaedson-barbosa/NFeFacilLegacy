using BaseGeral.Controles;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    public sealed partial class CalcularTroco : ContentDialog
    {
        double ValorProduto { get; }

        public CalcularTroco(double valorProduto)
        {
            InitializeComponent();
            ValorProduto = valorProduto;
        }

        private void EntradaNumerica_NumeroChanged(EntradaNumerica sender, NumeroChangedEventArgs e)
        {
            var troco = e.NovoNumero - ValorProduto;
            if (troco < 0)
            {
                txtTroco.Text = "Valor dado muito pequeno.";
            }
            else
            {
                txtTroco.Text = troco.ToString("C2");
            }
        }
    }
}
