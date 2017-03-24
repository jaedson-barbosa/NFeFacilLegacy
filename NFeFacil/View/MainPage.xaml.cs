using NFeFacil.Log;
using NFeFacil.NavegacaoUI;
using NFeFacil.View.Controles;
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
        public int IndexFunçãoPrincipal
        {
            get { return lstFunções.SelectedIndex; }
            set { lstFunções.SelectedIndex = value; }
        }
        public int IndexFunçãoExtra
        {
            get { return lstExtras.SelectedIndex; }
            set { lstExtras.SelectedIndex = value; }
        }
        public int UltimoIndex { get; private set; }
        #endregion

        public MainPage()
        {
            this.InitializeComponent();
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
                        await esconder.Esconder();
                    }
                    else
                    {
                        ILog log = new Saida();
                        log.Escrever(TitulosComuns.ErroSimples, $"A tela {tela} ainda precisa implementar IEsconde!");
                    }
                }
                else
                {
                    IndexFunçãoPrincipal = UltimoIndex;
                    IndexFunçãoExtra = -1;
                    return;
                }
            }
            else if (FramePrincipal?.Content is IEsconde)
            {
                var esconder = FramePrincipal.Content as IEsconde;
                await esconder.Esconder();
            }
            Propriedades.Intercambio.AbrirFunçao(tela);
            UltimoIndex = IndexFunçãoPrincipal;
        }
    }
}
