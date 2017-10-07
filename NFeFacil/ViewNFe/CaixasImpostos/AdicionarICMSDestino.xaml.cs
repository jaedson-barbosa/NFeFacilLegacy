using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasImpostos
{
    public sealed partial class AdicionarICMSDestino : ContentDialog
    {
        public AdicionarICMSDestino()
        {
            InitializeComponent();
        }

        public ICMSUFDest Imposto { get; } = new ICMSUFDest();
    }
}
