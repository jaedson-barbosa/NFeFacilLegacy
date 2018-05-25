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
                if (motorista == null && !string.IsNullOrEmpty(BuscaInicial))
                    motorista = Motoristas.BuscarViaDocumento(BuscaInicial);
                else
                    motorista = Motoristas.Itens.FirstOrDefault(x => x.Id == IdMotoristaSelecionado);
                MoverParaItemSelecionado(motorista);
                return motorista;
            }
            set
            {
                if (value == null) return;
                SetValue(MotoristaSelecionadoProperty, value);
                IdMotoristaSelecionado = value.Id;
            }
        }

        public static DependencyProperty IdMotoristaSelecionadoProperty = DependencyProperty.Register(
            nameof(IdMotoristaSelecionado),
            typeof(Guid),
            typeof(EscolhaMotorista),
            new PropertyMetadata(null));

        public Guid IdMotoristaSelecionado
        {
            get
            {
                if (GetValue(IdMotoristaSelecionadoProperty) is Guid guid) return guid;
                else return Guid.Empty;
            }
            set => SetValue(IdMotoristaSelecionadoProperty, value);
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

        async void MoverParaItemSelecionado(MotoristaDI motorista)
        {
            if (motorista != null)
            {
                await Task.Delay(1000);
                grdView.ScrollIntoView(motorista, ScrollIntoViewAlignment.Leading);
            }
        }
    }
}
