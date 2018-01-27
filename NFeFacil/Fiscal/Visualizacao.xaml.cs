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
    public sealed partial class Visualizacao : Page
    {
        AcoesVisualizacao Acoes;
        InformacoesBase ObjetoVisualizado { get; set; }

        public Visualizacao()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Acoes = (AcoesVisualizacao)e.Parameter;
            ObjetoVisualizado = Acoes.ObterVisualizacao();
            AtualizarBotoesComando((StatusNota)Acoes.ItemBanco.Status);
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

        void AtualizarBotoesComando(StatusNota status)
        {
            if (status == StatusNota.Edição) status = StatusNota.Validada;
            btnEditar.IsEnabled = status == StatusNota.Validada || status == StatusNota.Salva || status == StatusNota.Assinada;
            btnSalvar.IsEnabled = status == StatusNota.Validada;
            btnAssinar.IsEnabled = status == StatusNota.Salva;
            btnTransmitir.IsEnabled = status == StatusNota.Assinada;
            btnImprimir.IsEnabled = status == StatusNota.Emitida;
        }
    }
}
