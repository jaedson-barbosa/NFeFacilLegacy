using NFeFacil.Fiscal.ViewNFCe;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.Validacao;
using NFeFacil.View;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace NFeFacil.Fiscal
{
    sealed class AcoesNFCe : AcoesVisualizacao
    {
        public object ItemCompleto { get; private set; }

        public AcoesNFCe(NFeDI nota) : base(nota)
        {
            var xml = XElement.Parse(nota.XML);
            if (ItemBanco.Status < (int)StatusNota.Emitida)
            {
                var nfe = xml.FromXElement<NFCe>();
                ItemCompleto = nfe;
            }
            else
            {
                var processo = xml.FromXElement<ProcessoNFCe>();
                ItemCompleto = processo;
            }
        }

        public AcoesNFCe(NFeDI banco, object manipulacao) : base(banco)
        {
            ItemCompleto = manipulacao;
        }

        public override async Task Assinar()
        {
            var nfe = (NFCe)ItemCompleto;
            try
            {
                var assina = new Certificacao.AssinaFacil()
                {
                    Nota = nfe
                };
                await assina.Preparar();
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var result = await assina.Assinar<NFCe>(x, nfe.Informacoes.Id, "infNFe");
                    if (result.Item1)
                    {
                        ItemBanco.Status = (int)StatusNota.Assinada;
                        AtualizarDI(ItemCompleto);
                        OnStatusChanged(StatusNota.Assinada);
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

        public override void Editar()
        {
            var nfe = (NFCe)ItemCompleto;
            var analisador = new AnalisadorNFCe(ref nfe);
            analisador.Desnormalizar();
            ItemBanco.Status = (int)StatusNota.Edição;
            MainPage.Current.Navegar<ManipulacaoNFCe>(nfe);
        }

        public override async Task Exportar()
        {
            XElement xml;
            string id;
            if (ItemBanco.Status < (int)StatusNota.Emitida)
            {
                var nfe = (NFCe)ItemCompleto;
                id = nfe.Informacoes.Id;
                xml = nfe.ToXElement();
            }
            else
            {
                var processo = (ProcessoNFCe)ItemCompleto;
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
            var processo = (ProcessoNFCe)ItemCompleto;
            var margem = ExtensoesPrincipal.CMToPixel(DefinicoesPermanentes.MargemDANFENFCe / 10);
            var largura = ExtensoesPrincipal.CMToPixel(DefinicoesPermanentes.LarguraDANFENFCe / 10);
            var dados = new DadosImpressao(processo, new Thickness(margem), largura);
            MainPage.Current.Navegar<ViewDANFE>(dados);
            ItemBanco.Impressa = true;
            AtualizarDI(ItemCompleto);
        }

        public override void Salvar()
        {
            ItemBanco.Status = (int)StatusNota.Salva;
            AtualizarDI(ItemCompleto);
            OnStatusChanged(StatusNota.Salva);
        }

        bool AdicionarInfoSuplementares()
        {
            try
            {
                var notaSalva = (NFCe)ItemCompleto;
                var emit = DefinicoesTemporarias.EmitenteAtivo;
                if (string.IsNullOrEmpty(emit.IdToken) || string.IsNullOrEmpty(emit.CSC))
                {
                    throw new Exception("O CSC e seu identificador são informações obrigatórias, por favor, cadastre elas no cadastro deste emitente.");
                }
                notaSalva.PrepararInformacoesSuplementares(emit.IdToken, emit.CSC);
                return true;
            }
            catch (Exception e)
            {
                e.ManipularErro();
                return false;
            }
        }

        public override async Task Transmitir()
        {
            if (!AdicionarInfoSuplementares()) return;

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

                    var homologacao = ((NFCe)ItemCompleto).AmbienteTestes;
                    var resultadoResposta = await ConsultarRespostaFinal(retTransmissao, homologacao);
                    await progresso.Update(3);

                    var protocoloResposta = resultadoResposta.Protocolo.InfProt;
                    if (protocoloResposta?.cStat == 100)
                    {
                        var nfce = (NFCe)ItemCompleto;
                        ItemCompleto = new ProcessoNFCe()
                        {
                            NFe = nfce,
                            ProtNFe = resultadoResposta.Protocolo
                        };
                        ItemBanco.Status = (int)StatusNota.Emitida;
                        AtualizarDI(ItemCompleto);
                        OnStatusChanged(StatusNota.Emitida);
                        await progresso.Update(4);

                        if (DefinicoesPermanentes.ConfiguracoesEstoque.NFCe)
                        {
                            using (var leit = new Repositorio.Leitura())
                            using (var escr = new Repositorio.Escrita())
                            {
                                escr.AtualizarEstoques(DefinicoesTemporarias.DateTimeNow,
                                    (from prod in nfce.Informacoes.produtos
                                     let orig = leit.ObterProduto(prod.Produto.CodigoProduto)
                                     where orig != null
                                     select (orig.Id, prod.Produto.QuantidadeComercializada * -1)).ToArray());
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
            var nota = (NFCe)ItemCompleto;
            var uf = nota.Informacoes.Emitente.Endereco.SiglaUF;
            var gerenciador = new GerenciadorGeral<EnviNFCe, RetEnviNFe>(uf, Operacoes.Autorizar, nota.AmbienteTestes, true);
            var envio = new EnviNFCe(nota);
            return await gerenciador.EnviarAsync(envio, true);
        }

        async Task<RetConsReciNFe> ConsultarRespostaFinal(RetEnviNFe retTransmissao, bool homologacao)
        {
            var gerenciador = new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(
                retTransmissao.Estado, Operacoes.RespostaAutorizar, homologacao, true);
            var envio = new ConsReciNFe(retTransmissao.TipoAmbiente, retTransmissao.DadosRecibo.NumeroRecibo);
            return await gerenciador.EnviarAsync(envio);
        }

        public override InformacoesBase ObterVisualizacao()
        {
            if (ItemCompleto is NFCe nfe) return nfe.Informacoes;
            else if (ItemCompleto is ProcessoNFCe proc) return proc.NFe.Informacoes;
            throw new Exception();
        }
    }

    struct DadosImpressao
    {
        public DadosImpressao(ProcessoNFCe processo, Thickness margem, double largura)
        {
            Processo = processo;
            Margem = margem;
            Largura = largura;
        }

        ProcessoNFCe Processo { get; }
        Thickness Margem { get; }
        double Largura { get; }

        public static implicit operator ProcessoNFCe(DadosImpressao dados) => dados.Processo;
        public static implicit operator Thickness(DadosImpressao dados) => dados.Margem;
        public static implicit operator double(DadosImpressao dados) => dados.Largura;
    }
}
