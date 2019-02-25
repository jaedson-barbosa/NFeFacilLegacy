using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos.DetalhamentoICMS.TelasSN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo101 : UserControl, IProcessamentoImposto
    {
        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public Tipo101(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosSN.Tipo101(this)
            {
                CSOSN = Detalhamento.TipoICMSSN,
                Origem = Detalhamento.Origem
            };
            var imposto = new ICMS
            {
                Corpo = (ComumICMS)dados.Processar(prod)
            };
            return new IImposto[1] { imposto };
        }
    }
}
