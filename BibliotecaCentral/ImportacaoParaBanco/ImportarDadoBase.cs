using BibliotecaCentral.ItensBD;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace BibliotecaCentral.ImportacaoParaBanco
{
    internal sealed class ImportarDadoBase : Importacao
    {
        private TiposDadoBasico TipoDado;
        private IReadOnlyList<StorageFile> arquivos;

        public ImportarDadoBase(TiposDadoBasico tipoDado) : base(".xml")
        {
            TipoDado = tipoDado;
        }

        public async Task<RelatorioImportacao> Importar()
        {
            arquivos = await ImportarArquivos();
            var listaXML = await Task.WhenAll(arquivos.Select(async x =>
            {
                using (var stream = await x.OpenStreamForReadAsync())
                {
                    return XElement.Load(stream);
                }
            }));
            switch (TipoDado)
            {
                case TiposDadoBasico.Emitente:
                    return await AnaliseCompletaXml<Emitente, EmitenteDI>(listaXML, nameof(Emitente), "emit");
                case TiposDadoBasico.Cliente:
                    return await AnaliseCompletaXml<Destinatario, ClienteDI>(listaXML, nameof(Destinatario), "dest");
                case TiposDadoBasico.Motorista:
                    return await AnaliseCompletaXml<Motorista, MotoristaDI>(listaXML, nameof(Motorista), "transporta");
                case TiposDadoBasico.Produto:
                    return await AnaliseCompletaXml<BaseProdutoOuServico, ProdutoDI>(listaXML, nameof(BaseProdutoOuServico), "prod");
                default:
                    return null;
            }
        }

        private async Task<RelatorioImportacao> AnaliseCompletaXml<TipoBase, TipoBanco>(XElement[] listaXML, string nomePrimario, string nomeSecundario) where TipoBase : class where TipoBanco : IId, IConverterDI<TipoBase>, new()
        {
            var retorno = new RelatorioImportacao();
            using (var db = new AplicativoContext())
            {
                for (int i = 0; i < listaXML.Length; i++)
                {
                    var resultado = RemoverNamespace(Busca(listaXML[i], nomePrimario, nomeSecundario));
                    if (resultado == null)
                    {
                        retorno.Erros.Add(new XmlNaoReconhecido(arquivos[i].Name, listaXML[i].Name.LocalName, nomeSecundario, nameof(TipoBase)));
                        continue;
                    }
                    var xml = resultado;
                    xml.Name = nomePrimario;
                    db.Add(new TipoBanco().Converter(xml.FromXElement<TipoBase>()));
                }
                await db.SaveChangesAsync();
            }
            return retorno;
        }

        private XElement Busca(XElement universo, params string[] nome)
        {
            if (nome.Contains(universo.Name.LocalName))
            {
                return universo;
            }
            else
            {
                var filhos = universo.Elements().ToArray();
                for (int i = 0; i < filhos.Length; i++)
                {
                    if (nome.Contains(filhos[i].Name.LocalName))
                    {
                        return filhos[i];
                    }
                    else if (filhos[i].HasElements)
                    {
                        var resultadoProfundo = Busca(filhos[i], nome);
                        if (resultadoProfundo != null)
                        {
                            return resultadoProfundo;
                        }
                    }
                }
            }
            return null;
        }

        private static XElement RemoverNamespace(XElement xmlBruto)
        {
            return new XElement(
                xmlBruto.Name.LocalName,
                xmlBruto.HasElements ?
                    xmlBruto.Elements().Select(el => RemoverNamespace(el)) :
                    (object)xmlBruto.Value);
        }
    }
}
