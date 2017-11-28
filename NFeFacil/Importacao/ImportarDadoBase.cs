using NFeFacil.ItensBD;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
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
            switch (TipoDado)
            {
                case TiposDadoBasico.Cliente:
                    var clientes = AnaliseCompletaXml<Destinatario>(listaXML, "dest");
                    AnalisarAdicionarClientes(clientes.Item2.Select(dest => new ClienteDI(dest)));
                    return clientes.Item1;
                case TiposDadoBasico.Motorista:
                    var motoristas = AnaliseCompletaXml<Motorista>(listaXML, "transporta");
                    AnalisarAdicionarMotoristas(motoristas.Item2.Select(mot => new MotoristaDI(mot)));
                    return motoristas.Item1;
                case TiposDadoBasico.Produto:
                    var produtos = AnaliseCompletaXml<ProdutoOuServicoGenerico>(listaXML, "prod");
                    AnalisarAdicionarProdutos(produtos.Item2.Select(prod => new ProdutoDI(prod)));
                    return produtos.Item1;
                default:
                    return null;
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

        internal void AnalisarAdicionarClientes(IEnumerable<ClienteDI> clientes)
        {
            using (var db = new AplicativoContext())
            {
                foreach (var dest in clientes)
                {
                    if (dest.Id != null && db.Clientes.Find(dest.Id) != null)
                    {
                        dest.UltimaData = Propriedades.DateTimeNow;
                        db.Update(dest);
                    }
                    else
                    {
                        var busca = db.Clientes.FirstOrDefault(x => x.Documento == dest.Documento);
                        dest.UltimaData = Propriedades.DateTimeNow;
                        if (busca != default(ClienteDI))
                        {
                            dest.Id = busca.Id;
                            db.Update(dest);
                        }
                        else
                        {
                            db.Add(dest);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        internal void AnalisarAdicionarMotoristas(IEnumerable<MotoristaDI> motoristas)
        {
            using(var db = new AplicativoContext())
            {
                foreach (var mot in motoristas)
                {
                    if (mot.Id != null && db.Motoristas.Find(mot.Id) != null)
                    {
                        mot.UltimaData = Propriedades.DateTimeNow;
                        db.Update(mot);
                    }
                    else
                    {
                        var busca = db.Motoristas.FirstOrDefault(x => x.Documento == mot.Documento
                            || (x.Nome == mot.Nome && x.XEnder == mot.XEnder));
                        mot.UltimaData = Propriedades.DateTimeNow;
                        if (busca != default(MotoristaDI))
                        {
                            mot.Id = busca.Id;
                            db.Update(mot);
                        }
                        else
                        {
                            db.Add(mot);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        internal void AnalisarAdicionarProdutos(IEnumerable<ProdutoDI> produtos)
        {
            using (var db = new AplicativoContext())
            {
                foreach (var prod in produtos)
                {
                    if (prod.Id != null && db.Produtos.Find(prod.Id) != null)
                    {
                        prod.UltimaData = Propriedades.DateTimeNow;
                        db.Update(prod);
                    }
                    else
                    {
                        var busca = db.Produtos.FirstOrDefault(x => x.Descricao == prod.Descricao
                            || (x.CodigoProduto == prod.CodigoProduto && x.CFOP == prod.CFOP));
                        prod.UltimaData = Propriedades.DateTimeNow;
                        if (busca != default(ProdutoDI))
                        {
                            prod.Id = busca.Id;
                            db.Update(prod);
                        }
                        else
                        {
                            db.Add(prod);
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
