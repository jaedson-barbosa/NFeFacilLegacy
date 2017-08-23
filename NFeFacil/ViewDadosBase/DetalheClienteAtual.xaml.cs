using NFeFacil.ItensBD;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed partial class DetalheClienteAtual : ContentDialog
    {
        public DetalheClienteAtual(ClienteDI cliente)
        {
            InitializeComponent();
            DataContext = new ClienteDataContext(ref cliente);
        }
    }
}
