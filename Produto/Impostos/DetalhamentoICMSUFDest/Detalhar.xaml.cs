using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMSUFDest
{
    [DetalhePagina("ICMS para a UF de destino")]
    public sealed partial class Detalhar : UserControl, IProcessamentoImposto
    {
        readonly ICMSUFDest Imposto = new ICMSUFDest();
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMSUFDest;

        public Detalhar()
        {
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            return new IImposto[1] { Imposto };
        }
    }
}
