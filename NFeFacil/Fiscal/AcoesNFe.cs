using NFeFacil.Fiscal.ViewNFe;
using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using NFeFacil.Validacao;
using NFeFacil.View;
using NFeFacil.WebService;
using NFeFacil.WebService.Pacotes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Pickers;

namespace NFeFacil.Fiscal
{
    sealed class AcoesNFe : AcoesVisualizacao
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
                var assina = new Certificacao.AssinaFacil()
                {
                    Nota = nfe
                };
                await assina.Preparar();
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var result = await assina.Assinar(x, nfe.Informacoes.Id, "infNFe");
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
            var nfe = (NFe)ItemCompleto;
            var analisador = new AnalisadorNFe(ref nfe);
            analisador.Desnormalizar();
            ItemBanco.Status = (int)StatusNota.Edição;
            MainPage.Current.Navegar<ManipulacaoNotaFiscal>(nfe);
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
            MainPage.Current.Navegar<ViewDANFE>(processo);
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

                    var homologacao = ((NFe)ItemCompleto).AmbienteTestes;
                    var resultadoResposta = await ConsultarRespostaFinal(retTransmissao, homologacao);
                    await progresso.Update(3);

                    var protocoloResposta = resultadoResposta.Protocolo.InfProt;
                    if (protocoloResposta?.cStat == 100)
                    {
                        ItemCompleto = new ProcessoNFe()
                        {
                            NFe = (NFe)ItemCompleto,
                            ProtNFe = resultadoResposta.Protocolo
                        };
                        ItemBanco.Status = (int)StatusNota.Emitida;
                        AtualizarDI(ItemCompleto);
                        OnStatusChanged(StatusNota.Emitida);
                        await progresso.Update(4);

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
            var gerenciador = new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(
                retTransmissao.Estado, Operacoes.RespostaAutorizar, homologacao, false);
            var envio = new ConsReciNFe(retTransmissao.TipoAmbiente, retTransmissao.DadosRecibo.NumeroRecibo);
            return await gerenciador.EnviarAsync(envio);
        }

        public override InformacoesBase ObterVisualizacao()
        {
            if (ItemCompleto is NFe nfe) return nfe.Informacoes;
            else if (ItemCompleto is ProcessoNFe proc) return proc.NFe.Informacoes;
            throw new Exception();
        }
    }
}
