using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoIPI
{
    class DadosTrib : DadosIPI
    {
        public double Aliquota { protected get; set; }
        public double Valor { protected get; set; }
        public TiposCalculo TipoCalculo { protected get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            ComumIPI corpo = TipoCalculo == TiposCalculo.PorAliquota
                ? new IPITrib(CST, prod.ValorTotal, Aliquota, false)
                : new IPITrib(CST, prod.QuantidadeComercializada, Valor, true);
            PreImposto.Corpo = corpo;
            return PreImposto;
        }
    }
}
