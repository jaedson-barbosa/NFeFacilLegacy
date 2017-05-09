using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<NotasFiscais> ObterAsync()
        {
            var regXml = await new PastaNotasFiscais().RegistroCompleto();
            var dici = new Dictionary<NFeDI, XElement>(regXml.Count);
            for (int i = 0; i < regXml.Count; i++)
            {
                var di = Contexto.NotasFiscais.Find(regXml[i].nome);
                if (di != null)
                {
                    dici.Add(di, regXml[i].xml);
                }
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
