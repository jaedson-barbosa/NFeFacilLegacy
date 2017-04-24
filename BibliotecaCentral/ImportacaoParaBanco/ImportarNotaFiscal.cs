using BibliotecaCentral.ItensBD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BibliotecaCentral.ImportacaoParaBanco
{
    public sealed class ImportarNotaFiscal : Importacao
    {
        public ImportarNotaFiscal() : base(".xml") { }

        public async Task<RelatorioImportacao> Importar()
        {
            var arquivos = await ImportarArquivos();
            var retorno = new RelatorioImportacao();
            Dictionary<NFeDI, XElement> conjuntos = new Dictionary<NFeDI, XElement>();
            for (int i = 0; i < arquivos.Count; i++)
            {
                using (var stream = await arquivos[i].OpenStreamForReadAsync())
                {
                    var xmlAtual = XElement.Load(stream);
                    if (xmlAtual.Name.LocalName != "nfeProc" && xmlAtual.Name.LocalName != "NFe")
                    {
                        retorno.Erros.Add(new XmlNaoReconhecido(arquivos[i].Name, xmlAtual.Name.LocalName, "nfeProc", "NFe"));
                    }
                    else
                    {
                        var diAtual = NFeDI.Converter(xmlAtual);
                        if (conjuntos.Keys.Count(x => x.Id == diAtual.Id) == 0)
                        {
                            conjuntos.Add(diAtual, xmlAtual);
                        }
                        else
                        {
                            var atual = conjuntos.Single(x => x.Key.Id == diAtual.Id);
                            if (atual.Key.Status < diAtual.Status)
                            {
                                conjuntos.Remove(atual.Key);
                                conjuntos.Add(diAtual, xmlAtual);
                            }
                        }
                    }
                }
            }
            using (var db = new AplicativoContext())
            {
                PastaNotasFiscais pasta = new PastaNotasFiscais();
                foreach (var item in conjuntos)
                {
                    await pasta.AdicionarOuAtualizar(item.Value, item.Key.Id);
                    var quant = db.NotasFiscais.Count(x => x.Id == item.Key.Id);
                    if (quant == 1) db.Update(item.Key);
                    else db.Add(item.Key);
                }
                await db.SaveChangesAsync();
            }
            return retorno;
        }
    }
}
