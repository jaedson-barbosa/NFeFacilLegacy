using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoII
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosII dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new ImpostoBase[1] { imposto };
        }

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosII)Tela;
            }
            else if (Detalhamento is ImpostoArmazenado pronto)
            {
                ProcessarDadosProntos(pronto);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto) { }
    }

    sealed class Dados : IDadosII
    {
        public II Imposto { get; set; }
    }
}
