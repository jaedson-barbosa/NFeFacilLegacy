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
            get
            {
                var cliente = (ClienteDI)GetValue(ClienteSelecionadoProperty);
                if (cliente == null)
                    cliente = Clientes.BuscarViaDocumento(BuscaInicial);
                MoverParaItemSelecionado(cliente);
                return cliente;
            }
            set => SetValue(ClienteSelecionadoProperty, value);
        }

        public static DependencyProperty BuscaInicialProperty = DependencyProperty.Register(
            nameof(BuscaInicial),
            typeof(string),
            typeof(EscolhaCliente),
            new PropertyMetadata(null, (x, y) => ((EscolhaCliente)x).BuscaInicial = (string)y.NewValue));

        public string BuscaInicial { get; set; }

        BuscadorCliente Clientes { get; }

        public EscolhaCliente()
        {
            InitializeComponent();
            Clientes = new BuscadorCliente();
        }

        void BuscarCliente(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Clientes.Buscar(busca);
        }

        void MoverParaItemSelecionado(ClienteDI cliente)
        {
            if (cliente != null)
                grdView.ScrollIntoView(cliente, ScrollIntoViewAlignment.Leading);
        }
    }
}
