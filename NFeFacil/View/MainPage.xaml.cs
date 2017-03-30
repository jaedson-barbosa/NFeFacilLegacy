using NFeFacil.Log;
using NFeFacil.View.Controles;
using System;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Propriedades públicas
        public Frame FramePrincipal
        {
            get { return frmPrincipal; }
        }
        public Symbol Símbolo
        {
            get { return symTitulo.Symbol; }
            set { symTitulo.Symbol = value; }
        }
        public string Título
        {
            get { return txtTitulo.Text; }
            set { txtTitulo.Text = value; }
        }
        public int IndexHamburguer
        {
            get { return lstFunções.SelectedIndex; }
            set { lstFunções.SelectedIndex = value; }
        }
        #endregion

        private int ultimoIndex;
        public MainPage()
        {
            this.InitializeComponent();
            ProcessarAsync();
            Propriedades.Intercambio = new IntercambioTelas(this);
            AbrirFunção(nameof(Inicio));
            SystemNavigationManager.GetForCurrentView().BackRequested += Propriedades.Intercambio.RetornoEvento;
            if (NFeFacil.Configuracoes.ConfiguracoesSincronizacao.InícioAutomático) InicarServerAsync();
        }

        private static async void ProcessarAsync()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
                await StatusBar.GetForCurrentView().HideAsync();
            else if (familia.Contains("Desktop"))
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                new Saida().Escrever(TitulosComuns.ErroSimples, "Tipo não reconhecido de dispositivo, não é possível mudar a barra de título.");
        }

        private async static void InicarServerAsync() => await Propriedades.Server.IniciarServer().ConfigureAwait(false);
        private void btnHamburguer_Click(object sender, RoutedEventArgs e) => hmbMenu.IsPaneOpen = !hmbMenu.IsPaneOpen;
        private void AbrirFunção(object sender, ItemClickEventArgs e) => AbrirFunção((e.ClickedItem as SplitViewItem).Name);

        private async void AbrirFunção(string tela)
        {
            if (FramePrincipal?.Content is IValida)
            {
                var validar = FramePrincipal.Content as IValida;
                if (await validar.Verificar())
                {
                    if (FramePrincipal.Content is IEsconde)
                    {
                        var esconder = FramePrincipal.Content as IEsconde;
                        await esconder.EsconderAsync();
                    }
                    else
                    {
                        ILog log = new Saida();
                        log.Escrever(TitulosComuns.ErroSimples, $"A tela {tela} ainda precisa implementar IEsconde!");
                    }
                }
                else
                {
                    IndexHamburguer = ultimoIndex;
                    return;
                }
            }
            else if (FramePrincipal?.Content is IEsconde)
            {
                var esconder = FramePrincipal.Content as IEsconde;
                await esconder.EsconderAsync();
            }
            Propriedades.Intercambio.AbrirFunçaoAsync(tela);
            ultimoIndex = IndexHamburguer;
        }
    }
}
