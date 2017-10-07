using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarIPIAliquota : ContentDialog
    {
        public AdicionarIPIAliquota()
        {
            InitializeComponent();
        }

        public IPI Conjunto { get; } = new IPI();
        public string Aliquota { get; private set; }
    }
}
