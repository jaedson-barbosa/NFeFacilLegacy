using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewModel.ImpostosProduto;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarIPIValor : ContentDialog
    {
        public AdicionarIPIValor()
        {
            this.InitializeComponent();
        }

        public IPI Conjunto { get; } = new IPI();
        public ConteinerIPI Imposto { get; set; }
    }
}
