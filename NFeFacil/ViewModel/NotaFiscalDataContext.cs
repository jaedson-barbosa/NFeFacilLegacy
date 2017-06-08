using BibliotecaCentral.IBGE;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using BibliotecaCentral.Validacao;
using NFeFacil.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BibliotecaCentral;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using System.Threading.Tasks;
using BibliotecaCentral.WebService.Pacotes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using BibliotecaCentral.WebService;
using Windows.Storage.Pickers;
using System.IO;
using Windows.UI.Popups;

namespace NFeFacil.ViewModel
{
    public sealed class NotaFiscalDataContext : INotifyPropertyChanged, IValida
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        private Popup Log = new Popup();
        public NFe NotaSalva { get; private set; }
        private Processo NotaEmitida;

        public bool ManipulacaoAtivada => StatusAtual == StatusNFe.Edição;
        public bool BotaoEditarVisivel => StatusAtual == StatusNFe.Validada || StatusAtual == StatusNFe.Salva || StatusAtual == StatusNFe.Assinada;
        public bool BotaoConfirmarVisivel => StatusAtual == StatusNFe.Edição;
        public bool BotaoSalvarAtivado => StatusAtual == StatusNFe.Validada;
        public bool BotaoAssinarAtivado => StatusAtual == StatusNFe.Salva;
        public bool BotaoTransmitirAtivado => StatusAtual == StatusNFe.Assinada;
        public bool BotaoGerarDANFEAtivado => StatusAtual == StatusNFe.Emitida || StatusAtual == StatusNFe.Impressa;
        public bool BotaoExportarXMLAtivado => StatusAtual != StatusNFe.Edição;

        internal StatusNFe StatusAtual
        {
            get => Conjunto.StatusAtual;
            set
            {
                Conjunto.StatusAtual = value;
                OnPropertyChanged(nameof(ManipulacaoAtivada),
                    nameof(BotaoEditarVisivel),
                    nameof(BotaoConfirmarVisivel),
                    nameof(BotaoSalvarAtivado),
                    nameof(BotaoAssinarAtivado),
                    nameof(BotaoTransmitirAtivado),
                    nameof(BotaoGerarDANFEAtivado),
                    nameof(BotaoExportarXMLAtivado));
            }
        }

        public List<ClienteDI> ClientesDisponiveis { get; }
        public List<EmitenteDI> EmitentesDisponiveis { get; }
        public List<ProdutoDI> ProdutosDisponiveis { get; }
        public List<MotoristaDI> MotoristasDisponiveis { get; }

        private ClienteDI clienteSelecionado;
        public ClienteDI ClienteSelecionado
        {
            get
            {
                var dest = NotaSalva.Informações.destinatário;
                if (clienteSelecionado == null && dest != null)
                {
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.Documento == dest.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                clienteSelecionado = value;
                NotaSalva.Informações.destinatário = value.ToDestinatario();
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        private EmitenteDI emitenteSelecionado;
        public EmitenteDI EmitenteSelecionado
        {
            get
            {
                var emit = NotaSalva.Informações.emitente;
                if (emitenteSelecionado == null && emit != null)
                {
                    emitenteSelecionado = EmitentesDisponiveis.FirstOrDefault(x => x.CNPJ == emit.CNPJ);
                }
                return emitenteSelecionado;
            }
            set
            {
                emitenteSelecionado = value;
                NotaSalva.Informações.emitente = value.ToEmitente();
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        public ProdutoDI ProdutoSelecionado { get; set; }

        private MotoristaDI motoristaSelecionado;
        public MotoristaDI MotoristaSelecionado
        {
            get
            {
                var mot = NotaSalva.Informações.transp?.transporta;
                if (motoristaSelecionado == null && mot != null)
                {
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                NotaSalva.Informações.transp.transporta = value.ToMotorista();
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        public int IndexPivotSelecionado { get; set; } = 0;
        private TipoOperacao OperacaoRequirida => Conjunto.OperacaoRequirida;
        ConjuntoManipuladorNFe Conjunto { get; }

        internal NotaFiscalDataContext(ref ConjuntoManipuladorNFe Dados)
        {
            using (var clientes = new Clientes())
            using (var emitentes = new Emitentes())
            using (var motoristas = new Motoristas())
            using (var produtos = new Produtos())
            {
                ClientesDisponiveis = clientes.Registro.ToList();
                EmitentesDisponiveis = emitentes.Registro.ToList();
                MotoristasDisponiveis = motoristas.Registro.ToList();
                ProdutosDisponiveis = produtos.Registro.ToList();
            }

            Conjunto = Dados;

            if (Dados.NotaEmitida != null)
            {
                NotaEmitida = Dados.NotaEmitida;
                NotaSalva = NotaEmitida.NFe;
            }
            else if (Dados.NotaSalva != null)
            {
                if (Dados.NotaSalva.Informações.total == null)
                    Dados.NotaSalva.Informações.total = new Total(Dados.NotaSalva.Informações.produtos);
                NotaSalva = Dados.NotaSalva;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public ICommand AdicionarProdutoCommand => new Comando(AdicionarProduto, true);
        public ICommand RemoverProdutoCommand => new Comando<DetalhesProdutos>(RemoverProduto);

        private void AdicionarProduto()
        {
            var detCompleto = new DetalhesProdutos
            {
                Produto = ProdutoSelecionado != null ? ProdutoSelecionado.ToProdutoOuServico() : new ProdutoOuServico()
            };
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoProdutoCompleto), detCompleto);
        }

        private void RemoverProduto(DetalhesProdutos produto)
        {
            NotaSalva.Informações.produtos.Remove(produto);
            OnPropertyChanged(nameof(NotaSalva));
        }

        public ICommand ObterNovoNumeroCommand => new Comando(ObterNovoNumero, true);
        public ICommand LiberarEdicaoCommand => new Comando(LiberarEdicao, true);
        public ICommand ConfirmarCommand => new Comando(Confirmar, true);
        public ICommand SalvarCommand => new Comando(() =>
        {
            NormalizarNFe();
            StatusAtual = StatusNFe.Salva;
            SalvarAsync();
            Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso. Agora podes sair da aplicação sem perder esta NFe.");
        }, true);
        public ICommand AssinarCommand => new Comando(Assinar, true);
        public ICommand TransmitirCommand => new Comando(Transmitir, true);
        public ICommand GerarDANFECommand => new Comando(GerarDANFE, true);
        public ICommand ExportarXMLCommand => new Comando(ExportarXML, true);

        private void ObterNovoNumero()
        {
            if (NotaSalva.Informações.emitente.CNPJ == null)
            {
                Log.Escrever(TitulosComuns.ErroSimples, "Primeiro escolha o emitente da nota fiscal.\n");
            }
            else
            {
                using (var notasFiscais = new NotasFiscais())
                {
                    NotaSalva.Informações.identificação.Numero = notasFiscais.ObterNovoNumero(NotaSalva.Informações.emitente.CNPJ, NotaSalva.Informações.identificação.Serie);
                    OnPropertyChanged(nameof(NotaSalva));
                }
            }
        }

        private void LiberarEdicao()
        {
            if (StatusAtual == StatusNFe.Assinada)
            {
                NotaSalva.Signature = null;
            }
            StatusAtual = StatusNFe.Edição;
            DesnormalizarNFe();
            Log.Escrever(TitulosComuns.Sucesso, "As alterações só terão efeito quando a nota fiscal for novamente salva.");
        }

        private void Confirmar()
        {
            if (NotaEmitida != null)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, "Comando não faz neste contexto.");
            }
            else
            {
                if (new ValidarDados(new ValidadorEmitente(NotaSalva.Informações.emitente),
                    new ValidadorDestinatario(NotaSalva.Informações.destinatário)).ValidarTudo(Log))
                {
                    NormalizarNFe();
                    Log.Escrever(TitulosComuns.ValidaçãoConcluída, "A nota fiscal foi validada. Aparentemente, não há irregularidades");
                    StatusAtual = StatusNFe.Validada;
                }
            }
        }

        public bool AmbienteTestes
        {
            get => NotaSalva.Informações.identificação.TipoAmbiente == 2;
            set
            {
                NotaSalva.Informações.identificação.TipoAmbiente = (ushort)(value ? 2 : 1);
                if (value)
                {
                    Log.Escrever(TitulosComuns.Atenção, "Notas feitas no ambiente de testes não são salvas e não tem valor fiscal.");
                }
            }
        }

        private void Assinar()
        {
            try
            {
                NormalizarNFe();
                var assina = new BibliotecaCentral.Certificacao.AssinaNFe(NotaSalva);
                assina.Assinar();
                StatusAtual = StatusNFe.Assinada;
                SalvarAsync();
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.ErroSimples, e.Message);
            }
        }

        private async void Transmitir()
        {
            var resultadoTransmissao = await new GerenciadorGeral<EnviNFe, RetEnviNFe>(NotaSalva.Informações.emitente.endereco.SiglaUF, Operacoes.Autorizar, AmbienteTestes)
                .EnviarAsync(new EnviNFe(NotaSalva.Informações.identificação.Numero, NotaSalva));
            if (resultadoTransmissao.cStat == 103)
            {
                var resultadoResposta = await new GerenciadorGeral<ConsReciNFe, RetConsReciNFe>(resultadoTransmissao.cUF, Operacoes.RespostaAutorizar, AmbienteTestes)
                    .EnviarAsync(new ConsReciNFe(resultadoTransmissao.tpAmb, resultadoTransmissao.infRec.nRec));
                if (resultadoResposta.protNFe.InfProt.cStat == 100)
                {
                    NotaEmitida = new Processo()
                    {
                        NFe = NotaSalva,
                        ProtNFe = resultadoResposta.protNFe
                    };
                    Log.Escrever(TitulosComuns.Sucesso, resultadoResposta.xMotivo);
                    StatusAtual = StatusNFe.Emitida;
                    SalvarAsync();
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, $"A nota fiscal foi processada, mas recusada. Mensagem de retorno: \n{resultadoResposta.protNFe.InfProt.xMotivo}");
                }
            }
            else
            {
                Log.Escrever(TitulosComuns.ErroSimples, $"A NFe não foi aceita. Mensagem de retorno: \n{resultadoTransmissao.xMotivo}\nPor favor, exporte esta nota fiscal e envie o XML gerado para o desenvolvedor do aplicativo para que o erro possa ser corrigido.");
            }
        }

        private async void ExportarXML()
        {
            FileSavePicker salvador = new FileSavePicker
            {
                DefaultFileExtension = ".xml",
                SuggestedFileName = $"{NotaSalva.Informações.Id}.xml",
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            salvador.FileTypeChoices.Add("Arquivo XML", new List<string> { ".xml" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                var xml = UsarNotaSalva ? NotaSalva.ToXElement<NFe>() : NotaEmitida.ToXElement<Processo>();
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    xml.Save(stream);
                    await stream.FlushAsync();
                }
                Log.Escrever(TitulosComuns.Sucesso, $"Nota fiscal exportada com sucesso para o caminho: {arquivo.Path}");
            }
        }

        private void GerarDANFE()
        {
            MainPage.Current.AbrirFunçao(typeof(ViewDANFE), NotaEmitida);
            StatusAtual = StatusNFe.Impressa;
            SalvarAsync();
        }

        #region Identificação

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informações.identificação.DataHoraEmissão))
                {
                    var agora = DateTime.Now;
                    NotaSalva.Informações.identificação.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informações.identificação.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informações.identificação.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(NotaSalva.Informações.identificação.DataHoraSaídaEntrada))
                {
                    return DataEmissao;
                }
                return DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                NotaSalva.Informações.identificação.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(NotaSalva.Informações.identificação.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                NotaSalva.Informações.identificação.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public ushort EstadoIdentificacao
        {
            get => NotaSalva.Informações.identificação.CódigoUF;
            set
            {
                NotaSalva.Informações.identificação.CódigoUF = value;
                OnPropertyChanged(nameof(EstadoIdentificacao));
            }
        }

        public ICommand AdicionarNFeReferenciadaCommand => new Comando(AdicionarNFeReferenciada, true);
        public ICommand AdicionarNFReferenciadaCommand => new Comando(AdicionarNFReferenciada, true);
        public ICommand RemoverDocReferenciadoCommand => new Comando<DocumentoFiscalReferenciado>(RemoverDocReferenciado);

        public ObservableCollection<DocumentoFiscalReferenciado> NFesReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => !string.IsNullOrEmpty(x.refNFe)).GerarObs();
        public ObservableCollection<DocumentoFiscalReferenciado> NFsReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => x.refNF != null).GerarObs();

        private async void AdicionarNFeReferenciada()
        {
            var caixa = new View.CaixasDialogo.AdicionarReferenciaEletronica();
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    refNFe = caixa.Chave
                });
                OnPropertyChanged(nameof(NFesReferenciadas));
            }
        }

        private async void AdicionarNFReferenciada()
        {
            var caixa = new View.CaixasDialogo.AdicionarNF1AReferenciada();
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                var contexto = (NF1AReferenciada)caixa.DataContext;
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    refNF = contexto
                });
                OnPropertyChanged(nameof(NFsReferenciadas));
            }
        }

        private void RemoverDocReferenciado(DocumentoFiscalReferenciado doc)
        {
            NotaSalva.Informações.identificação.DocumentosReferenciados.Remove(doc);
            OnPropertyChanged(nameof(NFesReferenciadas));
            OnPropertyChanged(nameof(NFsReferenciadas));
        }

        #endregion

        #region Transporte

        private Estado ufEscolhida;
        public Estado UFEscolhida
        {
            get
            {
                if (!string.IsNullOrEmpty(NotaSalva.Informações.transp.retTransp.cMunFG) && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == int.Parse(NotaSalva.Informações.transp.retTransp.cMunFG)) > 0)
                        {
                            ufEscolhida = item;
                        }
                    }
                }
                return ufEscolhida;
            }
            set
            {
                ufEscolhida = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UFEscolhida"));
            }
        }

        public ObservableCollection<ModalidadesTransporte> Modalidades => Extensoes.ObterItens<ModalidadesTransporte>();

        public ModalidadesTransporte ModFrete
        {
            get => (ModalidadesTransporte)NotaSalva.Informações.transp.modFrete;
            set => NotaSalva.Informações.transp.modFrete = (int)value;
        }

        public ICommand AdicionarReboqueCommand => new Comando(AdicionarReboque, true);
        public ICommand RemoverReboqueCommand => new Comando<Reboque>(RemoverReboque);
        public ICommand AdicionarVolumeCommand => new Comando(AdicionarVolume, true);
        public ICommand RemoverVolumeCommand => new Comando<Volume>(RemoverVolume);

        private async void AdicionarReboque()
        {
            var add = new View.CaixasDialogo.AdicionarReboque();
            add.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.transp.reboque.Add(x.DataContext as Reboque);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            };
            await add.ShowAsync();
        }

        private void RemoverReboque(Reboque reboque)
        {
            NotaSalva.Informações.transp.reboque.Remove(reboque);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        private async void AdicionarVolume()
        {
            var add = new View.CaixasDialogo.AdicionarVolume();
            add.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.transp.vol.Add(x.DataContext as Volume);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            };
            await add.ShowAsync();
        }

        private void RemoverVolume(Volume volume)
        {
            NotaSalva.Informações.transp.vol.Remove(volume);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        #endregion

        #region Cobrança

        public ICommand AdicionarDuplicataCommand => new Comando(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new Comando<Duplicata>(RemoverDuplicata);

        private async void AdicionarDuplicata()
        {
            var caixa = new View.CaixasDialogo.AdicionarDuplicata();
            caixa.PrimaryButtonClick += (sender, e) =>
            {
                NotaSalva.Informações.cobr.Dup.Add(sender.DataContext as Duplicata);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
            };
            await caixa.ShowAsync();
        }

        private void RemoverDuplicata(Duplicata duplicata)
        {
            NotaSalva.Informações.cobr.Dup.Remove(duplicata);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
        }

        #endregion;

        #region Cana

        public ICommand AdicionarFornecimentoCommand => new Comando(AdicionarFornecimento, true);
        public ICommand RemoverFornecimentoCommand => new Comando<FornecimentoDiario>(RemoverFornecimento);
        public ICommand AdicionarDeducaoCommand => new Comando(AdicionarDeducao, true);
        public ICommand RemoverDeducaoCommand => new Comando<Deducoes>(RemoverDeducao);

        public async void AdicionarFornecimento()
        {
            var caixa = new View.CaixasDialogo.AdicionarFornecimentoDiario();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.cana.forDia.Add(x.DataContext as FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            NotaSalva.Informações.cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new View.CaixasDialogo.AdicionarDeducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.cana.deduc.Add(x.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deducoes deducao)
        {
            NotaSalva.Informações.cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        #endregion

        #region InformacoesAdicionais

        public ICommand AdicionarObsContribuinteCommand => new Comando(AdicionarObsContribuinte, true);
        public ICommand RemoverObsContribuinteCommand => new Comando<Observacao>(RemoverObsContribuinte);
        public ICommand AdicionarProcReferenciadoCommand => new Comando(AdicionarProcReferenciado, true);
        public ICommand RemoverProcReferenciadoCommand => new Comando<ProcessoReferenciado>(RemoverProcReferenciado);

        private async void AdicionarObsContribuinte()
        {
            var caixa = new View.CaixasDialogo.AdicionarObservacaoContribuinte();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.infAdic.obsCont.Add((Observacao)x.DataContext);
                OnPropertyChanged(nameof(NotaSalva));
            };
            await caixa.ShowAsync();
        }

        private void RemoverObsContribuinte(Observacao obs)
        {
            NotaSalva.Informações.infAdic.obsCont.Remove(obs);
            OnPropertyChanged(nameof(NotaSalva));
        }

        private async void AdicionarProcReferenciado()
        {
            var caixa = new View.CaixasDialogo.AdicionarProcessoReferenciado();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                NotaSalva.Informações.infAdic.procRef.Add((ProcessoReferenciado)x.DataContext);
                OnPropertyChanged(nameof(NotaSalva));
            };
            await caixa.ShowAsync();
        }

        private void RemoverProcReferenciado(ProcessoReferenciado proc)
        {
            NotaSalva.Informações.infAdic.procRef.Remove(proc);
            OnPropertyChanged(nameof(NotaSalva));
        }

        #endregion

        #region Emitente

        public ICommand ExibirEmitente => new Comando(async () =>
        {
            var emit = new EmitenteDI(NotaSalva.Informações.emitente);
            var caixa = new View.CaixasDialogo.DetalheEmitenteAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new EmitenteDataContext(ref emit)
            };
            await caixa.ShowAsync();
        });

        public ICommand EditarEmitente => new Comando(async () =>
        {
            var emit = new EmitenteDI(NotaSalva.Informações.emitente);
            var caixa = new View.CaixasDialogo.DetalheEmitenteAtual()
            {
                ManipulacaoAtivada = true,
                DataContext = new EmitenteDataContext(ref emit)
            };
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                EmitenteSelecionado = ((EmitenteDataContext)caixa.DataContext).Emit;
            }
        });

        #endregion

        #region Cliente

        public ICommand ExibirCliente => new Comando(async () =>
        {
            var emit = new ClienteDI(NotaSalva.Informações.destinatário);
            var caixa = new View.CaixasDialogo.DetalheClienteAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new ClienteDataContext(ref emit)
            };
            await caixa.ShowAsync();
        });

        public ICommand EditarCliente => new Comando(async () =>
        {
            var emit = new ClienteDI(NotaSalva.Informações.destinatário);
            var caixa = new View.CaixasDialogo.DetalheClienteAtual()
            {
                ManipulacaoAtivada = true,
                DataContext = new ClienteDataContext(ref emit)
            };
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                ClienteSelecionado = ((ClienteDataContext)caixa.DataContext).Cliente;
            }
        });

        #endregion

        #region Motorista

        public ICommand ExibirMotorista => new Comando(async () =>
        {
            var emit = new MotoristaDI(NotaSalva.Informações.transp.transporta);
            var caixa = new View.CaixasDialogo.DetalheMotoristaAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new MotoristaDataContext(ref emit)
            };
            await caixa.ShowAsync();
        });

        public ICommand EditarMotorista => new Comando(async () =>
        {
            var emit = new MotoristaDI(NotaSalva.Informações.transp.transporta);
            var caixa = new View.CaixasDialogo.DetalheMotoristaAtual()
            {
                ManipulacaoAtivada = true,
                DataContext = new MotoristaDataContext(ref emit)
            };
            if (await caixa.ShowAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                MotoristaSelecionado = ((MotoristaDataContext)caixa.DataContext).Motorista;
            }
        });

        #endregion

        private void NormalizarNFe()
        {
            NotaSalva.Informações.transp.transporta = NotaSalva.Informações.transp.transporta?.ToXElement<Motorista>().HasElements ?? false ? NotaSalva.Informações.transp.transporta : null;
            NotaSalva.Informações.transp.veicTransp = ValidarVeiculo(NotaSalva.Informações.transp.veicTransp) ? NotaSalva.Informações.transp.veicTransp : null;
            NotaSalva.Informações.transp.retTransp = NotaSalva.Informações.transp.retTransp?.ToXElement<ICMSTransporte>().HasElements ?? false ? NotaSalva.Informações.transp.retTransp : null;

            NotaSalva.Informações.total.ISSQNtot = ValidarISSQN(NotaSalva.Informações.total.ISSQNtot) ? NotaSalva.Informações.total.ISSQNtot : null;
            NotaSalva.Informações.total.retTrib = ValidarRetencaoTributaria(NotaSalva.Informações.total.retTrib) ? NotaSalva.Informações.total.retTrib : null;
            NotaSalva.Informações.cobr = ValidarFatura(NotaSalva.Informações.cobr?.Fat) ? NotaSalva.Informações.cobr : null;
            NotaSalva.Informações.infAdic = ValidarInfoAdicional(NotaSalva.Informações.infAdic) ? NotaSalva.Informações.infAdic : null;
            NotaSalva.Informações.exporta = new ValidadorExportacao(NotaSalva.Informações.exporta).Validar(null) ? NotaSalva.Informações.exporta : null;
            NotaSalva.Informações.compra = ValidarCompra(NotaSalva.Informações.compra) ? NotaSalva.Informações.compra : null;
            NotaSalva.Informações.cana = ValidarCana(NotaSalva.Informações.cana) ? NotaSalva.Informações.cana : null;

            OnPropertyChanged(nameof(NotaSalva));

            bool ValidarVeiculo(Veiculo veic)
            {
                return StringsNaoNulas(veic.Placa, veic.UF);
            }

            bool ValidarISSQN(ISSQNtot tot)
            {
                if (tot == null)
                {
                    return false;
                }
                else
                {
                    return !string.IsNullOrEmpty(tot.dCompet);
                }
            }

            bool ValidarRetencaoTributaria(RetTrib ret)
            {
                if (ret == null)
                {
                    return false;
                }
                else
                {
                    return StringsNaoNulas(ret.vBCIRRF, ret.vBCRetPrev, ret.vIRRF, ret.vRetCOFINS,
                        ret.vRetCSLL, ret.vRetPIS, ret.vRetPrev);
                }
            }

            bool ValidarFatura(Fatura fat)
            {
                if (fat == null)
                {
                    return false;
                }
                else
                {
                    var erradosAnaliseSuperficial = new bool[4]
                    {
                        string.IsNullOrEmpty(fat.NFat),
                        string.IsNullOrEmpty(fat.VDesc),
                        string.IsNullOrEmpty(fat.VLiq),
                        string.IsNullOrEmpty(fat.VOrig)
                    };
                    var erradosAnaliseProfunda = new bool[4]
                    {
                        int.Parse(fat.NFat) == 0,
                        double.Parse(fat.VDesc) == 0,
                        double.Parse(fat.VLiq) == 0,
                        double.Parse(fat.VOrig) == 0
                    };
                    if (erradosAnaliseSuperficial.Count(x => x) == 4)
                    {
                        return false;
                    }
                    else
                    {
                        int quantNaoNulos = 0;
                        int quantErrados = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (!erradosAnaliseSuperficial[i])
                            {
                                quantNaoNulos++;
                                if (erradosAnaliseProfunda[i])
                                {
                                    quantErrados++;
                                }
                            }
                        }
                        return quantNaoNulos - quantErrados >= 1;
                    }
                }
            }

            bool ValidarInfoAdicional(InformacoesAdicionais info)
            {
                if (info == null)
                {
                    return false;
                }
                else
                {
                    var errados = new bool[3]
                    {
                        string.IsNullOrEmpty(info.infCpl),
                        info.obsCont.Count == 0,
                        info.procRef.Count == 0
                    };
                    return errados.Count(x => x) < 3;
                }
            }

            bool ValidarCompra(Compra compra)
            {
                if (compra == null)
                {
                    return false;
                }
                else
                {
                    return StringsNaoNulas(compra.XCont, compra.XNEmp, compra.XPed);
                }
            }

            bool ValidarCana(RegistroAquisicaoCana cana)
            {
                if (cana == null)
                {
                    return false;
                }
                else
                {
                    return cana.forDia.Count > 0;
                }
            }

            bool StringsNaoNulas(params string[] strings)
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    if (string.IsNullOrEmpty(strings[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Analisa os campos opcionais e, caso necessário, instancia novos objetos
        /// </summary>
        void DesnormalizarNFe()
        {
            if (NotaSalva.Informações.transp.transporta == null)
            {
                NotaSalva.Informações.transp.transporta = new Motorista();
            }
            if (NotaSalva.Informações.transp.veicTransp == null)
            {
                NotaSalva.Informações.transp.veicTransp = new Veiculo();
            }
            if (NotaSalva.Informações.transp.retTransp == null)
            {
                NotaSalva.Informações.transp.retTransp = new ICMSTransporte();
            }

            if (NotaSalva.Informações.cobr == null)
            {
                NotaSalva.Informações.cobr = new Cobranca();
            }
            if (NotaSalva.Informações.infAdic == null)
            {
                NotaSalva.Informações.infAdic = new InformacoesAdicionais();
            }
            if (NotaSalva.Informações.exporta == null)
            {
                NotaSalva.Informações.exporta = new Exportacao();
            }
            if (NotaSalva.Informações.compra == null)
            {
                NotaSalva.Informações.compra = new Compra();
            }
            if (NotaSalva.Informações.cana == null)
            {
                NotaSalva.Informações.cana = new RegistroAquisicaoCana();
            }
            OnPropertyChanged(nameof(NotaSalva));
        }

        private bool UsarNotaSalva => StatusAtual != StatusNFe.Emitida && StatusAtual != StatusNFe.Impressa;
        private void SalvarAsync()
        {
            if (!AmbienteTestes)
            {
                var xml = UsarNotaSalva ? NotaSalva.ToXElement<NFe>() : NotaEmitida.ToXElement<Processo>();
                var di = UsarNotaSalva ? new NFeDI(NotaSalva, xml.ToString()) : new NFeDI(NotaEmitida, xml.ToString());
                di.Status = (int)StatusAtual;
                using (var db = new NotasFiscais())
                {
                    if(db.Registro.Count(x => x.Id == di.Id) == 0)
                    {
                        db.Adicionar(di);
                    }
                    else
                    {
                        db.Atualizar(di);
                    }
                }
            }
        }

        async Task<bool> IValida.Verificar()
        {
            var retorno = true;
            if (StatusAtual == StatusNFe.Edição)
            {
                var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, Cancelar.", "Atenção");
                mensagem.Commands.Add(new UICommand("Sair"));
                mensagem.Commands.Add(new UICommand("Cancelar", x => retorno = false));
                await mensagem.ShowAsync();
            }
            return retorno;
        }
    }
}
