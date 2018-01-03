using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.Validacao;
using NFeFacil.View;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    [View.DetalhePagina(Symbol.View, "Visualizar NFe")]
    public sealed partial class VisualizacaoNFe : Page
    {
        Popup Log = Popup.Current;
        NFeDI ItemBanco { get; set; }
        object ObjetoItemBanco { get; set; }
        Detalhes Visualizacao { get; set; }

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
                Visualizacao = nfe.Informacoes;
            }
            else
            {
                var processo = xml.FromXElement<Processo>();
                ObjetoItemBanco = processo;
                Visualizacao = processo.NFe.Informacoes;
            }
            AtualizarBotoesComando();
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
                var assina = new Certificacao.AssinaFacil()
                {
                    Nota = nfe
                };
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var result = await assina.Assinar<NFe>(x, nfe.Informacoes.Id, "infNFe");
                    if (result.Item1)
                    {
                        ItemBanco.Status = (int)StatusNFe.Assinada;
                        AtualizarDI();
                        AtualizarBotoesComando();
                    }
                    return result;
                }, assina.CertificadosDisponiveis, "Subject", Certificacao.AssinaFacil.Etapas);
                assina.ProgressChanged += async (x, y) => await progresso.Update(y);
                await progresso.ShowAsync();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private async void Transmitir(object sender, RoutedEventArgs e)
        {
            Progresso progresso = null;
            progresso = new Progresso(async () =>
            {
                var retTransmissao = await ConsultarRespostaInicial(true);
                await progresso.Update(1);
                if (retTransmissao.StatusResposta == 103)
                {
                    var tempoResposta = retTransmissao.DadosRecibo.TempoMedioResposta;
                    await Task.Delay(TimeSpan.FromSeconds(tempoResposta + 5));
                    await progresso.Update(2);

                    var homologacao = ((NFe)ObjetoItemBanco).AmbienteTestes;
                    var resultadoResposta = await ConsultarRespostaFinal(retTransmissao, homologacao);
                    await progresso.Update(3);

                    if (resultadoResposta.Protocolo.InfProt.cStat == 100)
                    {
                        ObjetoItemBanco = new Processo()
                        {
                            NFe = (NFe)ObjetoItemBanco,
                            ProtNFe = resultadoResposta.Protocolo
                        };
                        ItemBanco.Status = (int)StatusNFe.Emitida;
                        AtualizarDI();
                        AtualizarBotoesComando();
                        await progresso.Update(4);

                        return (true, resultadoResposta.DescricaoResposta);
                    }
                    else
                    {
                        return (false, resultadoResposta.DescricaoResposta);
                    }
                }
                else
                {
                    return (false, retTransmissao.DescricaoResposta);
                }
            }, "Processar e enviar requisição inicial",
            "Aguardar tempo médio de resposta",
            "Processar e enviar requisição final",
            "Processar e analisar resposta final");
            progresso.Start();
            await progresso.ShowAsync();
        }

        async Task<RetEnviNFe> ConsultarRespostaInicial(bool homologacao)
        {
            var nota = (NFe)ObjetoItemBanco;
            var uf = nota.Informacoes.emitente.Endereco.SiglaUF;
            var gerenciador = new GerenciadorGeral<EnviNFe, RetEnviNFe>(uf, Operacoes.Autorizar, nota.AmbienteTestes);
            var envio = new EnviNFe(nota);
            return await gerenciador.EnviarAsync(envio, true);
        }

        async Task<RetConsReciNFe> ConsultarRespostaFinal(RetEnviNFe retTransmissao, bool homologacao)
        {
            var gerenciador = new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(
                retTransmissao.Estado, Operacoes.RespostaAutorizar, homologacao);
            var envio = new ConsReciNFe(retTransmissao.TipoAmbiente, retTransmissao.DadosRecibo.NumeroRecibo);
            return await gerenciador.EnviarAsync(envio);
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
                    ItemBanco.XML = ItemBanco.Status < (int)StatusNFe.Emitida
                        ? ObjetoItemBanco.ToXElement<NFe>().ToString()
                        : ObjetoItemBanco.ToXElement<Processo>().ToString();
                    repo.SalvarItemSimples(ItemBanco, DefinicoesTemporarias.DateTimeNow);
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
