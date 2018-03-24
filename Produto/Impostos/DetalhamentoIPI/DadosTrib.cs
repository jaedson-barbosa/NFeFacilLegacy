using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace NFeFacil.Produto.Impostos.DetalhamentoIPI
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
                corpo = new IPITrib
                {
                    vBC = ToStr(vBC),
                    pIPI = ToStr(pIPI, "F4"),
                    vIPI = ToStr(vBC * pIPI / 100)
                };
            }
            else
            {
                var qUnid = prod.QuantidadeComercializada;
                var vUnid = Valor;
                corpo = new IPITrib
                {
                    qUnid = ToStr(qUnid, "F4"),
                    vUnid = ToStr(vUnid, "F4"),
                    vIPI = ToStr(qUnid * vUnid)
                };
            }
            PreImposto.Corpo = corpo;
            return PreImposto;
        }
    }
}
