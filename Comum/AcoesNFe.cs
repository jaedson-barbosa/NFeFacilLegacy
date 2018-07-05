using BaseGeral.ItensBD;
using BaseGeral.ModeloXML;
using BaseGeral.Validacao;
using NFeFacil.View;
using Fiscal.WebService;
using Fiscal.WebService.Pacotes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using BaseGeral;
using Fiscal;

namespace Comum
{
    public sealed class AcoesNFe : AcoesVisualizacao
    {
        public object ItemCompleto { get; private set; }

        public AcoesNFe(NFeDI nota) : base(nota)
        {
            var xml = XElement.Parse(nota.XML);
            if (ItemBanco.Status < (int)StatusNota.Emitida)
            {
                var nfe = xml.FromXElement<NFe>();
                ItemCompleto = nfe;
            }
            else
            {
                var processo = xml.FromXElement<ProcessoNFe>();
                ItemCompleto = processo;
            }
        }

        public AcoesNFe(NFeDI banco, object manipulacao) : base(banco)
        {
            ItemCompleto = manipulacao;
        }

        public override async Task Assinar()
        {
            var nfe = (NFe)ItemCompleto;
            try
            {
                var assina = new Fiscal.Certificacao.AssinaFacil()
                {
                    Nota = nfe
                };
                await assina.Preparar();
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var result = await assina.Assinar<NFe>(x, nfe.Informacoes.Id, "infNFe");
                    if (result.Item1)
                    {
                        ItemBanco.Status = (int)StatusNota.Assinada;
                        AtualizarDI(ItemCompleto);
                        OnStatusChanged(StatusNota.Assinada);
                    }
                    return result;
                }, assina.CertificadosDisponiveis, "Subject", Fiscal.Certificacao.AssinaFacil.Etapas);
                assina.ProgressChanged += async (x, y) => await progresso.Update(y);
                await progresso.ShowAsync();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        public override void Editar()
        {
            var nfe = (NFe)ItemCompleto;
            var analisador = new AnalisadorNFe(ref nfe);
            analisador.Desnormalizar();
            ItemBanco.Status = (int)StatusNota.Edição;
            var controle = new ControleViewProduto(nfe);
            BasicMainPage.Current.Navegar<Venda.ViewProdutoVenda.ListaProdutos>(controle);
        }

        public override async Task Exportar()
        {
            XElement xml;
            string id;
            if (ItemBanco.Status < (int)StatusNota.Emitida)
            {
                var nfe = (NFe)ItemCompleto;
                id = nfe.Informacoes.Id;
                xml = nfe.ToXElement();
            }
            else
            {
                var processo = (ProcessoNFe)ItemCompleto;
                id = processo.NFe.Informacoes.Id;
                xml = processo.ToXElement();
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
                    AtualizarDI(ItemCompleto);
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        public override void Imprimir()
        {
            var processo = (ProcessoNFe)ItemCompleto;
            BasicMainPage.Current.Navegar<ViewDANFE>(processo);
            ItemBanco.Impressa = true;
            AtualizarDI(ItemCompleto);
        }

        public override void Salvar()
        {
            ItemBanco.Status = (int)StatusNota.Salva;
            AtualizarDI(ItemCompleto);
            OnStatusChanged(StatusNota.Salva);
        }

        public override async Task Transmitir()
        {
            Progresso progresso = null;
            progresso = new Progresso(async () =>
            {
                var retTransmissao = await ConsultarRespostaInicial(true);
                await progresso.Update(1);
                var status = retTransmissao.StatusResposta;
                if (status == 103 || status == 104)
                {
                    if (status == 103)
                    {
                        var tempoResposta = retTransmissao.DadosRecibo.TempoMedioResposta;
                        await Task.Delay(TimeSpan.FromSeconds(tempoResposta + 5));
                    }
                    await progresso.Update(2);

                    bool homologacao;
                    try
                    {
                        homologacao = ((NFe)ItemCompleto).AmbienteTestes;
                    }
                    catch (Exception e)
                    {
                        throw new AnaliseErro("Erro durante a obtenção do ambiente de testes.", e);
                    }
                    var resultadoResposta = await ConsultarRespostaFinal(retTransmissao, homologacao);
                    await progresso.Update(3);

                    var protocoloResposta = resultadoResposta.Protocolo?.InfProt;
                    if (protocoloResposta?.cStat == 100)
                    {
                        var nfe = (NFe)ItemCompleto;
                        ItemCompleto = new ProcessoNFe()
                        {
                            NFe = nfe,
                            ProtNFe = resultadoResposta.Protocolo
                        };
                        ItemBanco.Status = (int)StatusNota.Emitida;
                        AtualizarDI(ItemCompleto);
                        OnStatusChanged(StatusNota.Emitida);
                        await progresso.Update(4);

                        using (var opEx = new BaseGeral.Repositorio.OperacoesExtras())
                        {
                            var rvVinculado = opEx.GetRVVinculado(nfe.Informacoes.Id);
                            if (rvVinculado != null)
                            {
                                if (rvVinculado.Cancelado)
                                {
                                    var dialog = new MessageDialog("Um registro de venda já cancelado está vinculado a esta nota fiscal, você ainda deseja aplicar as alterações no estoque?", "Aviso");
                                    dialog.Commands.Add(new UICommand("Sim", x => AtualizarEstoques()));
                                    dialog.Commands.Add(new UICommand("Não"));
                                    await dialog.ShowAsync();
                                }
                                else
                                {
                                    var dialog = new MessageDialog("Um registro de venda válido está vinculado a esta nota fiscal, você ainda deseja aplicar as alterações no estoque?", "Aviso");
                                    dialog.Commands.Add(new UICommand("Sim", x => AtualizarEstoques()));
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
                            var tpOp = nfe.Informacoes.identificacao.TipoOperacao;
                            if (tpOp == 1 && DefinicoesPermanentes.ConfiguracoesEstoque.NFeS)
                            {
                                using (var leit = new BaseGeral.Repositorio.Leitura())
                                using (var escr = new BaseGeral.Repositorio.Escrita())
                                {
                                    escr.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                        (from prod in nfe.Informacoes.produtos
                                         let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                         where orig != null
                                         select (orig.Id, prod.Produto.QuantidadeComercializada * -1)).ToArray());
                                }
                            }
                            else if (tpOp == 0 && DefinicoesPermanentes.ConfiguracoesEstoque.NFeE)
                            {
                                using (var leit = new BaseGeral.Repositorio.Leitura())
                                using (var escr = new BaseGeral.Repositorio.Escrita())
                                {
                                    escr.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                        (from prod in nfe.Informacoes.produtos
                                         let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                         where orig != null
                                         select (orig.Id, prod.Produto.QuantidadeComercializada)).ToArray());
                                }
                            }
                        }

                        return (true, protocoloResposta.xMotivo);
                    }
                    else if (protocoloResposta != null)
                    {
                        return (false, $"{protocoloResposta.cStat}: {protocoloResposta.xMotivo}");
                    }
                    else
                    {
                        return (false, $"{resultadoResposta.StatusResposta}: {resultadoResposta.DescricaoResposta}");
                    }
                }
                else
                {
                    return (false, $"{retTransmissao.StatusResposta}: {retTransmissao.DescricaoResposta}");
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
            var nota = (NFe)ItemCompleto;
            var uf = nota.Informacoes.Emitente.Endereco.SiglaUF;
            var gerenciador = new GerenciadorGeral<EnviNFe, RetEnviNFe>(uf, Operacoes.Autorizar, nota.AmbienteTestes, false);
            var envio = new EnviNFe(nota);
            return await gerenciador.EnviarAsync(envio, true);
        }

        async Task<RetConsReciNFe> ConsultarRespostaFinal(RetEnviNFe retTransmissao, bool homologacao)
        {
            if (retTransmissao == null)
            {
                throw new AnaliseErro("O resultado da transmissão inicial não é válido.");
            }
            GerenciadorGeral<ConsReciNFe, RetConsReciNFe> gerenciador;
            try
            {
                gerenciador = new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(
                    retTransmissao.Estado, Operacoes.RespostaAutorizar, homologacao, false);
            }
            catch (Exception e)
            {
                throw new AnaliseErro("Erro durante a criação do gerenciador.", e);
            }

            ConsReciNFe envio;
            try
            {
                envio = new ConsReciNFe(retTransmissao.TipoAmbiente, retTransmissao.DadosRecibo.NumeroRecibo);
            }
            catch (Exception e)
            {
                throw new AnaliseErro("Erro durante a criação do envio.", e);
            }

            try
            {
                return await gerenciador.EnviarAsync(envio);
            }
            catch (Exception e)
            {
                throw new AnaliseErro("Erro durante o envio.", e);
            }
        }

        sealed class AnaliseErro : Exception
        {
            public AnaliseErro(string message) : base(message) { }
            public AnaliseErro(string message, Exception inner) : base(message, inner) { }
        }

        public override InformacoesBase ObterVisualizacao()
        {
            if (ItemCompleto is NFe nfe) return nfe.Informacoes;
            else if (ItemCompleto is ProcessoNFe proc) return proc.NFe.Informacoes;
            throw new Exception();
        }
    }
}
