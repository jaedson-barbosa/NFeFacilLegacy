using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    public sealed class DetalharVazio : IProcessamentoImposto
    {
        readonly int CST;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.COFINS;

        public DetalharVazio(int cst) => CST = cst;

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = new DadosNT()
            {
                CST = CST.ToString("00")
            }.Processar(prod.Produto);
            if (resultado is IImposto[] list) return list;
            else return new IImposto[1] { (COFINS)resultado };
        }
    }
}
