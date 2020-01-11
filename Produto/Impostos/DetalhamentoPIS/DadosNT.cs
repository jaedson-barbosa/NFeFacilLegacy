using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoPIS
{
    sealed class DadosNT : IDadosPIS
    {
        public string CST { private get; set; }

        public object Processar(ProdutoOuServico prod)
        {
            return new PIS
            {
                Corpo = new PISNT(CST)
            };
        }
    }
}
