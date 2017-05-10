using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco
    {
        public IEnumerable<Destinatario> Registro => Contexto.Clientes.Include(x => x.endereco);

        public void Adicionar(Destinatario cliente)
        {
            cliente.UltimaData = DateTime.Now;
            Contexto.Add(cliente);
        }

        public void Atualizar(Destinatario cliente)
        {
            cliente.UltimaData = DateTime.Now;
            Contexto.Update(cliente);
        }

        public void Remover(Destinatario cliente)
        {
            Contexto.Remove(cliente);
        }
    }
}
