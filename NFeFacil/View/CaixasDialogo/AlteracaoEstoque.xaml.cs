using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed partial class AlteracaoEstoque : ContentDialog
    {
        bool Positivo { get; set; } = true;
        double Valor { get; set; }
        public double ValorProcessado => Valor * (Positivo ? 1 : -1);

        public AlteracaoEstoque()
        {
            this.InitializeComponent();
        }
    }
}
