using System.Collections.Generic;
using System;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco
    {
        public IEnumerable<ProdutoDI> Registro => Contexto.Produtos;

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
