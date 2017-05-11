using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaCentral.Sincronizacao
{
    internal class ProcessamentoNotas
    {
        private AplicativoContext Contexto { get; }

        internal ProcessamentoNotas(AplicativoContext contexto)
        {
            Contexto = contexto;
        }

        public async Task<NotasFiscais> ObterAsync(DateTime minimo)
        {
            var pasta = new PastaNotasFiscais();
            var retorno = new NotasFiscais
            {
                DIs = new List<NFeDI>(),
                XMLs = new List<XElement>()
            };
            foreach (var nota in from item in Contexto.NotasFiscais.AsNoTracking()
                                 where item.UltimaData > minimo
                                 select item)
            {
                retorno.DIs.Add(nota);
                retorno.XMLs.Add(await pasta.Retornar(nota.Id));
            }

            return retorno;
        }

        public async Task SalvarAsync(NotasFiscais notas)
        {
            var db = new Repositorio.MudancaOtimizadaBancoDados(Contexto);
            var dici = new Dictionary<NFeDI, XElement>();
            for (int i = 0; i < notas.DIs.Count; i++)
            {
                dici.Add(notas.DIs[i], notas.XMLs[i]);
            }
            await db.AdicionarNotasFiscais(dici);
        }
    }
}
