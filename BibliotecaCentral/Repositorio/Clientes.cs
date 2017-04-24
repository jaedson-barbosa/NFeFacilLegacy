using BibliotecaCentral.ItensBD;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco<ClienteDI, ClienteDI>
    {
        public IEnumerable<ClienteDI> Registro => Contexto.Clientes.Include(x => x.endereco);
        public void Adicionar(ClienteDI cliente) => Contexto.Add(cliente);
        public void Atualizar(ClienteDI cliente) => Contexto.Update(cliente);
        public void Remover(ClienteDI dado) => Contexto.Remove(dado);
    }
}
