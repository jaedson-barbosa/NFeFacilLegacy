using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NFeFacil.Controles;
using System.Xml.Serialization;
using NFeFacil.WebService.Pacotes;
using NFeFacil.WebService;
using NFeFacil.Validacao;
using NFeFacil.View;
using NFeFacil.WebService.Pacotes.PartesEnvEvento;
using NFeFacil.WebService.Pacotes.PartesRetEnvEvento;
using NFeFacil.Certificacao;
using NFeFacil.Fiscal.ViewNFe;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Fiscal
{
    [DetalhePagina(Symbol.Library, "Notas salvas")]
    public sealed partial class NotasSalvas : Page, IHambuguer
    {
        bool isNFCe;

        public NotasSalvas()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            isNFCe = e.Parameter != null ? (bool)e.Parameter : false;
            using (var repo = new Repositorio.Leitura())
            {
                var (emitidas, outras, canceladas) = repo.ObterNotas(DefinicoesTemporarias.EmitenteAtivo.CNPJ, isNFCe);
                NotasEmitidas = emitidas.GerarObs();
                OutrasNotas = outras.GerarObs();
                NotasCanceladas = canceladas.GerarObs();
            }
        }

        ObservableCollection<NFeDI> NotasEmitidas { get; set; }
        ObservableCollection<NFeDI> OutrasNotas { get; set; }
        ObservableCollection<NFeDI> NotasCanceladas { get; set; }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Send, "Emitidas"),
            new ItemHambuguer(Symbol.SaveLocal, "Outras"),
            new ItemHambuguer(Symbol.Cancel, "Canceladas")
        };

        void Exibir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            AcoesVisualizacao acoes;
            if (isNFCe) acoes = new AcoesNFCe(nota);
            else acoes = new AcoesNFe(nota);
            MainPage.Current.Navegar<Visualizacao>(acoes);
        }

        void Excluir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            using (var repo = new Repositorio.OperacoesExtras())
            {
                repo.ExcluirNFe(nota);
                OutrasNotas.Remove(nota);
            }
        }

        async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            InformacoesBase informacoes;
            ProtocoloNFe protNFe;
            if (nota.IsNFCe)
            {
                var processo = XElement.Parse(nota.XML).FromXElement<ProcessoNFCe>();
                informacoes = processo.NFe.Informacoes;
                protNFe = processo.ProtNFe;
            }
            else
            {
                var processo = XElement.Parse(nota.XML).FromXElement<ProcessoNFe>();
                informacoes = processo.NFe.Informacoes;
                protNFe = processo.ProtNFe;
            }

            var estado = informacoes.identificacao.CódigoUF;
            var cnpj = informacoes.Emitente.CNPJ;
            var chave = informacoes.ChaveAcesso;
            var tipoAmbiente = protNFe.InfProt.tpAmb;
            var nProtocolo = protNFe.InfProt.nProt;

            var entrada = new CancelarNFe();
            if (await entrada.ShowAsync() == ContentDialogResult.Primary)
            {
                var infoEvento = new InformacoesEvento(estado, cnpj, chave, nProtocolo, entrada.Motivo, tipoAmbiente);
                var envio = new EnvEvento(infoEvento);

                AssinaFacil assinador = new AssinaFacil();
                await assinador.Preparar();

                var gerenciador = new GerenciadorGeral<EnvEvento, RetEnvEvento>(estado, Operacoes.RecepcaoEvento, tipoAmbiente == 2, isNFCe);
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var resultado = await envio.PrepararEventos(assinador, x);
                    if (!resultado.Item1)
                    {
                        return resultado;
                    }
                    await progresso.Update(1);

                    var resposta = await gerenciador.EnviarAsync(envio);
                    if (resposta.ResultadorEventos[0].InfEvento.CStat == 135)
                    {
                        using (var repo = new Repositorio.Escrita())
                        {
                            repo.SalvarItemSimples(new RegistroCancelamento()
                            {
                                ChaveNFe = chave,
                                DataHoraEvento = resposta.ResultadorEventos[0].InfEvento.DhRegEvento,
                                TipoAmbiente = tipoAmbiente,
                                XML = new ProcEventoCancelamento()
                                {
                                    Eventos = envio.Eventos,
                                    RetEvento = resposta.ResultadorEventos,
                                    Versao = resposta.Versao
                                }.ToXElement<ProcEventoCancelamento>().ToString()
                            }, DefinicoesTemporarias.DateTimeNow);

                            nota.Status = (int)StatusNota.Cancelada;
                            repo.SalvarItemSimples(nota, DefinicoesTemporarias.DateTimeNow);
                            await progresso.Update(6);

                            using (var opEx = new Repositorio.OperacoesExtras())
                            {
                                var rvVinculado = opEx.GetRVVinculado(informacoes.Id);
                                if (rvVinculado != null)
                                {
                                    if (rvVinculado.Cancelado)
                                    {
                                        var dialog = new MessageDialog("Um registro de venda já cancelado está vinculado a esta nota fiscal, você ainda deseja aplicar as alterações no estoque?", "Aviso");
                                        dialog.Commands.Add(new UICommand("Sim", y => AtualizarEstoques()));
                                        dialog.Commands.Add(new UICommand("Não"));
                                        await dialog.ShowAsync();
                                    }
                                    else
                                    {
                                        var dialog = new MessageDialog("Um registro de venda válido está vinculado a esta nota fiscal, você ainda deseja aplicar as alterações no estoque?", "Aviso");
                                        dialog.Commands.Add(new UICommand("Sim", y => AtualizarEstoques()));
                                        dialog.Commands.Add(new UICommand("Não"));
                                        await dialog.ShowAsync();

                                        dialog = new MessageDialog("Esta nota foi cancelada com sucesso, você deseja também cancelar o registro de venda?", "Aviso");
                                        dialog.Commands.Add(new UICommand("Sim", y =>
                                        {
                                            using (var escr = new Repositorio.Escrita())
                                            {
                                                escr.CancelarRV(rvVinculado, new CancelamentoRegistroVenda()
                                                {
                                                    Id = rvVinculado.Id,
                                                    MomentoCancelamento = DefinicoesTemporarias.DateTimeNow,
                                                    Motivo = "Cancelamento decorrente de cancelamento da nota fiscal correspondente."
                                                }, DefinicoesTemporarias.DateTimeNow);
                                            }
                                        }));
                                        dialog.Commands.Add(new UICommand("Não"));
                                        await dialog.ShowAsync();
                                    }
                                }
                                else
                                {
                                    AtualizarEstoques();
                                }
                            }

                            void AtualizarEstoques()
                            {
                                if (informacoes is InformacoesNFe nfe)
                                {
                                    var tpOp = nfe.identificacao.TipoOperacao;
                                    if (tpOp == 1 && DefinicoesPermanentes.ConfiguracoesEstoque.NFeSCancel)
                                    {
                                        using (var leit = new Repositorio.Leitura())
                                        {
                                            repo.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                                (from prod in nfe.produtos
                                                 let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                                 where orig != null
                                                 select (orig.Id, prod.Produto.QuantidadeComercializada)).ToArray());
                                        }
                                    }
                                    else if (tpOp == 0 && DefinicoesPermanentes.ConfiguracoesEstoque.NFeECancel)
                                    {
                                        using (var leit = new Repositorio.Leitura())
                                        {
                                            repo.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                                (from prod in nfe.produtos
                                                 let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                                 where orig != null
                                                 select (orig.Id, prod.Produto.QuantidadeComercializada * -1)).ToArray());
                                        }
                                    }
                                }
                                else if (informacoes is InformacoesNFCe nfce
                                    && DefinicoesPermanentes.ConfiguracoesEstoque.NFCeCancel)
                                {
                                    using (var leit = new Repositorio.Leitura())
                                    {
                                        repo.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                            (from prod in nfce.produtos
                                             let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                             where orig != null
                                             select (orig.Id, prod.Produto.QuantidadeComercializada)).ToArray());
                                    }
                                }
                            }

                            NotasEmitidas.Remove(nota);
                            NotasCanceladas.Insert(0, nota);
                        }


                        return (true, "NFe cancelada com sucesso.");
                    }
                    else
                    {
                        return (false, resposta.ResultadorEventos[0].InfEvento.XMotivo);
                    }
                }, assinador.CertificadosDisponiveis, "Subject",
                "Preparar eventos com assinatura do emitente",
                "Preparar conexão",
                "Obter conteúdo da requisição",
                "Enviar requisição",
                "Processar resposta",
                "Salvar registro de cancelamento no banco de dados");
                gerenciador.ProgressChanged += async (x, y) => await progresso.Update(y + 1);
                await progresso.ShowAsync();
            }
        }

        public int SelectedIndex { set => main.SelectedIndex = value; }

        [XmlRoot("procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public struct ProcEventoCancelamento
        {
            [XmlAttribute("versao")]
            public string Versao { get; set; }

            [XmlElement("evento")]
            public Evento[] Eventos { get; set; }

            [XmlElement("retEvento")]
            public ResultadoEvento[] RetEvento { get; set; }
        }

        async void CriarCopia(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            if (nota.IsNFCe)
            {
                var processo = XElement.Parse(nota.XML).FromXElement<ProcessoNFCe>();
                var nfe = processo.NFe;
                var analisador = new AnalisadorNFCe(ref nfe);
                analisador.Desnormalizar();
                var controle = new ControleNFCe(nfe);
                await new Criador(controle).ShowAsync();
            }
            else
            {
                var processo = XElement.Parse(nota.XML).FromXElement<ProcessoNFe>();
                var nfe = processo.NFe;
                var analisador = new AnalisadorNFe(ref nfe);
                analisador.Desnormalizar();
                var controle = new ControleNFe(nfe);
                await new Criador(controle).ShowAsync();
            }
        }
    }
}
