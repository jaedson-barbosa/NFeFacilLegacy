using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
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
            var db = new AplicativoContext();
            try
            {
                var repo = new Repositorio.MudancaOtimizadaBancoDados(db);
                switch (TipoDado)
                {
                    case TiposDadoBasico.Emitente:
                        return AnaliseCompletaXml<Emitente>(listaXML, nameof(Emitente), "emit", x => repo.AnalisarAdicionarEmitentes(x.Select(emit => new ItensBD.EmitenteDI(emit)).ToList()));
                    case TiposDadoBasico.Cliente:
                        return AnaliseCompletaXml<Destinatario>(listaXML, nameof(Destinatario), "dest", x=> repo.AnalisarAdicionarClientes(x.Select(dest => new ItensBD.ClienteDI(dest)).ToList()));
                    case TiposDadoBasico.Motorista:
                        return AnaliseCompletaXml<Motorista>(listaXML, nameof(Motorista), "transporta", x => repo.AnalisarAdicionarMotoristas(x.Select(mot => new ItensBD.MotoristaDI(mot)).ToList()));
                    case TiposDadoBasico.Produto:
                        return AnaliseCompletaXml<ProdutoOuServicoGenerico>(listaXML, nameof(ProdutoOuServicoGenerico), "prod", x=> repo.AnalisarAdicionarProdutos(x.Select(prod => new ItensBD.ProdutoDI(prod)).ToList()));
                    default:
                        return null;
                }
            }
            finally
            {
                db.SaveChanges();
                db.Dispose();
            }
        }

        public sealed class ProdutoOuServicoGenerico
        {
            /// <summary>
            /// Preencher com CFOP, caso se trate de itens não relacionados com mercadorias/produtos e que o contribuinte não possua codificação própria. Formato: ”CFOP9999”.
            /// </summary>
            [XmlElement(ElementName = "cProd")]
            public string CodigoProduto { get; set; }

            /// <summary>
            /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
            /// </summary>
            [XmlElement(ElementName = "cEAN")]
            public string CodigoBarras { get; set; } = "";

            [XmlElement(ElementName = "xProd")]
            public string Descricao { get; set; }

            /// <summary>
            /// Obrigatória informação do NCM completo (8 dígitos).
            /// Em caso de item de serviço ou item que não tenham Produto (ex. transferência de crédito), informar o valor 00 (dois zeros).
            /// </summary>
            public string NCM { get; set; }

            /// <summary>
            /// (Opcional)
            /// Preencher de acordo com o código EX da TIPI. Em caso de serviço, não incluir a TAG.
            /// </summary>
            public string EXTIPI { get; set; }

            /// <summary>
            /// Código Fiscal de Operações e Prestações.
            /// </summary>
            public string CFOP { get; set; }

            /// <summary>
            /// Informar a unidade de comercialização do Produto.
            /// </summary>
            [XmlElement(ElementName = "uCom")]
            public string UnidadeComercializacao { get; set; }

            /// <summary>
            /// Informar o valor unitário de comercialização do Produto.
            /// </summary>
            [XmlElement(ElementName = "vUnCom")]
            public double ValorUnitario { get; set; }

            /// <summary>
            /// GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras.
            /// Não informar o conteúdo da TAG em caso de o Produto não possuir este código.
            /// </summary>
            [XmlElement(ElementName = "cEANTrib")]
            public string CodigoBarrasTributo { get; set; } = "";

            /// <summary>
            /// Unidade Tributável.
            /// </summary>
            [XmlElement(ElementName = "uTrib")]
            public string UnidadeTributacao { get; set; }

            /// <summary>
            /// Informar o valor unitário de tributação do Produto.
            /// </summary>
            [XmlElement(ElementName = "vUnTrib")]
            public double ValorUnitarioTributo { get; set; }
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
