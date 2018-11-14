using BaseGeral.ItensBD;
using System.Collections.Generic;

namespace RegistroComum.RelatorioProduto01
{
    struct DadosRelatorioProduto01
    {
        internal Dictionary<ParCategoriaFornecedor, ExibicaoProduto> Produtos;

        public DadosRelatorioProduto01(Dictionary<ParCategoriaFornecedor, ExibicaoProduto> produtos)
        {
            Produtos = produtos;
        }
    }

    struct ParCategoriaFornecedor
    {
        internal CategoriaDI Categoria;
        internal FornecedorDI Fornecedor;

        public ParCategoriaFornecedor(CategoriaDI categoria, FornecedorDI fornecedor)
        {
            Categoria = categoria;
            Fornecedor = fornecedor;
        }
    }

    struct ExibicaoProduto
    {
        internal string Nome;
        internal string Preco;
        internal string Estoque;

        internal ExibicaoProduto(ProdutoDI produto, double estoque)
        {
            Nome = produto.Descricao;
            Preco = produto.ValorUnitario.ToString("C");
            Estoque = double.IsNaN(estoque) ? "Sem dados" : estoque.ToString("C");
        }
    }
}
