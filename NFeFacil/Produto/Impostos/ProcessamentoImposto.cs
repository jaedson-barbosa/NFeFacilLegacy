using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Produto.Impostos
{
    public abstract class ProcessamentoImposto
    {
        public IDetalhamentoImposto Detalhamento { protected get; set; }
        public PrincipaisImpostos Tipo => Detalhamento.Tipo;

        public abstract void ProcessarEntradaDados(object Tela);
        public abstract bool ValidarDados();
        protected abstract void ProcessarDadosProntos(ImpostoArmazenado imposto);
        public abstract ImpostoBase[] Processar(DetalhesProdutos prod);
    }
}
