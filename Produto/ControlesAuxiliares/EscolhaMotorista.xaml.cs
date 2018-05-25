using BaseGeral.Buscador;
using BaseGeral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Venda.ControlesAuxiliares
{
    public sealed partial class EscolhaMotorista : UserControl
    {
        public static DependencyProperty MotoristaSelecionadoProperty = DependencyProperty.Register(
            nameof(MotoristaSelecionado),
            typeof(MotoristaDI),
            typeof(EscolhaMotorista),
            new PropertyMetadata(null, (x, y) => ((EscolhaMotorista)x).MotoristaSelecionado = (MotoristaDI)y.NewValue));

        public MotoristaDI MotoristaSelecionado
        {
            get
            {
                var motorista = (MotoristaDI)GetValue(MotoristaSelecionadoProperty);
                if (motorista == null)
                    motorista = Motoristas.BuscarViaDocumento(BuscaInicial);
                MoverParaItemSelecionado(motorista);
                return motorista;
            }

            set => SetValue(MotoristaSelecionadoProperty, value);
        }

        public static DependencyProperty BuscaInicialProperty = DependencyProperty.Register(
            nameof(BuscaInicial),
            typeof(string),
            typeof(EscolhaMotorista),
            new PropertyMetadata(null, (x, y) => ((EscolhaMotorista)x).BuscaInicial = (string)y.NewValue));

        public string BuscaInicial { get; set; }

        BuscadorMotorista Motoristas { get; }

        public EscolhaMotorista()
        {
            InitializeComponent();
            Motoristas = new BuscadorMotorista();
        }

        void BuscarMotorista(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            Motoristas.Buscar(busca);
        }

        void MoverParaItemSelecionado(MotoristaDI motorista)
        {
            if (motorista != null)
                grdView.ScrollIntoView(motorista, ScrollIntoViewAlignment.Leading);
        }
    }
}
