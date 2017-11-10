using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    public sealed partial class EscolherTransparencia : ContentDialog
    {
        double Transparencia { get; set; }
        internal double Opacidade => 1 - (Transparencia / 100);

        public EscolherTransparencia(double opacidade)
        {
            this.InitializeComponent();
            Transparencia = (int)((1 - opacidade) * 100);
        }
    }
}
