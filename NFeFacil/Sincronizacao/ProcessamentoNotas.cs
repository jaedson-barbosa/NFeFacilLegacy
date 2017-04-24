using NFeFacil.ItensBD;
using NFeFacil.Sincronizacao.Pacotes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFeFacil.Sincronizacao
{
    internal static class ProcessamentoNotas
    {
        public async static Task<NotasFiscais> ObterAsync()
        {
            return new NotasFiscais()
            {
                XMLs = await new PastaNotasFiscais().RegistroCompleto()
            };
        }

        public async static Task SalvarAsync(NotasFiscais notas)
        {
            using (var db = new AplicativoContext())
            {
                var pasta = new PastaNotasFiscais();
                for (int i = 0; i < notas.XMLs.Length; i++)
                {
                    var nfeDI = NFeDI.Converter(notas.XMLs[i]);
                    await pasta.AdicionarOuAtualizar(notas.XMLs[i], nfeDI.Id);
                    var quant = db.NotasFiscais.Count(x => x.Id == nfeDI.Id);
                    if (quant > 0) db.Update(nfeDI);
                    else db.Add(nfeDI);
                }
                db.SaveChanges();
            }
        }
    }
}
