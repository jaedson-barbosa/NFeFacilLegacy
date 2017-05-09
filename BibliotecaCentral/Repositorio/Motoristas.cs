using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Motoristas : ConexaoBanco
    {
        public Motoristas() : base(){ }
        internal Motoristas(AplicativoContext contexto) : base(contexto) { }

        public IEnumerable<Motorista> Registro => Contexto.Motoristas;
        public void Adicionar(Motorista dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Add(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Motorista,
                TipoOperacaoRealizada = (int)TipoOperacao.Adicao
            });
        }
        public void Atualizar(Motorista dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Update(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Motorista,
                TipoOperacaoRealizada = (int)TipoOperacao.Edicao
            });
        }
        public void Remover(Motorista dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Remove(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Motorista,
                TipoOperacaoRealizada = (int)TipoOperacao.Remocao
            });
        }
    }
}
