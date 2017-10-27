using Microsoft.EntityFrameworkCore;
using NFeFacil.ItensBD;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            using (var db = new AplicativoContext())
            {
                db.Database.Migrate();
            }
            Analisar();
            AnalisarBarraTitulo();
            btnRetornar.Click += (x, y) => Retornar();
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };
            using (var db = new AplicativoContext())
            {
                Propriedades.EmitenteAtivo = db.Emitentes.FirstOrDefault();
                if (db.Emitentes.Count() > 0)
                {
                    Navegar<Login.EscolhaEmitente>();
                }
                else
                {
                    Navegar<Login.PrimeiroUso>();
                }
            }
        }

        async void Analisar()
        {
            using (var db = new AplicativoContext())
            {
                await db.Clientes.ForEachAsync(x => AnalisarItem(x));
                await db.Emitentes.ForEachAsync(x => AnalisarItem(x));
                await db.Motoristas.ForEachAsync(x => AnalisarItem(x));
                await db.Vendedores.ForEachAsync(x => AnalisarItem(x));
                await db.Produtos.ForEachAsync(x => AnalisarItem(x));
                await db.Estoque.Include(x => x.Alteracoes).ForEachAsync(x =>
                {
                    x.Alteracoes?.ForEach(alt =>
                    {
                        if (alt.MomentoRegistro == default(DateTime))
                        {
                            alt.MomentoRegistro = DateTime.Now;
                            db.Update(alt);
                        }
                    });
                    AnalisarItem(x);
                });
                await db.Vendas.ForEachAsync(x => AnalisarItem(x));
                await db.Imagens.ForEachAsync(x => AnalisarItem(x));
                db.SaveChanges();

                void AnalisarItem(IUltimaData item)
                {
                    if (item.UltimaData == DateTime.MinValue)
                    {
                        item.UltimaData = DateTime.Now;
                        db.Update(item);
                    }
                }
            }
        }

        private async void AnalisarBarraTitulo()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
            {
                btnRetornar.Visibility = Visibility.Collapsed;
                var barra = StatusBar.GetForCurrentView();
                await barra.HideAsync();
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

        public void Navegar<T>(object parametro = null) where T : Page
        {
            frmPrincipal.Navigate(typeof(T), parametro);
        }

        public void SeAtualizar(Symbol símbolo, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new SymbolIcon(símbolo);
            AnalisarMenuHamburguer();
        }

        public void SeAtualizar(string glyph, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new FontIcon { Glyph = glyph };
            AnalisarMenuHamburguer();
        }

        internal void SeAtualizar(Uri caminho, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new BitmapIcon { UriSource = caminho };
            AnalisarMenuHamburguer();
        }

        void AnalisarMenuHamburguer()
        {
            if (frmPrincipal.Content is IHambuguer hambuguer)
            {
                menuTemporario.ItemsSource = hambuguer.ConteudoMenu;
                menuTemporario.SelectedIndex = 0;
                splitView.CompactPaneLength = 48;
            }
            else
            {
                menuTemporario.ItemsSource = null;
                splitView.CompactPaneLength = 0;
            }
        }

        internal void AlterarSelectedIndexHamburguer(int index)
        {
            if (menuTemporario.ItemsSource != null)
            {
                menuTemporario.SelectedIndex = index;
            }
        }

        public async void Retornar(bool suprimirValidacao = false)
        {
            if (!suprimirValidacao && frmPrincipal.Content is IValida retorna && !await retorna.Verificar())
            {
                return;
            }

            if (frmPrincipal.CanGoBack)
            {
                frmPrincipal.GoBack();
            }
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
            if (menuTemporario.ItemsSource != null)
            {
                if (frmPrincipal.Content is IHambuguer hamb)
                {
                    hamb.AtualizarMain(menuTemporario.SelectedIndex);
                }
            }
        }

        public async Task AtualizarInformaçõesGerais()
        {
            grdInfoGeral.Visibility = Visibility.Visible;
            using (var db = new AplicativoContext())
            {
                var img = db.Imagens.Find(Propriedades.EmitenteAtivo.Id);
                if (img != null && img.Bytes != null)
                {
                    imgLogotipo.Source = await img.GetSourceAsync();
                }
                txtNomeEmitente.Text = Propriedades.EmitenteAtivo.Nome;
                txtNomeEmpresa.Text = Propriedades.EmitenteAtivo.NomeFantasia;

                if (Propriedades.VendedorAtivo != null)
                {
                    img = db.Imagens.Find(Propriedades.VendedorAtivo.Id);
                    if (img != null && img.Bytes != null)
                    {
                        imgVendedor.Source = await img.GetSourceAsync();
                    }
                    txtNomeVendedor.Text = Propriedades.VendedorAtivo.Nome;
                }
                else
                {
                    txtNomeVendedor.Text = "Administrador";
                }
            }
        }
    }
}
