using NFeFacil.ItensBD;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NFeFacil.ImportacaoParaBanco
{
    internal sealed class ImportarNotaFiscal : Importacao
    {
        public ImportarNotaFiscal() : base(".xml") { }

        public async Task Importar()
        {
            var arquivos = await ImportarArquivos();
            using (var db = new AplicativoContext())
            {
                PastaNotasFiscais pasta = new PastaNotasFiscais();
                for (int i = 0; i < arquivos.Count; i++)
                {
                    var xml = XElement.Load(await arquivos[i].OpenStreamForReadAsync());
                    if (xml.Name.LocalName != "nfeProc" && xml.Name.LocalName != "NFe")
                        throw new ArgumentException("Elemento raiz não reconhecido");
                    var nfeDI = NFeDI.Converter(xml);
                    await pasta.AdicionarOuAtualizar(xml, nfeDI.Id);
                    var quant = db.NotasFiscais.Count(x => x.Id == nfeDI.Id);
                    if (quant > 0) db.Update(nfeDI);
                    else db.Add(nfeDI);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
