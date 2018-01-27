using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.View;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal
{
    [DetalhePagina(Symbol.View, "Visualizar NFe")]
    public sealed partial class VisualizacaoNFe : Page
    {
        AcoesVisualizacao Acoes;
        InformacoesBase Visualizacao { get; set; }

        public VisualizacaoNFe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Acoes = (AcoesVisualizacao)e.Parameter;
            Visualizacao = Acoes.ObterVisualizacao();
            AtualizarBotoesComando((StatusNFe)Acoes.ItemBanco.Status);
            Acoes.StatusChanged += Acoes_StatusChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Acoes.StatusChanged -= Acoes_StatusChanged;
        }

        private void Acoes_StatusChanged(object sender, EventArgs e)
        {
            var status = (StatusChangedEventArgs)e;
            AtualizarBotoesComando(status.NovoStatus);
        }

        void Editar(object sender, RoutedEventArgs e) => Acoes.Editar();
        void Salvar(object sender, RoutedEventArgs e) => Acoes.Salvar();
        async void Assinar(object sender, RoutedEventArgs e) => await Acoes.Assinar();
        async void Transmitir(object sender, RoutedEventArgs e) => await Acoes.Transmitir();
        void Imprimir(object sender, RoutedEventArgs e) => Acoes.Imprimir();
        async void Exportar(object sender, RoutedEventArgs e) => await Acoes.Exportar();

        void AtualizarBotoesComando(StatusNFe status)
        {
            if (status == StatusNFe.Edição) status = StatusNFe.Validada;
            btnEditar.IsEnabled = status == StatusNFe.Validada || status == StatusNFe.Salva || status == StatusNFe.Assinada;
            btnSalvar.IsEnabled = status == StatusNFe.Validada;
            btnAssinar.IsEnabled = status == StatusNFe.Salva;
            btnTransmitir.IsEnabled = status == StatusNFe.Assinada;
            btnImprimir.IsEnabled = status == StatusNFe.Emitida;
        }
    }
}
