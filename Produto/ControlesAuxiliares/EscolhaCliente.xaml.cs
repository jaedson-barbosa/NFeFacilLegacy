using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using System;
using System.Linq;
using System.Threading.Tasks;
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
                if (cliente == null && !string.IsNullOrEmpty(BuscaInicial))
                    cliente = Clientes.BuscarViaDocumento(BuscaInicial);
                else
                    cliente = Clientes.Itens.FirstOrDefault(x => x.Id == IdClienteSelecionado);
                MoverParaItemSelecionado(cliente);
                return cliente;
            }
            set
            {
                if (value == null) return;
                SetValue(ClienteSelecionadoProperty, value);
                IdClienteSelecionado = value.Id;
            }
        }

        public static DependencyProperty IdClienteSelecionadoProperty = DependencyProperty.Register(
            nameof(IdClienteSelecionado),
            typeof(Guid),
            typeof(EscolhaMotorista),
            new PropertyMetadata(null));

        public Guid IdClienteSelecionado
        {
            get
            {
                if (GetValue(IdClienteSelecionadoProperty) is Guid guid) return guid;
                else return Guid.Empty;
            }
            set => SetValue(IdClienteSelecionadoProperty, value);
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

        async void MoverParaItemSelecionado(ClienteDI cliente)
        {
            if (cliente != null)
            {
                await Task.Delay(1000);
                grdView.ScrollIntoView(cliente, ScrollIntoViewAlignment.Leading);
            }
        }
    }
}
