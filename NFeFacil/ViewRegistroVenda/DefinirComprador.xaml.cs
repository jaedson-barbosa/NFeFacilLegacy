using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    public sealed partial class DefinirComprador : ContentDialog
    {
        ObservableCollection<string> Nomes { get; }
        internal string Escolhido { get; private set; }

        public DefinirComprador(IEnumerable<string> nomes)
        {
            this.InitializeComponent();
            Nomes = nomes.GerarObs();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(Escolhido))
            {
                args.Cancel = true;
            }
        }
    }
}
