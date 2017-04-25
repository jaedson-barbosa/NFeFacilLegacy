using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco
    {
        public IEnumerable<Destinatario> Registro => Contexto.Clientes.Include(x => x.endereco);
        public void Adicionar(Destinatario cliente) => Contexto.Add(cliente);
        public void Atualizar(Destinatario cliente) => Contexto.Update(cliente);
        public void Remover(Destinatario dado) => Contexto.Remove(dado);
    }
}
