using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoICMS.TelasRN
{
    [DetalhePagina("ICMS")]
    public sealed partial class TipoICMSST : UserControl, IProcessamentoImposto
    {
        public string vBCSTRet { get; set; }
        public string vICMSSTRet { get; set; }
        public string vBCSTDest { get; set; }
        public string vICMSSTDest { get; set; }

        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;

        public TipoICMSST(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var dados = new DadosRN.TipoICMSST(this)
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
