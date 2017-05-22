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
        private PastaNotasFiscais Pasta { get; } = new PastaNotasFiscais();

        public IEnumerable<NFeDI> Registro => from not in Contexto.NotasFiscais
                                              orderby not.DataEmissao descending
                                              select not;

        public async Task<IEnumerable<(NFeDI nota, XElement xml)>> RegistroAsync()
        {
            var registroXml = await Pasta.Registro();
            var retorno = new List<(NFeDI nota, XElement xml)>();
            foreach (var item in Registro)
            {
                retorno.Add((item, registroXml.First(x => x.nome == item.Id).xml));
            }
            return retorno;
        }

        public async Task Adicionar(NFeDI nota, XElement xml)
        {
            nota.UltimaData = DateTime.Now;
            var add = Contexto.Add(nota);
            await Pasta.AdicionarOuAtualizar(xml, nota.Id);
        }

        public async Task Atualizar(NFeDI nota, XElement xml)
        {
            nota.UltimaData = DateTime.Now;
            Contexto.Update(nota);
            await Pasta.AdicionarOuAtualizar(xml, nota.Id);
        }

        public async Task Remover(NFeDI nota)
        {
            Contexto.Remove(nota);
            await Pasta.Remover(nota.Id);
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
