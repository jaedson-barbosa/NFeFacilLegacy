using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

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
            var dici = new Dictionary<NFeDI, XElement>();
            foreach (var item in from item in Contexto.NotasFiscais
                                 where item.UltimaData > minimo
                                 select item)
            {
                var nota = Contexto.NotasFiscais.Find(item.Id);
                dici.Add(nota, await pasta.Retornar(nota.Id));
            }

            return new NotasFiscais()
            {
                Duplas = dici
            };
        }

        public async Task SalvarAsync(NotasFiscais notas)
        {
            var db = new Repositorio.MudancaOtimizadaBancoDados(Contexto);
            await db.AdicionarNotasFiscais(notas.Duplas);
        }
    }
}
