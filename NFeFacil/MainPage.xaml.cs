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
    public sealed partial class MainPage : Page
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
            var backgroundFrame = (SolidColorBrush)frmPrincipal.Background;
            backgroundFrame.Opacity = opacidade;
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
            frmPrincipal.Navigate(typeof(T), parametro);
        }

        private void Retornar(object sender, RoutedEventArgs e) => Retornar();
        public async void Retornar()
        {
            if (frmPrincipal.Content is IValida valida && !valida.Concluido)
            {
                var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, escolha Cancelar.", "Atenção");
                mensagem.Commands.Add(new UICommand("Sair"));
                mensagem.Commands.Add(new UICommand("Cancelar"));
                var resultado = await mensagem.ShowAsync();
                if (resultado.Label == "Cancelar") return;
            }

            if (frmPrincipal.BackStackDepth > 1) frmPrincipal.GoBack();
            else
            {
                var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
                if (familia.Contains("Mobile"))
                {
                    Application.Current.Exit();
                }
            }
        }

        private void AbrirHamburguer(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void MudouSubpaginaEscolhida(object sender, SelectionChangedEventArgs e)
        {
            if (frmPrincipal.Content is IHambuguer hamb)
            {
                hamb.SelectedIndex = menuTemporario.SelectedIndex;
            }
        }

        public void AtualizarInformaçõesGerais()
        {
            imgLogotipo.Source = DefinicoesTemporarias.Logotipo;
            txtNomeEmitente.Text = DefinicoesTemporarias.EmitenteAtivo.Nome;
            txtNomeEmpresa.Text = DefinicoesTemporarias.EmitenteAtivo.NomeFantasia;

            if (DefinicoesTemporarias.VendedorAtivo != null)
            {
                imgVendedor.Source = DefinicoesTemporarias.FotoVendedor;
                txtNomeVendedor.Text = DefinicoesTemporarias.VendedorAtivo.Nome;
            }
            else
            {
                txtNomeVendedor.Text = "Administrador";
            }
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
    }
}
