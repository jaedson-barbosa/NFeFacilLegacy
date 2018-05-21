using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Venda.ControlesAuxiliares
{
    public sealed partial class EscolhaCliente : UserControl
    {
        public static DependencyProperty ClienteSelecionadoProperty = DependencyProperty.Register(
            nameof(ClienteSelecionado),
            typeof(ClienteDI),
            typeof(EscolhaCliente),
            new PropertyMetadata(null, (x, y) => ((EscolhaCliente)x).ClienteSelecionado = (ClienteDI)y.NewValue));

        public ClienteDI ClienteSelecionado
        {
            get => (ClienteDI)GetValue(ClienteSelecionadoProperty);
            set => SetValue(ClienteSelecionadoProperty, value);
        }

        BuscadorCliente Clientes { get; }

        public EscolhaCliente()
        {
            InitializeComponent();
            Clientes = new BuscadorCliente();
        }

        private void BuscarCliente(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Clientes.Buscar(busca);
        }

        private void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            var input = (GridView)sender;
            var cliente = input.SelectedItem;
            if (cliente != null)
            {
                input.ScrollIntoView(cliente, ScrollIntoViewAlignment.Leading);
            }
        }
    }
}
