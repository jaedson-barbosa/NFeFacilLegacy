using BaseGeral.ItensBD;
using System.Collections.Generic;

namespace RegistroComum.RelatorioProduto01
{
    struct DadosRelatorioProduto01
    {
        internal Dictionary<ParCategoriaFornecedor, ExibicaoProduto[]> Produtos;

        public DadosRelatorioProduto01(Dictionary<ParCategoriaFornecedor, ExibicaoProduto[]> produtos)
        {
            Produtos = produtos;
        }
    }

    struct ParCategoriaFornecedor
    {
        internal readonly string Categoria;
        internal readonly string Fornecedor;

        public ParCategoriaFornecedor(CategoriaDI categoria, FornecedorDI fornecedor)
        {
            Categoria = categoria?.Nome ?? "Sem categoria";
            Fornecedor = fornecedor?.Nome ?? "Fornecedor desconhecido";
        }
    }

    struct ExibicaoProduto
    {
        internal bool Adicionado;
        internal readonly string Codigo;
        internal readonly string Nome;
        internal readonly string Preco;
        internal readonly string Estoque;

        internal ExibicaoProduto(ProdutoDI produto, double estoque)
        {
            Adicionado = false;
            Codigo = produto.CodigoProduto;
            Nome = produto.Descricao;
            Preco = produto.ValorUnitario.ToString("C");
            Estoque = double.IsNaN(estoque) ? "Sem dados" : estoque.ToString("C");
        }
    }
}
