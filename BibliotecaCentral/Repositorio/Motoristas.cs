using System.Collections.Generic;
using System;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Motoristas : ConexaoBanco
    {
        public IEnumerable<MotoristaDI> Registro => Contexto.Motoristas;

        public void Adicionar(MotoristaDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }

        public void Atualizar(MotoristaDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }

        public void Remover(MotoristaDI dado)
        {
            Contexto.Remove(dado);
        }
    }
}
