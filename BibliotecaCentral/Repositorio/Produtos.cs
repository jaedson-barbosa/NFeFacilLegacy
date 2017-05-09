using System.Collections.Generic;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ItensBD;
using System;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Produtos : ConexaoBanco
    {
        public Produtos() : base() { }
        internal Produtos(AplicativoContext contexto) : base(contexto) { }

        public IEnumerable<BaseProdutoOuServico> Registro => Contexto.Produtos;
        public void Adicionar(BaseProdutoOuServico dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Add(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Produto,
                TipoOperacaoRealizada = (int)TipoOperacao.Adicao
            });
        }
        public void Atualizar(BaseProdutoOuServico dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Update(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Produto,
                TipoOperacaoRealizada = (int)TipoOperacao.Edicao
            });
        }
        public void Remover(BaseProdutoOuServico dado)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Remove(dado).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.Produto,
                TipoOperacaoRealizada = (int)TipoOperacao.Remocao
            });
        }
    }
}
