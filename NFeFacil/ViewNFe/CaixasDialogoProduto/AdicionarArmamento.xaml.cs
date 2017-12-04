using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoProduto
{
    public sealed partial class AdicionarArmamento : ContentDialog
    {
        public Arma Contexto { get; } = new Arma();

        public AdicionarArmamento()
        {
            InitializeComponent();
        }
    }
}
