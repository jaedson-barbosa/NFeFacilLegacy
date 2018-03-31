using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    public abstract class ProcessamentoImposto
    {
        public IDetalhamentoImposto Detalhamento { protected get; set; }
        public PrincipaisImpostos Tipo => Detalhamento.Tipo;

        public abstract void ProcessarEntradaDados(object Tela);
        protected abstract void ProcessarDadosProntos(ImpostoArmazenado imposto);
        public abstract ImpostoBase[] Processar(DetalhesProdutos prod);
    }
}
