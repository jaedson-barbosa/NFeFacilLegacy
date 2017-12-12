using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosQtde : IDadosPIS
    {
        public string CST { private get; set; }
        public double Valor { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            var qBCProd = prod.QuantidadeComercializada;
            var vAliqProd = Valor;
            return new PIS
            {
                Corpo = new PISQtde
                {
                    CST = CST,
                    qBCProd = ToStr(qBCProd, "F4"),
                    vAliqProd = ToStr(vAliqProd, "F4"),
                    vPIS = ToStr(qBCProd * vAliqProd)
                }
            };
        }
    }
}
