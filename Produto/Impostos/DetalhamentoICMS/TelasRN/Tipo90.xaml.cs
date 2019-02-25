using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo90 : UserControl, IProcessamentoImposto
    {
        public int modBC { get; set; } = -1;
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public int modBCST { get; set; } = -1;
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public Tipo90(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosRN.Tipo90(this)
            {
                CST = Detalhamento.TipoICMSRN.Substring(0, 2),
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
