using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NFeFacil
{
    internal sealed class ImportarDadoBase : Importacao
    {
        private TiposDadoBásico TipoDado;

        public ImportarDadoBase(TiposDadoBásico tipoDado) : base(".xml")
        {
            TipoDado = tipoDado;
        }

        public async Task Importar()
        {
            var arquivos = await ImportarArquivos();
            var XMLs = new List<XElement>();
            foreach (var item in arquivos)
            {
                XMLs.Add(XElement.Load(await item.OpenStreamForReadAsync()));
            }
            await ImportarDadosAsync(XMLs, TipoDado);
        }

        public async Task ImportarDadosAsync(List<XElement> listaXML, TiposDadoBásico tipo)
        {
            using (var db = new AplicativoContext())
            {
                switch (tipo)
                {
                    case TiposDadoBásico.Emitente:
                        {
                            var xmlVálidosEmit = new List<XElement>();
                            foreach (var item in listaXML)
                            {
                                var xml = RemoverNamespace(item);
                                xml = Busca(xml, @"emit", nameof(Emitente));
                                xml.Name = nameof(Emitente);
                                xmlVálidosEmit.Add(xml);
                            }
                            db.AddRange(from x in xmlVálidosEmit
                                        select FromXElement<Emitente>(x));
                            break;
                        }
                    case TiposDadoBásico.Cliente:
                        {
                            var xmlVálidosDest = new List<XElement>();
                            foreach (var item in listaXML)
                            {
                                var xml = RemoverNamespace(item);
                                xml = Busca(xml, @"dest", nameof(Destinatario));
                                xml.Name = nameof(Destinatario);
                                xmlVálidosDest.Add(xml);
                            }
                            db.AddRange(from x in xmlVálidosDest
                                        select FromXElement<Destinatario>(x));
                            break;
                        }
                    case TiposDadoBásico.Motorista:
                        {
                            var xmlVálidosMot = new List<XElement>();
                            foreach (var item in listaXML)
                            {
                                var xml = RemoverNamespace(item);
                                xml = Busca(xml, @"transporta", nameof(Motorista));
                                xml.Name = nameof(Motorista);
                                xmlVálidosMot.Add(xml);
                            }
                            db.AddRange(from x in xmlVálidosMot
                                        select FromXElement<Motorista>(x));
                            break;
                        }
                    case TiposDadoBásico.Produto:
                        {
                            var xmlVálidosProd = new List<XElement>();
                            foreach (var item in listaXML)
                            {
                                var xml = RemoverNamespace(item);
                                xml = Busca(xml, "prod", nameof(DadosBaseProdutoOuServico));
                                xml.Name = nameof(DadosBaseProdutoOuServico);
                                xmlVálidosProd.Add(xml);
                            }
                            db.AddRange(from x in xmlVálidosProd
                                        select FromXElement<DadosBaseProdutoOuServico>(x));
                            break;
                        }
                }
                await db.SaveChangesAsync();
            }
        }

        private XElement RemoverNamespace(XElement xmlBruto)
        {
            return new XElement(
                xmlBruto.Name.LocalName,
                xmlBruto.HasElements ?
                    xmlBruto.Elements().Select(el => RemoverNamespace(el)) :
                    (object)xmlBruto.Value);
        }

        private XElement Busca(XElement universo, params string[] nome)
        {
            Func<XElement, bool> analise = x => nome.Contains(x.Name.LocalName);
            if (analise(universo)) return universo;
            var quantidade = universo.Elements().Count(analise);
            if (quantidade == 1)
                return universo.Elements().Single(analise);
            else if (quantidade > 1)
                throw new ArgumentException($"Existem {quantidade} nós diferentes com o nome pedido");
            else
                foreach (var item in universo.Elements()) return Busca(item, nome);

            throw new ArgumentException($"Não foi encontrado nó com o nome {nome}", nameof(universo));
        }

        private static T FromXElement<T>(XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
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
