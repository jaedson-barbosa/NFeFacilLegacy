using System.Collections.Generic;
using BibliotecaCentral.ItensBD;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Emitentes : ConexaoBanco<EmitenteDI, EmitenteDI>
    {
        public IEnumerable<EmitenteDI> Registro => Contexto.Emitentes.Include(x => x.endereco);
        public void Adicionar(EmitenteDI dado) => Contexto.Add(dado);
        public void Atualizar(EmitenteDI dado) => Contexto.Update(dado);
        public void Remover(EmitenteDI dado) => Contexto.Remove(dado);
    }
}
