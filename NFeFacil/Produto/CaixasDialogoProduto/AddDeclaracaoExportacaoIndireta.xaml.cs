using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.CaixasDialogoProduto
{
    public sealed partial class AddDeclaracaoExportacaoIndireta : ContentDialog
    {
        public GrupoExportacao Declaracao { get; }

        public AddDeclaracaoExportacaoIndireta()
        {
            InitializeComponent();
            Declaracao = new GrupoExportacao()
            {
                ExportInd = new ExportacaoIndireta()
            };
        }

        public double NRE
        {
            get => Declaracao.ExportInd.NRE;
            set => Declaracao.ExportInd.NRE = (long)value;
        }
    }
}
