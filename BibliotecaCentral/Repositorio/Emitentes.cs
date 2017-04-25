using System.Collections.Generic;
using BibliotecaCentral.ItensBD;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Emitentes : ConexaoBanco
    {
        public IEnumerable<Emitente> Registro => Contexto.Emitentes.Include(x => x.endereco);
        public void Adicionar(Emitente dado) => Contexto.Add(dado);
        public void Atualizar(Emitente dado) => Contexto.Update(dado);
        public void Remover(Emitente dado) => Contexto.Remove(dado);
    }
}
