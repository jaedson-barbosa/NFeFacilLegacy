using System.Collections.Generic;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Motoristas : ConexaoBanco<MotoristaDI, MotoristaDI>
    {
        public IEnumerable<MotoristaDI> Registro => Contexto.Motoristas;
        public void Adicionar(MotoristaDI dado) => Contexto.Add(dado);
        public void Atualizar(MotoristaDI dado) => Contexto.Update(dado);
        public void Remover(MotoristaDI dado) => Contexto.Remove(dado);
    }
}
