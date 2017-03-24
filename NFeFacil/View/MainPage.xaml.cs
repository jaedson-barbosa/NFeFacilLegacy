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
    }
}
