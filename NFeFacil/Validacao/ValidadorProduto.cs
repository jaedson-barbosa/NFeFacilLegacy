using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.Validacao
{
    public sealed class ValidadorProduto : IValidavel
    {
        DadosBaseProdutoOuServico Prod;

        public ValidadorProduto(DadosBaseProdutoOuServico prod)
        {
            Prod = prod;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.CodigoProduto), "Não foi informado o código do produto"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.Descricao), "Não foi informada uma breve descrição do produto"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.CFOP), "Não foi informado o CFOP do produto"));
        }
    }
}
