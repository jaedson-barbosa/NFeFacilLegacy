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
            var prod = detalhes.Produto;
            var totalBruto = prod.ValorTotal;
            var frete = string.IsNullOrEmpty(prod.Frete) ? 0 : Parse(prod.Frete);
            var seguro = string.IsNullOrEmpty(prod.Seguro) ? 0 : Parse(prod.Seguro);
            var despesas = string.IsNullOrEmpty(prod.DespesasAcessorias) ? 0 : Parse(prod.DespesasAcessorias);
            var desconto = string.IsNullOrEmpty(prod.Desconto) ? 0 : Parse(prod.Desconto);

            var impCriados = detalhes.Impostos.impostos;
            var vIPI = 0d;
            for (int i = 0; i < impCriados.Count; i++)
            {
                if (impCriados[i] is IPI ipi && ipi.Corpo is IPITrib trib)
                {
                    vIPI = Parse(trib.vIPI);
                }
            }

            return totalBruto + frete + seguro + despesas - desconto + vIPI;
        }
    }
}
