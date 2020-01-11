using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    [DetalhePagina(Symbol.Home, "Dados da empresa")]
    public sealed partial class GeralEmitente : Page
    {
        EmitenteDI emitente;
        ImageSource imagem;

        public GeralEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame.BackStack.Clear();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                var emit = repo.ObterEmitente();
                var imagem = emit.Item2?.GetSource();

                var brush = new ImageBrush { ImageSource = imagem };
                imgLogotipo.Background = brush;
                txtNomeFantasia.Text = emit.Item1.NomeFantasia;
                txtNome.Text = emit.Item1.Nome;
                emitente = emit.Item1;
                this.imagem = imagem;

                var vendedores = repo.ObterVendedores().GerarObs();
                if (vendedores.Count == 0)
                {
                    lstVendedores.Visibility = Visibility.Collapsed;
                    chkAdmin.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lstVendedores.ItemsSource = vendedores;
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back) e.Cancel = true;
        }

        private void Confirmar(object sender, RoutedEventArgs e)
        {
            DefinicoesTemporarias.VendedorAtivo = lstVendedores.SelectedItem as Vendedor;
            DefinicoesTemporarias.EmitenteAtivo = emitente;
            DefinicoesTemporarias.Logotipo = imagem;
            MainPage.Current.Navegar<View.Inicio>();
            MainPage.Current.AtualizarInformaçõesGerais();
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>(emitente);
        }

        async void Logotipo(object sender, RoutedEventArgs e)
        {
            var brush = (ImageBrush)imgLogotipo.Background;
            var caixa = new View.DefinirImagem(emitente.Id, brush.ImageSource);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                brush.ImageSource = caixa.Imagem;
                imagem = caixa.Imagem;
            }
        }

        void ChkAdmin_Unchecked(object sender, RoutedEventArgs e) => lstVendedores.IsEnabled = true;
        void ChkAdmin_Checked(object sender, RoutedEventArgs e)
        {
            lstVendedores.IsEnabled = false;
            lstVendedores.SelectedIndex = -1;
        }
    }
}
