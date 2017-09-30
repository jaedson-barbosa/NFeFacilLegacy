using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class VisualizarDetalhesCancelamento : ContentDialog
    {
        string DataEHora { get; }
        string Motivo { get; }

        public VisualizarDetalhesCancelamento(ItensBD.CancelamentoRegistroVenda item)
        {
            InitializeComponent();
            DataEHora = item.MomentoCancelamento.ToString("yyyy-MM-dd HH:mm:ss");
            Motivo = item.Motivo;
        }
    }
}
