using BibliotecaCentral.IBGE;
using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
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
using Windows.ApplicationModel.Core;
using BibliotecaCentral;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal;
using System.Threading.Tasks;

namespace NFeFacil.ViewModel
{
    public sealed class NotaFiscalDataContext : INotifyPropertyChanged
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

        public bool BotaoConfirmarAtivado => StatusAtual == StatusNFe.EdiçãoCriação;
        public bool BotaoSalvarAtivado => StatusAtual == StatusNFe.Validado;
        public bool BotaoAssinarAtivado => StatusAtual == StatusNFe.Salvo;
        public bool BotaoTransmitirAtivado => StatusAtual == StatusNFe.Assinado;
        public bool BotaoGerarDANFEAtivado => StatusAtual == StatusNFe.Emitido;

        internal StatusNFe StatusAtual
        {
            get => Conjunto.StatusAtual;
            set
            {
                Conjunto.StatusAtual = value;
                OnPropertyChanged(nameof(BotaoConfirmarAtivado),
                    nameof(BotaoSalvarAtivado),
                    nameof(BotaoAssinarAtivado),
                    nameof(BotaoTransmitirAtivado),
                    nameof(BotaoGerarDANFEAtivado));
            }
        }

        public List<Destinatario> ClientesDisponiveis { get; }
        public List<Emitente> EmitentesDisponiveis { get; }
        public List<BaseProdutoOuServico> ProdutosDisponiveis { get; }
        public List<Motorista> MotoristasDisponiveis { get; }

        private Destinatario clienteSelecionado;
        public Destinatario ClienteSelecionado
        {
            get
            {
                if (clienteSelecionado == null)
                {
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.Documento == NotaSalva.Informações.destinatário.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                NotaSalva.Informações.destinatário = clienteSelecionado = value;
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        private Emitente emitenteSelecionado;
        public Emitente EmitenteSelecionado
        {
            get
            {
                if (emitenteSelecionado == null)
                {
                    emitenteSelecionado = EmitentesDisponiveis.FirstOrDefault(x => x.CNPJ == NotaSalva.Informações.emitente.CNPJ);
                }
                return emitenteSelecionado;
            }
            set
            {
                NotaSalva.Informações.emitente = emitenteSelecionado = value;
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        public BaseProdutoOuServico ProdutoSelecionado { get; set; }

        private Motorista motoristaSelecionado;
        public Motorista MotoristaSelecionado
        {
            get
            {
                if (motoristaSelecionado == null)
                {
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == NotaSalva.Informações.transp?.transporta?.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                NotaSalva.Informações.transp.transporta = motoristaSelecionado = value;
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

            if (CoreApplication.Properties.TryGetValue("ProdutoPendente", out object temp))
            {
                var detalhes = (DetalhesProdutos)temp;
                detalhes.número = Dados.NotaSalva.Informações.produtos.Count + 1;
                Dados.NotaSalva.Informações.produtos.Add(detalhes);
                IndexPivotSelecionado = 3;
                CoreApplication.Properties.Remove("ProdutoPendente");
                Dados.NotaSalva.Informações.total = new Total(Dados.NotaSalva.Informações.produtos);
            }

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

        public ICommand AdicionarProdutoCommand => new ComandoSimples(AdicionarProduto, true);
        public ICommand RemoverProdutoCommand => new ComandoParametrizado<DetalhesProdutos, ObterDataContext<DetalhesProdutos>>(RemoverProduto);

        private async void AdicionarProduto()
        {
            var detCompleto = new DetalhesProdutos
            {
                Produto = ProdutoSelecionado != null ? new ProdutoOuServico(ProdutoSelecionado) : new ProdutoOuServico()
            };
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ManipulacaoProdutoCompleto), detCompleto);
        }

        private void RemoverProduto(DetalhesProdutos produto)
        {
            NotaSalva.Informações.produtos.Remove(produto);
            OnPropertyChanged(nameof(Produtos));
        }

        public ICommand ConfirmarCommand => new ComandoSimples(Confirmar, true);
        public ICommand SalvarCommand => new ComandoSimples(async () =>
        {
            NormalizarNFe();
            StatusAtual = StatusNFe.Salvo;
            await SalvarAsync();
            Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso. Agora podes sair da aplicação sem perder esta NFe.");
        }, true);
        public ICommand AssinarCommand => new ComandoSimples(Assinar, true);
        public ICommand TransmitirCommand => new ComandoSimples(Transmitir, true);
        public ICommand GerarDANFECommand => new ComandoSimples(GerarDANFE, true);

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
                    StatusAtual = StatusNFe.Validado;
                }
            }
        }

        private bool nfeNormalizado = false;
        private void NormalizarNFe()
        {
            if (!nfeNormalizado)
            {
                NormalizarTranporte();
                NotaSalva.Informações.total.ISSQNtot = NotaSalva.Informações.total.ISSQNtot.ToXElement<ISSQNtot>().Elements().Count(x => x.Value != "0") > 0 ? NotaSalva.Informações.total.ISSQNtot : null;
                NotaSalva.Informações.total.retTrib = NotaSalva.Informações.total.retTrib.ToXElement<RetTrib>().HasElements ? NotaSalva.Informações.total.retTrib : null;
                NotaSalva.Informações.cobr = NotaSalva.Informações.cobr?.Fat.ToXElement<Fatura>().HasElements ?? false ? NotaSalva.Informações.cobr : null;
                NotaSalva.Informações.infAdic = NotaSalva.Informações.infAdic?.ToXElement<InformacoesAdicionais>().HasElements ?? false ? NotaSalva.Informações.infAdic : null;
                NotaSalva.Informações.exporta = new ValidadorExportacao(NotaSalva.Informações.exporta).Validar(null) ? NotaSalva.Informações.exporta : null;
                NotaSalva.Informações.compra = NotaSalva.Informações.compra?.ToXElement<Compra>().HasElements ?? false ? NotaSalva.Informações.compra : null;
                NotaSalva.Informações.cana = NotaSalva.Informações.cana?.ToXElement<RegistroAquisicaoCana>().HasElements ?? false ? NotaSalva.Informações.cana : null;
                nfeNormalizado = true;
            }
        }

        private void NormalizarTranporte()
        {
            NotaSalva.Informações.transp.transporta = NotaSalva.Informações.transp.transporta?.ToXElement<Motorista>().HasElements ?? false ? NotaSalva.Informações.transp.transporta : null;
            NotaSalva.Informações.transp.veicTransp = NotaSalva.Informações.transp.veicTransp?.ToXElement<Veiculo>().HasElements ?? false ? NotaSalva.Informações.transp.veicTransp : null;
            NotaSalva.Informações.transp.retTransp = NotaSalva.Informações.transp.retTransp?.ToXElement<ICMSTransporte>().HasElements ?? false ? NotaSalva.Informações.transp.retTransp : null;
        }

        private static T RemoverDefault<T>(T valor) where T : class => valor != null && valor != default(T) ? valor : null;
        private static T InserirDefault<T>(T valor) where T : class => valor ?? default(T);

        private async Task SalvarAsync()
        {
            bool usarNotaSalva = StatusAtual != StatusNFe.Emitido && StatusAtual != StatusNFe.Impresso;
            var xml = usarNotaSalva ? NotaSalva.ToXElement<NFe>() : NotaEmitida.ToXElement<Processo>();
            var di = usarNotaSalva ? new NFeDI(NotaSalva) : new NFeDI(NotaEmitida);
            di.Status = (int)StatusAtual;
            using (var db = new NotasFiscais())
            {
                if (StatusAtual == StatusNFe.Salvo || StatusAtual == StatusNFe.EdiçãoCriação)
                {
                    await db.Adicionar(di, xml);
                }
                else
                {
                    await db.Atualizar(di, xml);
                }
            }
        }

        private async void Assinar()
        {
            NormalizarNFe();
            var assina = new BibliotecaCentral.Assinatura.AssinaNFe(NotaSalva);
            assina.Assinar();
            StatusAtual = StatusNFe.Assinado;
            await SalvarAsync();
        }

        private async void Transmitir()
        {
            var estado = Estados.EstadosCache.First(x => x.Sigla == NotaSalva.Informações.emitente.endereco.SiglaUF);
            var resultadoTransmissao = await BibliotecaCentral.WebService.AutorizarNota.Gerenciador.AutorizarAsync(estado.Codigo, NotaSalva);
            if (resultadoTransmissao.retEnviNFe.cStat == 103)
            {
                var resultadoResposta = await BibliotecaCentral.WebService.RespostaAutorizarNota.Gerenciador.ObterRespostaAutorizacao(resultadoTransmissao.retEnviNFe);
                if (resultadoResposta.retConsReciNFe.protNFe.InfProt.cStat == 100)
                {
                    NotaEmitida = new Processo()
                    {
                        NFe = NotaSalva,
                        ProtNFe = resultadoResposta.retConsReciNFe.protNFe
                    };
                    Log.Escrever(TitulosComuns.Sucesso, resultadoResposta.retConsReciNFe.xMotivo);
                    StatusAtual = StatusNFe.Emitido;
                    await SalvarAsync();
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, $"A nota fiscal foi processada, mas recusada. Mensagem de retorno: \n{resultadoResposta.retConsReciNFe.protNFe.InfProt.xMotivo}");
                }
            }
            else
            {
                Log.Escrever(TitulosComuns.ErroSimples, $"A NFe não foi aceita. Mensagem de retorno: \n{resultadoTransmissao.retEnviNFe.xMotivo}\nPor favor, exporte esta nota fiscal e envie o XML gerado para o desenvolvedor do aplicativo para que o erro possa ser corrigido.");
            }
        }

        private async void GerarDANFE()
        {
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ViewDANFE), NotaEmitida);
            StatusAtual = StatusNFe.Impresso;
            await SalvarAsync();
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
                    var agora = DateTime.Now;
                    NotaSalva.Informações.identificação.DataHoraSaídaEntrada = agora.ToStringPersonalizado();
                    return agora;
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

        public ICommand AdicionarReboqueCommand => new ComandoSimples(AdicionarReboque, true);
        public ICommand RemoverReboqueCommand => new ComandoParametrizado<Reboque, ObterDataContext<Reboque>>(RemoverReboque);
        public ICommand AdicionarVolumeCommand => new ComandoSimples(AdicionarVolume, true);
        public ICommand RemoverVolumeCommand => new ComandoParametrizado<Volume, ObterDataContext<Volume>>(RemoverVolume);

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

        public ICommand AdicionarDuplicataCommand => new ComandoSimples(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new ComandoParametrizado<Duplicata, ObterDataContext<Duplicata>>(RemoverDuplicata);

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

        public ICommand AdicionarFornecimentoCommand => new ComandoSimples(AdicionarFornecimento, true);
        public ICommand RemoverFornecimentoCommand => new ComandoParametrizado<FornecimentoDiario, ObterDataContext<FornecimentoDiario>>(RemoverFornecimento);
        public ICommand AdicionarDeducaoCommand => new ComandoSimples(AdicionarDeducao, true);
        public ICommand RemoverDeducaoCommand => new ComandoParametrizado<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);

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
    }
}
