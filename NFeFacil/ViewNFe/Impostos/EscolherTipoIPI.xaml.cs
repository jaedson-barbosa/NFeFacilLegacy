using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public sealed partial class EscolherTipoIPI : ContentDialog
    {
        public EscolherTipoIPI()
        {
            InitializeComponent();
        }

        string cst;
        public string CST
        {
            get => cst;
            private set
            {
                cst = value;
                var valor = int.Parse(value);
                if (valor == 0 || valor == 49 || valor == 50 || valor == 99)
                {
                    cmbTipoCalculo.IsEnabled = true;
                }
                else
                {
                    cmbTipoCalculo.IsEnabled = false;
                    TipoCalculo = TiposCalculo.Inexistente;
                }
                cmbTipoCalculo.SelectedIndex = -1;
            }
        }

        internal TiposCalculo TipoCalculo { get; private set; }

        private void TipoCalculo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var novoItem = (ComboBoxItem)e.AddedItems[0];
            var tag = int.Parse(novoItem.Tag.ToString());
            if (tag != -1) TipoCalculo = (TiposCalculo)tag;
        }
    }
}
