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

namespace BibliotecaCentral.Importacao
{
    public sealed class ImportarDadoBase : Importacao
    {
        private TiposDadoBasico TipoDado;
        private IReadOnlyList<StorageFile> arquivos;

        public ImportarDadoBase(TiposDadoBasico tipoDado) : base(".xml")
        {
            TipoDado = tipoDado;
        }

        public override async Task<List<Exception>> ImportarAsync()
        {
            arquivos = await ImportarArquivos();
            var listaXML = await Task.WhenAll(arquivos.Select(async x =>
            {
                using (var stream = await x.OpenStreamForReadAsync())
                {
                    return XElement.Load(stream);
                }
            }));
            using (var db = new Repositorio.MudancaOtimizadaBancoDados())
            {
                switch (TipoDado)
                {
                    case TiposDadoBasico.Emitente:
                        return AnaliseCompletaXml<Emitente>(listaXML, nameof(Emitente), "emit", db.AnalisarAdicionarEmitentes);
                    case TiposDadoBasico.Cliente:
                        return AnaliseCompletaXml<Destinatario>(listaXML, nameof(Destinatario), "dest", db.AnalisarAdicionarClientes);
                    case TiposDadoBasico.Motorista:
                        return AnaliseCompletaXml<Motorista>(listaXML, nameof(Motorista), "transporta", db.AnalisarAdicionarMotoristas);
                    case TiposDadoBasico.Produto:
                        return AnaliseCompletaXml<BaseProdutoOuServico>(listaXML, nameof(BaseProdutoOuServico), "prod", db.AnalisarAdicionarProdutos);
                    default:
                        return null;
                }
            }
        }

        private List<Exception> AnaliseCompletaXml<TipoBase>(XElement[] listaXML, string nomePrimario, string nomeSecundario, Action<List<TipoBase>> Adicionar) where TipoBase : class
        {
            var retorno = new List<Exception>();
            var add = new List<TipoBase>();
            for (int i = 0; i < listaXML.Length; i++)
            {
                var resultado = RemoverNamespace(Busca(listaXML[i], nomePrimario, nomeSecundario));
                if (resultado == null)
                {
                    retorno.Add(new XmlNaoReconhecido(arquivos[i].Name, listaXML[i].Name.LocalName, nomeSecundario, nameof(TipoBase)));
                    continue;
                }
                var xml = resultado;
                xml.Name = nomePrimario;
                add.Add(xml.FromXElement<TipoBase>());
            }
            Adicionar(add);
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
