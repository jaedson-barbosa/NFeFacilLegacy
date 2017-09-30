using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.CaixasDialogoProduto
{
    public sealed partial class AddDeclaracaoExportacaoDireta : ContentDialog
    {
        public GrupoExportacao Declaracao { get; }

        public AddDeclaracaoExportacaoDireta()
        {
            InitializeComponent();
            Declaracao = new GrupoExportacao();
        }
    }
}
