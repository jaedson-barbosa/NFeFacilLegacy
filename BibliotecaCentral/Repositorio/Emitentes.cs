using System.Collections.Generic;
using System;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Emitentes : ConexaoBanco
    {
        public IEnumerable<EmitenteDI> Registro => Contexto.Emitentes;

        public void Adicionar(EmitenteDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }

        public void Atualizar(EmitenteDI dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }

        public void Remover(EmitenteDI dado)
        {
            Contexto.Remove(dado);
        }
    }
}
