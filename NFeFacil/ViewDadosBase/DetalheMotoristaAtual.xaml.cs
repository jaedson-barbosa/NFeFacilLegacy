using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class DetalheMotoristaAtual : ContentDialog
    {
        public DetalheMotoristaAtual(MotoristaDI motorista)
        {
            InitializeComponent();
            DataContext = new MotoristaDataContext(ref motorista);
        }
    }
}
