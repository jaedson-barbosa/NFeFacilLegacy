using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Sincronizacao.Pacotes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.Sincronizacao
{
    internal static class ProcessamentoNotas
    {
        public async static Task<NotasFiscais> ObterAsync()
        {
            var regXml = await new PastaNotasFiscais().RegistroCompleto();
            var dici = new Dictionary<NFeDI, XElement>(regXml.Count);
            using (var db = new AplicativoContext())
            {
                for (int i = 0; i < regXml.Count; i++)
                {
                    var di = db.NotasFiscais.Find(regXml[i].nome);
                    if (di != null)
                    {
                        dici.Add(di, regXml[i].xml);
                    }
                }
            }

            return new NotasFiscais()
            {
                Duplas = dici
            };
        }

        public async static Task SalvarAsync(NotasFiscais notas)
        {
            using (var db = new Repositorio.MudancaOtimizadaBancoDados())
            {
                await db.AdicionarNotasFiscais(notas.Duplas);
            }
        }
    }
}
