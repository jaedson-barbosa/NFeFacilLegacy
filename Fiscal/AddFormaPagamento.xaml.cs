using BaseGeral.ModeloXML;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    public sealed partial class AddFormaPagamento : ContentDialog
    {
        public DetalhamentoPagamento Pagamento { get; } = new DetalhamentoPagamento();

        public AddFormaPagamento()
        {
            InitializeComponent();
        }
    }
}
