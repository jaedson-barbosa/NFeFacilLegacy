using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos.DetalhamentoCOFINS
{
    sealed class DadosNT : IDadosCOFINS
    {
        public string CST { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            return new COFINS
            {
                Corpo = new COFINSNT()
                {
                    CST = CST
                }
            };
        }
    }
}
