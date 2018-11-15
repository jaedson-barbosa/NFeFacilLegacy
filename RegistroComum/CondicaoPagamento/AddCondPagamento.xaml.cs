using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum.CondicaoPagamento
{
    public sealed partial class AddCondPagamento : ContentDialog
    {
        public string Condicao { get; private set; }

        public AddCondPagamento()
        {
            InitializeComponent();
        }

        void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = Condicao.Length < 5;
        }
    }
}
