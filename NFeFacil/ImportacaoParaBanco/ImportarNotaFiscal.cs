using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NFeFacil.ImportacaoParaBanco
{
    internal sealed class ImportarNotaFiscal : Importacao
    {
        public ImportarNotaFiscal() : base(".xml") { }

        public async Task<RelatorioImportacao> Importar()
        {
            var arquivos = await ImportarArquivos();
            var retorno = new RelatorioImportacao();
            using (var db = new AplicativoContext())
            {
                PastaNotasFiscais pasta = new PastaNotasFiscais();
                for (int i = 0; i < arquivos.Count; i++)
                {
                    using (var stream = await arquivos[i].OpenStreamForReadAsync())
                    {
                        var xml = XElement.Load(stream);
                        if (xml.Name.LocalName != "nfeProc" && xml.Name.LocalName != "NFe")
                        {
                            retorno.Erros.Add(new XmlNaoReconhecido(arquivos[i].Name, xml.Name.LocalName));
                            continue;
                        }
                        var nfeDI = NFeDI.Converter(xml);
                        await pasta.AdicionarOuAtualizar(xml, nfeDI.Id);
                        var quant = db.NotasFiscais.Count(x => x.Id == nfeDI.Id);
                        if (quant == 1) db.Update(nfeDI);
                        else db.Add(nfeDI);
                    }
                }
                await db.SaveChangesAsync();
            }
            return retorno;
        }
    }

    internal struct XmlNaoReconhecido
    {
        public string NomeArquivo { get; }
        public string TagRaiz { get; }
        public string[] TagsEsperadas { get; }

        public XmlNaoReconhecido(string nomeArquivo, string tagRaiz, params string[] tagsEsperadas)
        {
            NomeArquivo = nomeArquivo;
            TagRaiz = tagRaiz;
            TagsEsperadas = tagsEsperadas;
        }
    }

    internal class RelatorioImportacao
    {
        public ResumoRelatorioImportacao Analise
        {
            get => Erros.Count == 0 ? ResumoRelatorioImportacao.Sucesso : ResumoRelatorioImportacao.Erro;
        }

        public List<XmlNaoReconhecido> Erros { get; set; } = new List<XmlNaoReconhecido>();
    }

    internal enum ResumoRelatorioImportacao
    {
        Sucesso,
        Erro
    }
}
