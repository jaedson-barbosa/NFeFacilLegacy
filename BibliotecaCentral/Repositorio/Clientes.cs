using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ItensBD;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Clientes : ConexaoBanco
    {
        public Clientes() : base() { }
        internal Clientes(AplicativoContext contexto) : base(contexto) { }

        public IEnumerable<Destinatario> Registro => Contexto.Clientes.Include(x => x.endereco);
        public void Adicionar(Destinatario cliente)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Add(cliente).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Cliente,
                TipoOperacaoRealizada = (int)TipoOperacao.Adicao
            });
        }

        public void Atualizar(Destinatario cliente)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Update(cliente).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Cliente,
                TipoOperacaoRealizada = (int)TipoOperacao.Edicao
            });
        }

        public void Remover(Destinatario cliente)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Remove(cliente).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Cliente,
                TipoOperacaoRealizada = (int)TipoOperacao.Remocao
            });
        }
    }
}
