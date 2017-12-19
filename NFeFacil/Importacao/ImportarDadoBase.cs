using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using NFeFacil.Repositorio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using static NFeFacil.ItensBD.ProdutoDI;

namespace NFeFacil.Importacao
{
    public sealed class ImportarDadoBase : Importacao
    {
        TiposDadoBasico TipoDado;
        IReadOnlyList<StorageFile> arquivos;

        public ImportarDadoBase(TiposDadoBasico tipoDado) : base(".xml")
        {
            TipoDado = tipoDado;
        }

        public async Task<List<Exception>> ImportarAsync()
        {
            arquivos = await ImportarArquivos();
            var listaXML = await Task.WhenAll(arquivos.Select(async x =>
            {
                using (var stream = await x.OpenStreamForReadAsync())
                {
                    return XElement.Load(stream);
                }
            }));
            using (var repo = new Escrita())
            {
                switch (TipoDado)
                {
                    case TiposDadoBasico.Cliente:
                        var clientes = AnaliseCompletaXml<Destinatario>(listaXML, "dest");
                        repo.AnalisarAdicionarClientes(clientes.Item2.Select(dest => new ClienteDI(dest)), Propriedades.DateTimeNow);
                        return clientes.Item1;
                    case TiposDadoBasico.Motorista:
                        var motoristas = AnaliseCompletaXml<Motorista>(listaXML, "transporta");
                        repo.AnalisarAdicionarMotoristas(motoristas.Item2.Select(mot => new MotoristaDI(mot)), Propriedades.DateTimeNow);
                        return motoristas.Item1;
                    case TiposDadoBasico.Produto:
                        var produtos = AnaliseCompletaXml<ProdutoOuServicoGenerico>(listaXML, "prod");
                        repo.AnalisarAdicionarProdutos(produtos.Item2.Select(prod => new ProdutoDI(prod)), Propriedades.DateTimeNow);
                        return produtos.Item1;
                    default:
                        return null;
                }
            }
        }

        private (List<Exception>, List<TipoBase>) AnaliseCompletaXml<TipoBase>(XElement[] listaXML, string nomeSecundario) where TipoBase : class
        {
            string nomeDesejado = typeof(TipoBase).Name;
            var retorno = new List<Exception>();
            var add = new List<TipoBase>();
            for (int i = 0; i < listaXML.Length; i++)
            {
                try
                {
                    var resultado = Busca(listaXML[i], nomeSecundario);
                    if (resultado == null)
                    {
                        retorno.Add(new XmlNaoReconhecido(arquivos[i].Name, listaXML[i].Name.LocalName, nomeSecundario, nameof(TipoBase)));
                        continue;
                    }
                    var xml = RemoverNamespace(resultado);
                    xml.Name = nomeDesejado;
                    add.Add(xml.FromXElement<TipoBase>());
                }
                catch (Exception e)
                {
                    retorno.Add(e);
                }
            }
            return (retorno, add);
        }

        XElement Busca(XElement universo, string nome)
        {
            if (nome == universo.Name.LocalName)
            {
                return universo;
            }
            else
            {
                foreach (var item in universo.Elements())
                {
                    if (nome == item.Name.LocalName)
                    {
                        return item;
                    }
                    else if (item.HasElements)
                    {
                        var resultadoProfundo = Busca(item, nome);
                        if (resultadoProfundo != null)
                        {
                            return resultadoProfundo;
                        }
                    }
                }
            }
            return null;
        }

        XElement RemoverNamespace(XElement xmlBruto)
        {
            return new XElement(
                xmlBruto.Name.LocalName,
                xmlBruto.HasElements ?
                    xmlBruto.Elements().Select(el => RemoverNamespace(el)) :
                    (object)xmlBruto.Value);
        }

    }
}
