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

        protected double CalcularBC(DetalhesProdutos detalhes)
        {
            var prod = detalhes.Produto;
            var totalBruto = prod.ValorTotal;
            var frete = string.IsNullOrEmpty(prod.Frete) ? 0 : Parse(prod.Frete);
            var seguro = string.IsNullOrEmpty(prod.Seguro) ? 0 : Parse(prod.Seguro);
            var despesas = string.IsNullOrEmpty(prod.DespesasAcessorias) ? 0 : Parse(prod.DespesasAcessorias);
            var desconto = string.IsNullOrEmpty(prod.Desconto) ? 0 : Parse(prod.Desconto);

            return totalBruto + frete + seguro + despesas - desconto;
        }

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
