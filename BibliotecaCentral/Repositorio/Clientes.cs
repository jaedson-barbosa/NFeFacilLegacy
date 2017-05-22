using System.Collections.Generic;
using System;
using System.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco
    {
        public IEnumerable<ClienteDI> Registro => from cli in Contexto.Clientes
                                                  orderby cli.Nome
                                                  select cli;

        public void Adicionar(ClienteDI cliente)
        {
            cliente.UltimaData = DateTime.Now;
            Contexto.Add(cliente);
        }

        public void Atualizar(ClienteDI cliente)
        {
            cliente.UltimaData = DateTime.Now;
            Contexto.Update(cliente);
        }

        public void Remover(ClienteDI cliente)
        {
            Contexto.Remove(cliente);
        }
    }
}
