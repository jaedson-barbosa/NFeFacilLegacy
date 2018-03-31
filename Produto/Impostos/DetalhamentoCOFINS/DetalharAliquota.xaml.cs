using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    [DetalhePagina("COFINS")]
    public sealed partial class DetalharAliquota : Page
    {
        public double Aliquota { get; private set; }

        public DetalharAliquota()
        {
            InitializeComponent();
        }
    }
}
