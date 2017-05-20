using System.Collections.Generic;
using System;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco
    {
        public IEnumerable<ClienteDI> Registro => Contexto.Clientes;

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
