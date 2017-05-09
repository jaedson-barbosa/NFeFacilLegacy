using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ItensBD;
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
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Add(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Emitente,
                TipoOperacaoRealizada = (int)TipoOperacao.Adicao
            });
        }
        public void Atualizar(Emitente dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Update(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Emitente,
                TipoOperacaoRealizada = (int)TipoOperacao.Edicao
            });
        }
        public void Remover(Emitente dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Remove(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Emitente,
                TipoOperacaoRealizada = (int)TipoOperacao.Remocao
            });
        }
    }
}
