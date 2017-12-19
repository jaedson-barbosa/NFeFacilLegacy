using NFeFacil.ItensBD;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace NFeFacil.Importacao
{
    public sealed class ImportarNotaFiscal : Importacao
    {
        public ImportarNotaFiscal() : base(".xml") { }

        public async Task<List<Exception>> ImportarAsync()
        {
            var arquivos = await ImportarArquivos();
            var retorno = new List<Exception>();
            List<NFeDI> conjuntos = new List<NFeDI>();
            for (int i = 0; i < arquivos.Count; i++)
            {
                try
                {
                    var xmlAtual = await ObterXML(arquivos[i]);
                    var diAtual = NFeDI.Converter(xmlAtual);

                    if (conjuntos.Count(x => x.Id == diAtual.Id) == 0)
                    {
                        conjuntos.Add(diAtual);
                    }
                    else
                    {
                        var atual = conjuntos.Single(x => x.Id == diAtual.Id);
                        if (atual.Status < diAtual.Status)
                        {
                            conjuntos.Remove(atual);
                            conjuntos.Add(diAtual);
                        }
                    }
                }
                catch (Exception e)
                {
                    retorno.Add(e);
                }
            }
            using (var repo = new Repositorio.MEGACLASSE())
            {
                repo.AdicionarNotasFiscais(conjuntos, Propriedades.DateTimeNow);
            }
            return retorno;
        }

        internal static async Task<XElement> ObterXML(StorageFile arquivo)
        {
            const string namespaceAtual = "http://www.portalfiscal.inf.br/nfe";

            using (var stream = await arquivo.OpenStreamForReadAsync())
            {
                var xmlAtual = XElement.Load(stream);
                if (xmlAtual.Name.LocalName != "nfeProc" && xmlAtual.Name.LocalName != "NFe")
                {
                    new XmlNaoReconhecido(arquivo.Name, xmlAtual.Name.LocalName, "nfeProc", "NFe");
                }

                var filhoIdent = xmlAtual.Name.LocalName == "NFe"
                ? xmlAtual : xmlAtual.Element(XName.Get("NFe", namespaceAtual));
                filhoIdent = filhoIdent.Element(XName.Get("infNFe", namespaceAtual));
                filhoIdent = filhoIdent.Element(XName.Get("ide", namespaceAtual));

                var antigaDataEmi = filhoIdent.Element(XName.Get("dEmi", namespaceAtual));
                if (antigaDataEmi != null)
                {
                    antigaDataEmi.Name = XName.Get("dhEmi", namespaceAtual);
                    antigaDataEmi.Value = DateTime.ParseExact(
                        antigaDataEmi.Value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture).ToStringPersonalizado();
                }

                var antigaDataSaidaEntrada = filhoIdent.Element(XName.Get("dSaiEnt", namespaceAtual));
                if (antigaDataSaidaEntrada != null)
                {
                    antigaDataSaidaEntrada.Name = XName.Get("dhSaiEnt", namespaceAtual);
                    antigaDataSaidaEntrada.Value = DateTime.ParseExact(
                        antigaDataSaidaEntrada.Value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture).ToStringPersonalizado();
                }

                return xmlAtual;
            }
        }
    }
}
