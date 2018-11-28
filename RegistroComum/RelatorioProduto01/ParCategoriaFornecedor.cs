using BaseGeral.ItensBD;

namespace RegistroComum.RelatorioProduto01
{
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
}
