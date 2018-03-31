using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.ProdutoEspecial
{
    public sealed partial class AdicionarMedicamento : ContentDialog
    {
        public Medicamento Contexto { get; } = new Medicamento();

        public AdicionarMedicamento()
        {
            InitializeComponent();
        }
    }
}
