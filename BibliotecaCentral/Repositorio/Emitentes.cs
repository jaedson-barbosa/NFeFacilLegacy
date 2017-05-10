using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Emitentes : ConexaoBanco
    {
        public Emitentes() : base() { }
        internal Emitentes(AplicativoContext contexto) : base(contexto) { }

        public IEnumerable<Emitente> Registro => Contexto.Emitentes.Include(x => x.endereco);
        public void Adicionar(Emitente dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Add(dado);
        }
        public void Atualizar(Emitente dado)
        {
            dado.UltimaData = DateTime.Now;
            Contexto.Update(dado);
        }
        public void Remover(Emitente dado)
        {
            Contexto.Remove(dado);
        }
    }
}
