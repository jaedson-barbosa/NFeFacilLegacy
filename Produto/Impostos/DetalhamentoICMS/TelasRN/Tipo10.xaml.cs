using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class Tipo10 : UserControl, IProcessamentoImposto
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public Tipo10(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosRN.Tipo10(this)
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
