using NFeFacil.ItensBD.Produto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMSUFDest
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
