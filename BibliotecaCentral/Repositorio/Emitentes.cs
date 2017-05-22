using System.Collections.Generic;
using System;
using System.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Emitentes : ConexaoBanco
    {
        public IEnumerable<EmitenteDI> Registro => from emi in Contexto.Emitentes
                                                   orderby emi.Nome
                                                   select emi;

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
