using System.Collections.Generic;
using System;
using System.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco
    {
        public IEnumerable<ProdutoDI> Registro => from prod in Contexto.Produtos
                                                  orderby prod.Descricao
                                                  select prod;

        public void Adicionar(ProdutoDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }

        public void Atualizar(ProdutoDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }

        public void Remover(ProdutoDI dado)
        {
            Contexto.Remove(dado);
        }
    }
}
