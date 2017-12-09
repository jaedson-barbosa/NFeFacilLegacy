using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewNFe.CaixasImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosST : DadosDuplos
    {
        public override object Processar(ProdutoOuServico prod)
        {
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotalDouble;
                return new Imposto[2]
                {
                        new PIS
                        {
                            Corpo = new PISNT()
                            {
                                CST = CST
                            }
                        },
                        new PISST
                        {
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pPIS = Aliquota.ToString("F4", CulturaPadrao),
                            vPIS = (vBC * Aliquota/ 100).ToString("F2", CulturaPadrao)
                        }
                };
            }
            else
            {
                var qBCProd = prod.QuantidadeComercializada;
                return new Imposto[2]
                {
                        new PIS
                        {
                            Corpo = new PISNT()
                            {
                                CST = CST
                            }
                        },
                        new PISST
                        {
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vPIS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                        }
                };
            }
        }
    }
}
