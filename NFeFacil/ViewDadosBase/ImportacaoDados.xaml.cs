using BaseGeral.Log;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using BaseGeral.ItensBD;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BaseGeral.Repositorio;
using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesTransporte;
using BaseGeral.ModeloXML;
using System.Xml.Serialization;
using BaseGeral;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.Import, "Importação")]
    public sealed partial class ImportacaoDados : Page
    {
        public ImportacaoDados()
        {
            InitializeComponent();
        }

        async void ImportarCliente(object sender, TappedRoutedEventArgs e)
        {
            using (var repo = new Escrita())
            {
                var clientes = await Entrada<Destinatario>("dest");
                repo.AnalisarAdicionarClientes(clientes.Select(dest => new ClienteDI(dest)), DefinicoesTemporarias.DateTimeNow);
            }
        }

        async void ImportarMotorista(object sender, TappedRoutedEventArgs e)
        {
            using (var repo = new Escrita())
            {
                var motoristas = await Entrada<Motorista>("transporta");
                repo.AnalisarAdicionarMotoristas(motoristas.Select(mot => new MotoristaDI(mot)), DefinicoesTemporarias.DateTimeNow);
            }
        }

        async void ImportarProduto(object sender, TappedRoutedEventArgs e)
        {
            using (var repo = new Escrita())
            {
                var produtos = await Entrada<ProdutoDI.ProdutoOuServicoGenerico>("prod");
                repo.AnalisarAdicionarProdutos(produtos.Select(prod => new ProdutoDI(prod)), DefinicoesTemporarias.DateTimeNow);
            }
        }

        async void ImportarNotaFiscal(object sender, TappedRoutedEventArgs e)
        {
            var arquivos = await ImportarArquivos();
            List<NFeDI> conjuntos = new List<NFeDI>();
            for (int i = 0; i < arquivos.Count; i++)
            {
                try
                {
                    var xmlAtual = await ObterXMLNFe(arquivos[i]);
                    if (xmlAtual != null)
                    {
                        NFeDI diAtual;
                        if (xmlAtual.Name.LocalName == nameof(NFe))
                        {
                            diAtual = new NFeDI(FromXElement<NFe>(xmlAtual), xmlAtual.ToString());
                        }
                        else
                        {
                            diAtual = new NFeDI(FromXElement<ProcessoNFe>(xmlAtual), xmlAtual.ToString());
                        }
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
                }
                catch (Exception) { }
            }
            using (var repo = new Escrita())
            {
                repo.AdicionarNotasFiscais(conjuntos, DefinicoesTemporarias.DateTimeNow);
            }
            Popup.Current.Escrever(TitulosComuns.Atenção, "Caso algum dado não tenha sido importado é porque ele não tem o formado aceito pelo aplicativo.");
        }

        async void ImportarNFCe(object sender, TappedRoutedEventArgs e)
        {
            var arquivos = await ImportarArquivos();
            List<NFeDI> conjuntos = new List<NFeDI>();
            for (int i = 0; i < arquivos.Count; i++)
            {
                try
                {
                    var xmlAtual = await ObterXMLNFe(arquivos[i]);
                    if (xmlAtual != null)
                    {
                        NFeDI diAtual;
                        if (xmlAtual.Name.LocalName == nameof(NFe))
                        {
                            diAtual = new NFeDI(FromXElement<NFCe>(xmlAtual), xmlAtual.ToString());
                        }
                        else
                        {
                            diAtual = new NFeDI(FromXElement<ProcessoNFCe>(xmlAtual), xmlAtual.ToString());
                        }

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
                }
                catch (Exception) { }
            }
            using (var repo = new Escrita())
            {
                repo.AdicionarNotasFiscais(conjuntos, DefinicoesTemporarias.DateTimeNow);
            }
            Popup.Current.Escrever(TitulosComuns.Atenção, "Caso algum dado não tenha sido importado é porque ele não tem o formado aceito pelo aplicativo.");
        }

        static T FromXElement<T>(XNode xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = xElement.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static async Task<XElement> ObterXMLNFe(StorageFile arquivo)
        {
            using (var stream = await arquivo.OpenStreamForReadAsync())
            {
                var xmlAtual = XElement.Load(stream);
                if (xmlAtual.Name.LocalName != "nfeProc" && xmlAtual.Name.LocalName != "NFe")
                {
                    return null;
                }

                var filhoIdent = xmlAtual.Name.LocalName == "NFe"
                ? xmlAtual : xmlAtual.Element(XName.Get("NFe", "http://www.portalfiscal.inf.br/nfe"));
                filhoIdent = filhoIdent.Element(XName.Get("infNFe", "http://www.portalfiscal.inf.br/nfe"));

                return filhoIdent.Attribute("versao").Value == "3.10" ? xmlAtual : null;
            }
        }

        async Task<List<TipoBase>> Entrada<TipoBase>(string nomeSecundario) where TipoBase : class
        {
            var arquivos = await ImportarArquivos();
            var listaXML = await Task.WhenAll(arquivos.Select(async x =>
            {
                using (var stream = await x.OpenStreamForReadAsync())
                {
                    return XElement.Load(stream);
                }
            }));

            string nomeDesejado = typeof(TipoBase).Name;
            var add = new List<TipoBase>();
            for (int i = 0; i < listaXML.Length; i++)
            {
                try
                {
                    var resultado = Busca(listaXML[i], nomeSecundario);
                    if (resultado != null)
                    {
                        var xml = RemoverNamespace(resultado);
                        xml.Name = nomeDesejado;
                        add.Add(xml.FromXElement<TipoBase>());
                    }
                }
                catch (Exception) { }
            }
            return add;

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

        async Task<IReadOnlyList<StorageFile>> ImportarArquivos()
        {
            var importar = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            importar.FileTypeFilter.Add(".xml");
            return await importar.PickMultipleFilesAsync();
        }
    }
}
