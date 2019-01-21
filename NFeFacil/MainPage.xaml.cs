using BaseGeral;
using BaseGeral.View;
using NFeFacil.View;
using System;
using System.Reflection;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace NFeFacil
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page, IMainPage
    {
        internal static MainPage Current { get; private set; }

        internal void DefinirTipoBackground(TiposBackground tipo)
        {
            switch (tipo)
            {
                case TiposBackground.Imagem:
                    backgroundCor.Visibility = Visibility.Collapsed;
                    backgroundImagem.Visibility = Visibility.Visible;
                    break;
                case TiposBackground.Cor:
                    backgroundCor.Visibility = Visibility.Visible;
                    backgroundImagem.Visibility = Visibility.Collapsed;
                    break;
                case TiposBackground.Padrao:
                    backgroundCor.Visibility = Visibility.Collapsed;
                    backgroundImagem.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
            DefinicoesPermanentes.TipoBackground = tipo;
        }

        internal void DefinirOpacidadeBackground(double opacidade)
        {
            var backgroundFrame = (SolidColorBrush)FramePrincipal.Background;
            backgroundFrame.Opacity = opacidade;
            DefinicoesPermanentes.OpacidadeBackground = opacidade;
        }

        internal ImageSource ImagemBackground
        {
            get => backgroundImagem.Source;
            set => backgroundImagem.Source = value;
        }

        public MainPage()
        {
            InitializeComponent();
            Current = this;
            Navegar<Login.Loading>();
        }

        public void Navegar<T>(object parametro = null) where T : Page
        {
            try
            {
                FramePrincipal.Navigate(typeof(T), parametro);
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        private void Retornar(object sender, RoutedEventArgs e) => Retornar();
        public async void Retornar()
        {
            if (FramePrincipal.Content is IValida valida && !valida.Concluido)
            {
                var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, escolha Cancelar.", "Atenção");
                mensagem.Commands.Add(new UICommand("Sair"));
                mensagem.Commands.Add(new UICommand("Cancelar"));
                var resultado = await mensagem.ShowAsync();
                if (resultado.Label == "Cancelar") return;
            }

            if (FramePrincipal.BackStackDepth >= 1) FramePrincipal.GoBack();
        }

        private void AbrirHamburguer(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
            if (splitView.IsPaneOpen)
                RotInfoPane.Begin();
        }

        private void MudouSubpaginaEscolhida(object sender, SelectionChangedEventArgs e)
        {
            if (FramePrincipal.Content is IHambuguer hamb)
            {
                hamb.SelectedIndex = menuTemporario.SelectedIndex;
            }
        }

        public void AtualizarInformaçõesGerais()
        {
            imgLogotipo.Source = DefinicoesTemporarias.Logotipo;
            txtNomeEmpresa.Text = DefinicoesTemporarias.EmitenteAtivo.NomeFantasia;
            txtNomeEmitente.Text = DefinicoesTemporarias.VendedorAtivo?.Nome ?? DefinicoesTemporarias.EmitenteAtivo.Nome;
        }

        private void NavegacaoConcluida(object sender, NavigationEventArgs e)
        {
            var navegada = e.Content;
            var infoTipo = e.Content.GetType().GetTypeInfo();
            var pag = infoTipo.GetCustomAttribute<DetalhePagina>();

            if (pag == null)
            {
                txtTitulo.Text = "Erro, informar desenvolvedor";
                symTitulo.Content = new SymbolIcon(Symbol.Help);
            }
            else
            {
                txtTitulo.Text = pag.Titulo;
                symTitulo.Content = pag.ObterIcone();
            }

            if (navegada is IHambuguer hambuguer)
            {
                btnHamburguer.Visibility = Visibility.Visible;
                menuTemporario.ItemsSource = hambuguer.ConteudoMenu;
                menuTemporario.SelectedIndex = 0;
                splitView.CompactPaneLength = 48;
            }
            else
            {
                btnHamburguer.Visibility = Visibility.Collapsed;
                menuTemporario.ItemsSource = null;
                splitView.CompactPaneLength = 0;
            }
        }

        bool deveExecutarAnimFechamento = true;
        private void SplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            if (!deveExecutarAnimFechamento)
            {
                deveExecutarAnimFechamento = true;
                return;
            }
            args.Cancel = true;
            RotInfoPaneInverse.Begin();
        }

        private void RotInfoPaneInverse_Completed(object sender, object e)
        {
            splitView.IsPaneOpen = deveExecutarAnimFechamento = false;
        }
    }
}
