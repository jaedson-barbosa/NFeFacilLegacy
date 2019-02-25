using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosSN
{
    public abstract class BaseSN : IDadosICMS
    {
        public string CSOSN { get; set; }
        public int Origem { get; set; }

        public abstract object Processar(DetalhesProdutos prod);

        protected double CalcularBC(DetalhesProdutos detalhes)
        {
            var impCriados = detalhes.Impostos.impostos;
            var vIPI = 0d;
            for (int i = 0; i < impCriados.Count; i++)
                if (impCriados[i] is IPI ipi && ipi.Corpo is IPITrib trib)
                    vIPI = Parse(trib.ValorIPI);

            return detalhes.Produto.ValorTotal
                + detalhes.Produto.Frete
                + detalhes.Produto.Seguro
                + detalhes.Produto.DespesasAcessorias
                - detalhes.Produto.Desconto + vIPI;
        }
    }
}
