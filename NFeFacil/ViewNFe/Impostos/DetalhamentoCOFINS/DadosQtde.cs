using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    sealed class DadosQtde : DadosCOFINS
    {
        public double Valor { private get; set; }

        public override object Processar(ProdutoOuServico prod)
        {
            var qBCProd = prod.QuantidadeComercializada;
            var vAliqProd = Valor;
            return new COFINS
            {
                Corpo = new COFINSQtde
                {
                    CST = CST,
                    qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                    vAliqProd = vAliqProd.ToString("F4", CulturaPadrao),
                    vCOFINS = (qBCProd * vAliqProd).ToString("F2", CulturaPadrao)
                }
            };
        }
    }
}
