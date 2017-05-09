using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class NotasFiscais : ConexaoBanco
    {
        private PastaNotasFiscais pasta { get; }

        public NotasFiscais() : base()
        {
            pasta = new PastaNotasFiscais();
        }

        internal NotasFiscais(AplicativoContext contexto) : base()
        {
            pasta = new PastaNotasFiscais();
        }

        public IEnumerable<NFeDI> Registro => Contexto.NotasFiscais;

        public async Task<IEnumerable<(NFeDI nota, XElement xml)>> RegistroAsync()
        {
            PastaNotasFiscais pasta = new PastaNotasFiscais();
            var registroXml = await pasta.RegistroCompleto();
            var retorno = new List<(NFeDI nota, XElement xml)>();
            foreach (var item in Registro)
            {
                retorno.Add((item, registroXml.First(x => x.nome == item.IdNotaFiscal).xml));
            }
            return retorno;
        }

        public async Task Adicionar(NFeDI nota, XElement xml)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Add(nota).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.NotaFiscal,
                TipoOperacaoRealizada = (int)TipoOperacao.Adicao
            });

            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.AdicionarOuAtualizar(xml, nota.IdNotaFiscal);
        }

        public async Task Atualizar(NFeDI nota, XElement xml)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Update(nota).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.NotaFiscal,
                TipoOperacaoRealizada = (int)TipoOperacao.Edicao
            });

            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.AdicionarOuAtualizar(xml, nota.IdNotaFiscal);
        }

        public async Task Remover(NFeDI nota)
        {
            Contexto.Add(new RegistroMudanca
            {
                Id = Contexto.Remove(nota).Entity.Id,
                MomentoMudanca = DateTime.Now,
                TipoDadoModificado = (int)TipoDado.NotaFiscal,
                TipoOperacaoRealizada = (int)TipoOperacao.Remocao
            });

            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.Remover(nota.IdNotaFiscal);
        }

        public long ObterNovoNumero(string cnpjEmitente, ushort serieNota)
        {
            return (from nota in Contexto.NotasFiscais
                    where nota.CNPJEmitente == cnpjEmitente
                    where nota.SerieNota == serieNota
                    select nota.NumeroNota).Max() + 1;
        }
    }
}
