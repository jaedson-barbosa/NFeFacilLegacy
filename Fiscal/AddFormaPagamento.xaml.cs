using BaseGeral.ModeloXML;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal
{
    public sealed partial class AddFormaPagamento : ContentDialog
    {
        public Pagamento Pagamento { get; } = new Pagamento();

        public AddFormaPagamento()
        {
            InitializeComponent();
        }
    }
}
