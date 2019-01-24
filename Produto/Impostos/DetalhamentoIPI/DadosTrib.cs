using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoIPI
{
    class DadosTrib : DadosIPI
    {
        public double Aliquota { protected get; set; }
        public double Valor { protected get; set; }
        public TiposCalculo TipoCalculo { protected get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            ComumIPI corpo;
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotal;
                var pIPI = Aliquota;
                corpo = new IPITrib(CST, vBC, pIPI, false);
            }
            else
            {
                var qUnid = prod.QuantidadeComercializada;
                var vUnid = Valor;
                corpo = new IPITrib(CST, qUnid, vUnid, true);
            }
            PreImposto.Corpo = corpo;
            return PreImposto;
        }
    }
}
