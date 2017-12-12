using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    sealed class DadosOutr : DadosDuplos
    {
        public override object Processar(ProdutoOuServico prod)
        {
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotal;
                return new COFINS
                {
                    Corpo = new COFINSOutr
                    {
                        CST = CST,
                        vBC = ToStr(vBC),
                        pCOFINS = ToStr(Aliquota, "F4"),
                        vCOFINS = ToStr(vBC * Aliquota / 100)
                    }
                };
            }
            else
            {
                var qBCProd = prod.QuantidadeComercializada;
                return new COFINS
                {
                    Corpo = new COFINSOutr
                    {
                        CST = CST,
                        qBCProd = ToStr(qBCProd, "F4"),
                        vAliqProd = ToStr(Valor, "F4"),
                        vCOFINS = ToStr(qBCProd * Valor)
                    }
                };
            }
        }
    }
}
