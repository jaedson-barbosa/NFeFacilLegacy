using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace NFeFacil.Validacao
{
    public sealed class ValidadorProduto : IValidavel
    {
        BaseProdutoOuServico Prod;

        public ValidadorProduto(BaseProdutoOuServico prod)
        {
            Prod = prod;
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.CodigoProduto), "Não foi informado o código do Produto"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.Descricao), "Não foi informada uma breve descrição do Produto"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.CFOP), "Não foi informado o CFOP do Produto"));
        }
    }
}
