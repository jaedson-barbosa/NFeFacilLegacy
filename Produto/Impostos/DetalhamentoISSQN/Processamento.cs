using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;

namespace Venda.Impostos.DetalhamentoISSQN
{
    public sealed class Processamento : ProcessamentoImposto
    {
        IDadosISSQN dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new ImpostoBase[1] { imposto };
        }

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento
                && AssociacoesSimples.ISSQN[detalhamento.Exterior] == Tela?.GetType())
            {
                dados = (IDadosISSQN)Tela;
            }
            else if (Detalhamento is ImpostoArmazenado pronto)
            {
                ProcessarDadosProntos(pronto);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto) { }
    }
}
