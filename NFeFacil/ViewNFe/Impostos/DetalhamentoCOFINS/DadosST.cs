using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    sealed class DadosST : DadosDuplos
    {
        public override object Processar(ProdutoOuServico prod)
        {
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotal;
                return new Imposto[2]
                {
                        new COFINS
                        {
                            Corpo = new COFINSNT()
                            {
                                CST = CST
                            }
                        },
                        new COFINSST
                        {
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pCOFINS = Aliquota.ToString("F4", CulturaPadrao),
                            vCOFINS = (vBC * Aliquota/ 100).ToString("F2", CulturaPadrao)
                        }
                };
            }
            else
            {
                var qBCProd = prod.QuantidadeComercializada;
                return new Imposto[2]
                {
                        new COFINS
                        {
                            Corpo = new COFINSNT()
                            {
                                CST = CST
                            }
                        },
                        new COFINSST
                        {
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vCOFINS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                        }
                };
            }
        }
    }
}
