using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.Validacao;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VisualizacaoNFe : Page
    {
        Popup Log = Popup.Current;
        NFeDI ItemBanco { get; set; }
        object ObjetoItemBanco { get; set; }

        public VisualizacaoNFe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ItemBanco = (NFeDI)e.Parameter;
            var xml = XElement.Parse(ItemBanco.XML);
            if (ItemBanco.Status < (int)StatusNFe.Emitida)
            {
                var nfe = xml.FromXElement<NFe>();
                ObjetoItemBanco = nfe;
                ObterPropriedades(nfe.Informacoes, 0);
            }
            else
            {
                var processo = xml.FromXElement<Processo>();
                ObjetoItemBanco = processo;
                ObterPropriedades(processo.NFe.Informacoes, 0);
            }
            AtualizarBotoesComando();
        }

        void ObterPropriedades(object obj, int profundidade)
        {
            foreach (var prop in obj.GetType().GetProperties().Where(x => x.CanWrite
                && x.GetCustomAttribute<System.Xml.Serialization.XmlIgnoreAttribute>() == null))
            {
                var valor = prop.GetValue(obj);
                if (valor != null)
                {
                    var desc = prop.GetCustomAttribute<DescricaoPropriedade>();
                    if (valor.GetType().Namespace.Contains("NFeFacil"))
                    {
                        AdicionarCampo(desc?.Descricao ?? prop.Name, (EstilosTexto)profundidade);
                        ObterPropriedades(valor, profundidade + 1);
                    }
                    else if (valor is IEnumerable listaFilha && !(valor is string))
                    {
                        var tipoItem = listaFilha.GetType().GenericTypeArguments[0];
                        var itemPersonalizado = tipoItem.Namespace.Contains("NFeFacil");
                        foreach (var item in listaFilha)
                        {
                            if (itemPersonalizado)
                            {
                                AdicionarCampo(desc?.Descricao ?? tipoItem.Name, (EstilosTexto)profundidade);
                                ObterPropriedades(item, profundidade + 1);
                            }
                            else
                            {
                                AdicionarCampo(desc?.Descricao ?? tipoItem.Name,
                                    (EstilosTexto)profundidade, item.ToString());
                            }
                        }
                    }
                    else
                    {
                        var ext = prop.GetCustomAttribute<PropriedadeExtensivel>();
                        AdicionarCampo(ext?.NomeExtensão ?? desc?.Descricao ?? prop.Name,
                            EstilosTexto.BodyTextBlockStyle, (ext?.ObterValor(valor) ?? valor).ToString());
                    }
                }
            }
        }

        void AdicionarCampo(string texto, EstilosTexto estilo, string textoComplementar = null)
        {
            visualizacao.Inlines.Add(CriarRun(texto, textoComplementar != null));

            if (textoComplementar != null)
                visualizacao.Inlines.Add(CriarRun(textoComplementar));

            visualizacao.Inlines.Add(new LineBreak());

            Run CriarRun(string conteudo, bool label = false)
            {
                var retorno = new Run() { Text = label ? conteudo + ": " : conteudo };
                switch (estilo)
                {
                    case EstilosTexto.HeaderTextBlockStyle:
                        retorno.FontWeight = FontWeights.Light;
                        retorno.FontSize = 46;
                        return retorno;
                    case EstilosTexto.SubheaderTextBlockStyle:
                        retorno.FontWeight = FontWeights.Light;
                        retorno.FontSize = 34;
                        return retorno;
                    case EstilosTexto.TitleTextBlockStyle:
                        retorno.FontWeight = FontWeights.SemiLight;
                        retorno.FontSize = 24;
                        return retorno;
                    case EstilosTexto.SubtitleTextBlockStyle:
                        retorno.FontWeight = FontWeights.Normal;
                        retorno.FontSize = 20;
                        return retorno;
                    default:
                        retorno.FontWeight = label ? FontWeights.Bold : FontWeights.Normal;
                        return retorno;
                }
            }
        }

        enum EstilosTexto
        {
            HeaderTextBlockStyle,
            SubheaderTextBlockStyle,
            TitleTextBlockStyle,
            SubtitleTextBlockStyle,
            BodyTextBlockStyle
        }

        private void Editar(object sender, RoutedEventArgs e)
        {
            var nfe = (NFe)ObjetoItemBanco;
            var analisador = new AnalisadorNFe(ref nfe);
            analisador.Desnormalizar();
            ItemBanco.Status = (int)StatusNFe.Edição;
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(nfe);
        }

        private void Salvar(object sender, RoutedEventArgs e)
        {
            ItemBanco.Status = (int)StatusNFe.Salva;
            AtualizarDI();
            AtualizarBotoesComando();
            Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso.");
        }

        private async void Assinar(object sender, RoutedEventArgs e)
        {
            var nfe = (NFe)ObjetoItemBanco;
            try
            {
                var assina = new Certificacao.AssinaFacil(nfe);
                await assina.Assinar<NFe>(nfe.Informacoes.Id, "infNFe");

                ItemBanco.Status = (int)StatusNFe.Assinada;
                AtualizarDI();
                AtualizarBotoesComando();
                Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal assinada com sucesso.");
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private async void Transmitir(object sender, RoutedEventArgs e)
        {
            var nota = (NFe)ObjetoItemBanco;
            try
            {
                var resultadoTransmissao = await new GerenciadorGeral<EnviNFe, RetEnviNFe>(nota.Informacoes.emitente.Endereco.SiglaUF, Operacoes.Autorizar, nota.AmbienteTestes)
                    .EnviarAsync(new EnviNFe(nota.Informacoes.identificacao.Numero, nota), true);
                if (resultadoTransmissao.cStat == 103)
                {
                    await Task.Delay(new TimeSpan(0, 0, 10));
                    var resultadoResposta = await new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(resultadoTransmissao.cUF, Operacoes.RespostaAutorizar, nota.AmbienteTestes)
                        .EnviarAsync(new ConsReciNFe(resultadoTransmissao.tpAmb, resultadoTransmissao.infRec.nRec));
                    if (resultadoResposta.protNFe.InfProt.cStat == 100)
                    {
                        Log.Escrever(TitulosComuns.Sucesso, resultadoResposta.xMotivo);

                        ObjetoItemBanco = new Processo()
                        {
                            NFe = nota,
                            ProtNFe = resultadoResposta.protNFe
                        };
                        ItemBanco.Status = (int)StatusNFe.Emitida;
                        AtualizarDI();
                        AtualizarBotoesComando();
                    }
                    else
                    {
                        Log.Escrever(TitulosComuns.Erro, $"A nota fiscal foi processada, mas recusada. Mensagem de retorno:\r\n" +
                            $"{resultadoResposta.protNFe.InfProt.xMotivo}");
                    }
                }
                else
                {
                    Log.Escrever(TitulosComuns.Erro, $"A NFe não foi aceita. Mensagem de retorno:\r\n" +
                        $"{resultadoTransmissao.xMotivo}\r\n" +
                        $"Por favor, exporte esta nota fiscal e envie o XML gerado para o desenvolvedor do aplicativo para que o erro possa ser corrigido.");
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Imprimir(object sender, RoutedEventArgs e)
        {
            var processo = (Processo)ObjetoItemBanco;
            MainPage.Current.Navegar<DANFE.ViewDANFE>(processo);
            ItemBanco.Impressa = true;
            AtualizarDI();
            AtualizarBotoesComando();
        }

        private async void Exportar(object sender, RoutedEventArgs e)
        {
            XElement xml;
            string id;
            if (ItemBanco.Status < (int)StatusNFe.Emitida)
            {
                var nfe = (NFe)ObjetoItemBanco;
                id = nfe.Informacoes.Id;
                xml = ObjetoItemBanco.ToXElement<NFe>();
            }
            else
            {
                var processo = (Processo)ObjetoItemBanco;
                id = processo.NFe.Informacoes.Id;
                xml = ObjetoItemBanco.ToXElement<Processo>();
            }

            try
            {
                FileSavePicker salvador = new FileSavePicker
                {
                    DefaultFileExtension = ".xml",
                    SuggestedFileName = $"{id}.xml",
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
                salvador.FileTypeChoices.Add("Arquivo XML", new string[] { ".xml" });
                var arquivo = await salvador.PickSaveFileAsync();
                if (arquivo != null)
                {
                    using (var stream = await arquivo.OpenStreamForWriteAsync())
                    {
                        xml.Save(stream);
                        await stream.FlushAsync();
                    }

                    ItemBanco.Exportada = true;
                    AtualizarDI();
                    Log.Escrever(TitulosComuns.Sucesso, $"Nota fiscal exportada com sucesso.");
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void AtualizarDI()
        {
            try
            {
                using (var repo = new Repositorio.Escrita())
                {
                    if (ItemBanco.Status < (int)StatusNFe.Emitida)
                    {
                        ItemBanco.XML = ObjetoItemBanco.ToXElement<NFe>().ToString();
                    }
                    else
                    {
                        ItemBanco.XML = ObjetoItemBanco.ToXElement<Processo>().ToString();
                    }
                    repo.SalvarDadoBase(ItemBanco, Propriedades.DateTimeNow);
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        void AtualizarBotoesComando()
        {
            var status = (StatusNFe)ItemBanco.Status;
            btnEditar.IsEnabled = status == StatusNFe.Validada || status == StatusNFe.Salva || status == StatusNFe.Assinada;
            btnSalvar.IsEnabled = status == StatusNFe.Validada;
            btnAssinar.IsEnabled = status == StatusNFe.Salva;
            btnTransmitir.IsEnabled = status == StatusNFe.Assinada;
            btnImprimir.IsEnabled = status == StatusNFe.Emitida;
        }
    }
}
