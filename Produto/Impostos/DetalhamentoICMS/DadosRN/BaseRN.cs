using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public abstract class BaseRN : IDadosICMS
    {
        public string CST { get; set; }
        public int Origem { get; set; }

        public abstract object Processar(DetalhesProdutos prod);

        protected double CalcularBC(DetalhesProdutos detalhes) => detalhes.Produto.ValorTotal
            + detalhes.Produto.Frete
            + detalhes.Produto.Seguro
            + detalhes.Produto.DespesasAcessorias
            - detalhes.Produto.Desconto;

        protected double ObterIPI(DetalhesProdutos detalhes)
        {
            var impCriados = detalhes.Impostos.impostos;
            for (int i = 0; i < impCriados.Count; i++)
                if (impCriados[i] is IPI ipi && ipi.Corpo is IPITrib trib && !string.IsNullOrEmpty(trib.ValorIPI))
                    return Parse(trib.ValorIPI);
            return 0;
        }
    }
}
