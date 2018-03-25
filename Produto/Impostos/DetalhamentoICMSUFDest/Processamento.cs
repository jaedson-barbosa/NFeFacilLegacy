using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;

namespace Produto.Impostos.DetalhamentoICMSUFDest
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMSUFDest dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new ImpostoBase[1] { imposto };
        }

        public override bool ValidarDados() => dados != null;

        public override void ProcessarEntradaDados(object Tela)
        {
            if (Detalhamento is Detalhamento detalhamento && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosICMSUFDest)Tela;
            }
            else if (Detalhamento is DadoPronto pronto)
            {
                ProcessarDadosProntos(pronto.ImpostoPronto);
            }
        }

        protected override void ProcessarDadosProntos(ImpostoArmazenado imposto) { }
    }
}
