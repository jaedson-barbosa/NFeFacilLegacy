using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using BibliotecaCentral.ItensBD;

namespace BibliotecaCentral.Repositorio
{
    public sealed class NotasFiscais : ConexaoBanco<(NFeDI nota, XElement xml), NFeDI>
    {
        public IEnumerable<NFeDI> Registro => Contexto.NotasFiscais;

        public async Task<IEnumerable<(NFeDI nota, XElement xml)>> RegistroAsync()
        {
            PastaNotasFiscais pasta = new PastaNotasFiscais();
            var registroXml = await pasta.RegistroCompleto();
            var retorno = new List<(NFeDI nota, XElement xml)>();
            foreach (var item in Registro)
            {
                retorno.Add((item, registroXml.First(x => x.nome == item.Id).xml));
            }
            return retorno;
        }

        public async Task Adicionar((NFeDI nota, XElement xml) dado)
        {
            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.AdicionarOuAtualizar(dado.xml, dado.nota.Id);
            Contexto.Add(dado.nota);
        }

        public async Task Atualizar((NFeDI nota, XElement xml) dado)
        {
            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.AdicionarOuAtualizar(dado.xml, dado.nota.Id);
            Contexto.Update(dado.nota);
        }

        public async Task Remover(NFeDI nota)
        {
            PastaNotasFiscais pasta = new PastaNotasFiscais();
            await pasta.Remover(nota.Id);
            Contexto.Remove(nota);
        }
    }
}
