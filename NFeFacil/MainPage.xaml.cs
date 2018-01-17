using NFeFacil.View;
using System;
using System.Globalization;
using System.Reflection;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
            Analisar();
            AnalisarBarraTitulo();
            btnRetornar.Click += (x, y) => Retornar();
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };

            var info = new CultureInfo("pt-BR");
            CultureInfo.CurrentCulture = info;
            CultureInfo.CurrentUICulture = info;
            CultureInfo.DefaultThreadCurrentCulture = info;
            CultureInfo.DefaultThreadCurrentUICulture = info;
        }

        async void Analisar()
        {
            using (var analise = new Repositorio.OperacoesExtras())
            {
                await analise.AnalisarBanco(DefinicoesTemporarias.DateTimeNow);
            }
            using (var repo = new Repositorio.Leitura())
            {
                switch (DefinicoesPermanentes.TipoBackground)
                {
                    case TiposBackground.Imagem:
                        if (DefinicoesPermanentes.IDBackgroung != default(Guid))
                        {
                            var img = repo.ProcurarImagem(DefinicoesPermanentes.IDBackgroung);
                            ImagemBackground = img?.Bytes?.GetSource();
                        }
                        DefinirTipoBackground(TiposBackground.Imagem);
                        DefinirOpacidadeBackground(DefinicoesPermanentes.OpacidadeBackground);
                        break;
                    case TiposBackground.Cor:
                        DefinirTipoBackground(TiposBackground.Cor);
                        DefinirOpacidadeBackground(DefinicoesPermanentes.OpacidadeBackground);
                        break;
                }

                if (repo.EmitentesCadastrados)
                {
                    Navegar<Login.EscolhaEmitente>();
                }
                else
                {
                    Navegar<Login.PrimeiroUso>();
                }
            }
        }

        private async void AnalisarBarraTitulo()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
            {
                btnRetornar.Visibility = Visibility.Collapsed;
                await StatusBar.GetForCurrentView().HideAsync();
            }
            else if (familia.Contains("Desktop"))
            {
                CoreApplicationViewTitleBar tb = CoreApplication.GetCurrentView().TitleBar;
                tb.ExtendViewIntoTitleBar = true;
                tb.LayoutMetricsChanged += (sender, e) => TitleBar.Height = sender.Height;

                Window.Current.SetTitleBar(MainTitleBar);
                Window.Current.Activated += (sender, e) => TitleBar.Opacity = e.WindowActivationState != CoreWindowActivationState.Deactivated ? 1 : 0.5;

                var novoTB = ApplicationView.GetForCurrentView().TitleBar;
                novoTB.ButtonBackgroundColor = Colors.Transparent;
                novoTB.ButtonInactiveBackgroundColor = Colors.Transparent;
                novoTB.ButtonHoverBackgroundColor = new Color { A = 50 };
                novoTB.ButtonPressedBackgroundColor = new Color { A = 100 };
            }
        }

        public void Navegar<T>(object parametro = null) where T : Page
        {
            frmPrincipal.Navigate(typeof(T), parametro);
        }

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

            if (frmPrincipal.CanGoBack) frmPrincipal.GoBack();
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
            var pag = infoTipo.GetCustomAttribute<View.DetalhePagina>();

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
