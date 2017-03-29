using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;

namespace NFeFacil.ImportacaoParaBanco
{
    internal sealed class ImportarDadoBase : Importacao
    {
        private TiposDadoBásico TipoDado;
        private IReadOnlyList<StorageFile> arquivos;

        public ImportarDadoBase(TiposDadoBásico tipoDado) : base(".xml")
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
                case TiposDadoBásico.Emitente:
                    return await AnaliseCompletaXml<Emitente>(listaXML, "emit");
                case TiposDadoBásico.Cliente:
                    return await AnaliseCompletaXml<Destinatario>(listaXML, "dest");
                case TiposDadoBásico.Motorista:
                    return await AnaliseCompletaXml<Motorista>(listaXML, "transporta");
                case TiposDadoBásico.Produto:
                    return await AnaliseCompletaXml<DadosBaseProdutoOuServico>(listaXML, "prod");
                default:
                    return null;
            }
        }

        private async Task<RelatorioImportacao> AnaliseCompletaXml<Tipo>(XElement[] listaXML, string nomeSecundario) where Tipo : class
        {
            var retorno = new RelatorioImportacao();
            using (var db = new AplicativoContext())
            {
                for (int i = 0; i < listaXML.Length; i++)
                {
                    var resultado = Busca(listaXML[i], nomeSecundario, nameof(Tipo));
                    if (resultado == null)
                    {
                        retorno.Erros.Add(new XmlNaoReconhecido(arquivos[i].Name, listaXML[i].Name.LocalName, nomeSecundario, nameof(Tipo)));
                        continue;
                    }
                    var xml = resultado;
                    xml.Name = nameof(Tipo);
                    db.Add(xml.FromXElement<Tipo>());
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
    }

    public enum TiposDadoBásico
    {
        Emitente,
        Cliente,
        Motorista,
        Produto
    }
}
