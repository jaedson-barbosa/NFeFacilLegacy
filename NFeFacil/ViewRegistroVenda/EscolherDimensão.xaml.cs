using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class EscolherDimensão : ContentDialog
    {
        public bool FormularioContinuo { get; set; }
        public double Altura { get; set; }
        public double Largura { get; set; }

        public EscolherDimensão()
        {
            this.InitializeComponent();
        }
    }
}
