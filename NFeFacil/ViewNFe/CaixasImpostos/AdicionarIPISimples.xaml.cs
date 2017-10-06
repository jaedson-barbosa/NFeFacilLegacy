using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarIPISimples : ContentDialog
    {
        public AdicionarIPISimples()
        {
            InitializeComponent();
        }

        public IPI Conjunto { get; } = new IPI()
        {
            Corpo = new IPINT()
        };
    }
}
