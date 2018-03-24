using BaseGeral;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal
{
    public sealed partial class EscolhaProdutos : ContentDialog
    {
        internal int TipoTotal { get; private set; }
        internal IEnumerable<string> Escolhidos => lst.SelectedItems.Cast<string>();

        public EscolhaProdutos(IEnumerable<string> produtos)
        {
            InitializeComponent();
            lst.ItemsSource = produtos.GerarObs();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (lst.SelectedItems.Count == 0)
            {
                args.Cancel = true;
            }
        }
    }
}
