using NFeFacil.ItensBD;
using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace NFeFacil
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        internal static MainPage Current { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            Current = this;
            AnalisarBarraTituloAsync();
            btnRetornar.Click += (x, y) => Retornar();
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };
            frmPrincipal.CacheSize = 4;
            AbrirFunçao(typeof(Login.EscolhaEmitente));
            using (var db = new AplicativoContext())
            {
                Propriedades.EmitenteAtivo = db.Emitentes.FirstOrDefault();
            }
        }

        private void AnalisarBarraTituloAsync()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
            {
                btnRetornar.Visibility = Visibility.Collapsed;
                var barra = StatusBar.GetForCurrentView();
                var cor = new View.Estilos.Auxiliares.BibliotecaCores().Cor1;
                barra.BackgroundColor = cor;
                barra.BackgroundOpacity = 1;
            }
            else if (familia.Contains("Desktop"))
            {
                Window.Current.CoreWindow.KeyDown += (x, y) => System.Diagnostics.Debug.WriteLine(y.VirtualKey);
                CoreApplicationViewTitleBar tb = CoreApplication.GetCurrentView().TitleBar;
                tb.ExtendViewIntoTitleBar = true;
                tb.LayoutMetricsChanged += (sender, e) => TitleBar.Height = sender.Height;

                Window.Current.SetTitleBar(MainTitleBar);
                Window.Current.Activated += (sender, e) => TitleBar.Opacity = e.WindowActivationState != CoreWindowActivationState.Deactivated ? 1 : 0.5;

                var novoTB = ApplicationView.GetForCurrentView().TitleBar;
                var corBackground = new Color { A = 0 };
                novoTB.ButtonBackgroundColor = corBackground;
                novoTB.ButtonInactiveBackgroundColor = corBackground;
                novoTB.ButtonHoverBackgroundColor = new Color { A = 50 };
                novoTB.ButtonPressedBackgroundColor = new Color { A = 100 };
            }
        }

        public void AbrirFunçao(Type tela, object parametro = null)
        {
            frmPrincipal.Navigate(tela, parametro);
        }

        public void SeAtualizar(Symbol símbolo, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new SymbolIcon(símbolo);
            UIElement conteudo = null;
            if (frmPrincipal.Content is IHambuguer hambuguer)
            {
                menuPermanente.Visibility = btnHambuguer.Visibility = Visibility.Visible;
                conteudo = hambuguer.ConteudoMenu;

                AtualizarPosicaoMenu(Window.Current.Bounds.Width >= 720);

                grupoTamanhoTela.CurrentStateChanged += TamanhoTelaMudou;
            }
            else
            {
                splitView.CompactPaneLength = 0;
                menuPermanente.Visibility = btnHambuguer.Visibility = Visibility.Collapsed;
                menuPermanente.Content = splitView.Pane = null;
                grupoTamanhoTela.CurrentStateChanging -= TamanhoTelaMudou;
            }

            void TamanhoTelaMudou(object sender, VisualStateChangedEventArgs e)
            {
                AtualizarPosicaoMenu(e.NewState.Name == "TelaGrande");
            }

            void AtualizarPosicaoMenu(bool telaGrande)
            {
                if (telaGrande)
                {
                    splitView.Pane = null;
                    splitView.CompactPaneLength = 0;
                    menuPermanente.Content = conteudo;
                }
                else
                {
                    menuPermanente.Content = null;
                    splitView.CompactPaneLength = 48;
                    splitView.Pane = conteudo;
                }
            }

            AtualizarExibicaoExtra(frmPrincipal.Content is View.Inicio ? ExibicaoExtra.EscolherEmitente : ExibicaoExtra.ExibirEmitente);
        }

        public void SeAtualizar(string glyph, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new FontIcon { Glyph = glyph };
            AtualizarExibicaoExtra(ExibicaoExtra.ExibirEmitente);
        }

        public void SeAtualizarEspecial(string glyph, string texto, ExibicaoExtra extra, string nomeVendedor)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new FontIcon { Glyph = glyph };
            if (extra == ExibicaoExtra.EscolherVendedor)
            {
                AtualizarExibicaoExtra(ExibicaoExtra.EscolherVendedor);
            }
            else
            {
                if (!string.IsNullOrEmpty(nomeVendedor))
                {
                    if (nomeVendedor.Contains(' '))
                    {
                        var indexEspaco = nomeVendedor.IndexOf(' ');
                        txtEscolhido.Text = nomeVendedor.Substring(0, indexEspaco);
                    }
                    else
                    {
                        txtEscolhido.Text = nomeVendedor;
                    }
                }
                else
                {
                    txtEscolhido.Text = "Indisponível";
                }
                txtEscolhido.Visibility = Visibility.Visible;
                txtTitulo.Visibility = Visibility.Visible;
                cmbEscolha.Visibility = Visibility.Collapsed;
                cmbEscolha.SelectionChanged -= SelecaoMudou;
                cmbEscolha.ItemsSource = null;
            }
        }

        public async void Retornar()
        {
            if (frmPrincipal.Content is IValida retorna)
            {
                if (!await retorna.Verificar())
                {
                    return;
                }
            }
            else if ((frmPrincipal.Content as FrameworkElement).DataContext is IValida retornaDC)
            {
                if (!await retornaDC.Verificar())
                {
                    return;
                }
            }

            if (frmPrincipal.CanGoBack)
            {
                frmPrincipal.GoBack();
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void AbrirHamburguer(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        void AtualizarExibicaoExtra(ExibicaoExtra ativa)
        {
            switch (ativa)
            {
                case ExibicaoExtra.ExibirEmitente:
                    var emit = Propriedades.EmitenteAtivo;
                    if (emit != null)
                    {
                        txtEscolhido.Text = emit.Nome.Substring(0, emit.Nome.IndexOf(' '));
                    }
                    else
                    {
                        txtEscolhido.Text = string.Empty;
                    }
                    txtEscolhido.Visibility = Visibility.Visible;
                    txtTitulo.Visibility = Visibility.Visible;
                    cmbEscolha.Visibility = Visibility.Collapsed;
                    cmbEscolha.SelectionChanged -= SelecaoMudou;
                    cmbEscolha.ItemsSource = null;
                    break;
                case ExibicaoExtra.EscolherEmitente:
                    using (var db = new AplicativoContext())
                    {
                        var emits = db.Emitentes;
                        cmbEscolha.ItemsSource = emits.GerarObs();
                        cmbEscolha.SelectionChanged += SelecaoMudou;
                        if (cmbEscolha.SelectedIndex == -1 && emits.Count() > 0)
                        {
                            if (Propriedades.EmitenteAtivo != null)
                            {
                                cmbEscolha.SelectedItem = Propriedades.EmitenteAtivo;
                            }
                            else
                            {
                                cmbEscolha.SelectedIndex = 0;
                            }
                        }
                        txtEscolhido.Text = string.Empty;
                        txtEscolhido.Visibility = Visibility.Collapsed;
                        txtTitulo.Visibility = Visibility.Collapsed;
                        cmbEscolha.Visibility = Visibility.Visible;
                    }
                    break;
                case ExibicaoExtra.EscolherVendedor:
                    using (var db = new AplicativoContext())
                    {
                        var vends = db.Vendedores;
                        cmbEscolha.ItemsSource = vends.GerarObs();
                        cmbEscolha.SelectionChanged += SelecaoMudou;
                        if (cmbEscolha.SelectedIndex == -1 && vends.Count() > 0)
                        {
                            if (Propriedades.VendedorAtivo != null)
                            {
                                cmbEscolha.SelectedItem = Propriedades.VendedorAtivo;
                            }
                            else
                            {
                                cmbEscolha.SelectedIndex = 0;
                            }
                        }
                        txtEscolhido.Text = string.Empty;
                        txtEscolhido.Visibility = Visibility.Collapsed;
                        txtTitulo.Visibility = Visibility.Visible;
                        cmbEscolha.Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    txtEscolhido.Visibility = Visibility.Collapsed;
                    cmbEscolha.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        void SelecaoMudou(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var item = e.AddedItems[0];
                if (item is EmitenteDI novoEmit)
                {
                    Propriedades.EmitenteAtivo = novoEmit;
                }
                else if (item is Vendedor vendedor)
                {
                    Propriedades.VendedorAtivo = vendedor;
                }
            }
        }
    }

    public enum ExibicaoExtra
    {
        ExibirVendedor,
        ExibirEmitente,
        EscolherVendedor,
        EscolherEmitente
    }
}
