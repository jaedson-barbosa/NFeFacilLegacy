using System.Collections.Generic;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco<ProdutoDI, ProdutoDI>
    {
        public IEnumerable<ProdutoDI> Registro => Contexto.Produtos;
        public void Adicionar(ProdutoDI dado) => Contexto.Add(dado);
        public void Atualizar(ProdutoDI dado) => Contexto.Update(dado);
        public void Remover(ProdutoDI dado) => Contexto.Remove(dado);
    }
}
