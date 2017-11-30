using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    public sealed partial class EscolhaProdutos : ContentDialog
    {
        internal int TipoTotal { get; private set; }
        internal IEnumerable<string> Escolhidos => lst.SelectedItems.Cast<string>();

        public EscolhaProdutos()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                lst.ItemsSource = db.Produtos.Select(x => x.Descricao).GerarObs();
            }
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
