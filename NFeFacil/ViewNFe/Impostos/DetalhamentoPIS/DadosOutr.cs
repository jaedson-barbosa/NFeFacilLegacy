using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosOutr : DadosDuplos
    {
        public override object Processar(ProdutoOuServico prod)
        {
            if (TipoCalculo == TiposCalculo.PorAliquota)
            {
                var vBC = prod.ValorTotal;
                return new PIS
                {
                    Corpo = new PISOutr
                    {
                        CST = CST,
                        vBC = vBC.ToString("F2", CulturaPadrao),
                        pPIS = Aliquota.ToString("F4", CulturaPadrao),
                        vPIS = (vBC * Aliquota / 100).ToString("F2", CulturaPadrao)
                    }
                };
            }
            else
            {
                var qBCProd = prod.QuantidadeComercializada;
                return new PIS
                {
                    Corpo = new PISOutr
                    {
                        CST = CST,
                        qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                        vAliqProd = Valor.ToString("F4", CulturaPadrao),
                        vPIS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                    }
                };
            }
        }
    }
}
