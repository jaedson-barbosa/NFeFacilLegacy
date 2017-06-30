using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

namespace BibliotecaCentral.Validacao
{
    public sealed class ValidadorProduto : IValidavel
    {
        ProdutoOuServico Prod;

        public ValidadorProduto(ProdutoOuServico prod)
        {
            Prod = prod;
        }

        public ValidadorProduto(ProdutoDI prod)
        {
            Prod = prod.ToProdutoOuServico();
        }

        public bool Validar(ILog log)
        {
            return new ValidarDados().ValidarTudo(log,
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.CodigoProduto), "Não foi informado o código do Produto"),
                new ConjuntoAnalise(string.IsNullOrEmpty(Prod.Descricao), "Não foi informada uma breve descrição do Produto"),
                new ConjuntoAnalise(Prod.CFOP == 0, "Não foi informado o CFOP do Produto"));
        }
    }
}
