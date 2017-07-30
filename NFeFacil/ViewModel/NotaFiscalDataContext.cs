using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using NFeFacil.Repositorio;
using NFeFacil.Validacao;
using NFeFacil.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class NotaFiscalDataContext : INotifyPropertyChanged, IValida
    {
        #region Constantes

        const string NomeClienteHomologacao = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        #region Propriedades públicas

        public NFe NotaSalva { get; private set; }

        public bool ManipulacaoAtivada => StatusAtual == StatusNFe.Edição;
        public bool BotaoEditarVisivel => StatusAtual == (StatusNFe.Validada | StatusNFe.Salva | StatusNFe.Assinada);
        public bool BotaoConfirmarVisivel => StatusAtual == StatusNFe.Edição;
        public bool BotaoSalvarAtivado => StatusAtual == StatusNFe.Validada;
        public bool BotaoAssinarAtivado => StatusAtual == StatusNFe.Salva;
        public bool BotaoTransmitirAtivado => StatusAtual == StatusNFe.Assinada;
        public bool BotaoGerarDANFEAtivado => StatusAtual == StatusNFe.Emitida;

        #region Dados base
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
                if (AmbienteTestes)
                {
                    NotaSalva.Informações.destinatário.Nome = NomeClienteHomologacao;
                }
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
                    emitenteSelecionado = EmitentesDisponiveis.FirstOrDefault(x => long.Parse(x.CNPJ) == emit.CNPJ);
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
                var mot = NotaSalva.Informações.transp?.Transporta;
                if (motoristaSelecionado == null && mot != null && mot.Documento != null)
                {
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == mot.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                motoristaSelecionado = value;
                var transporte = NotaSalva.Informações.transp;
                transporte.Transporta = value.ToMotorista();
                if (value.Veiculo != default(Guid))
                {
                    using (var db = new AplicativoContext())
                        transporte.VeicTransp = db.Veiculos.Find(value.Veiculo).ToVeiculo();
                }
                else
                {
                    transporte.VeicTransp = new Veiculo();
                }
                OnPropertyChanged(nameof(NotaSalva));
            }
        }
        #endregion

        public bool AmbienteTestes
        {
            get => NotaSalva.Informações.identificação.TipoAmbiente == 2;
            set
            {
                NotaSalva.Informações.identificação.TipoAmbiente = value ? (byte)2 : (byte)1;
                if (value)
                {
                    NotaSalva.Informações.destinatário.Nome = NomeClienteHomologacao;
                    Log.Escrever(TitulosComuns.Atenção, $"O nome do cliente foi alterado para que a nota seja aceita pela SEFAZ, o novo valor é {NomeClienteHomologacao}.");
                }
                else
                {
                    Log.Escrever(TitulosComuns.Atenção, "Verifique se o nome do cliente está correto.");
                }
            }
        }

        #region Identificação
        public ObservableCollection<DocumentoFiscalReferenciado> NFesReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => !string.IsNullOrEmpty(x.RefNFe)).GerarObs();
        public ObservableCollection<DocumentoFiscalReferenciado> NFsReferenciadas => NotaSalva.Informações.identificação.DocumentosReferenciados.Where(x => x.RefNF != null).GerarObs();

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

        public byte EstadoIdentificacao
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
                if (NotaSalva.Informações.transp.RetTransp.CMunFG != 0 && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == NotaSalva.Informações.transp.RetTransp.CMunFG) > 0)
                        {
                            ufEscolhida = item;
                            break;
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
            get => (ModalidadesTransporte)NotaSalva.Informações.transp.ModFrete;
            set => NotaSalva.Informações.transp.ModFrete = (byte)value;
        }
        #endregion

        #region Endereço de retirada
        public RetiradaOuEntrega Retirada => NotaSalva.Informações.Retirada;

        public bool exteriorEnderecoRetirada;
        public bool ExteriorEnderecoRetirada
        {
            get => Retirada != null ? Retirada.SiglaUF == "EX" : false;
            set
            {
                exteriorEnderecoRetirada = value;
                if (value)
                {
                    Retirada.CodigoMunicipio = 9999999;
                    Retirada.NomeMunicipio = "EXTERIOR";
                    Retirada.SiglaUF = "EX";
                }
                else
                {
                    Retirada.CodigoMunicipio = 0;
                    Retirada.NomeMunicipio = null;
                    Retirada.SiglaUF = null;
                }
                OnPropertyChanged(nameof(ExteriorEnderecoRetirada));
            }
        }

        public bool EnderecoRetiradaAtivado
        {
            get => Retirada != null;
            set
            {
                NotaSalva.Informações.Retirada = value ? new RetiradaOuEntrega() : null;
                OnPropertyChanged(nameof(Retirada), nameof(EdicaoEnderecoRetiradaAtivado), nameof(TipoDocumentoEnderecoEmitente),
                    nameof(DocumentoEnderecoEmitente), nameof(UFEscolhidaEnderecoEmitente), nameof(ConjuntoMunicipioEnderecoEmitente));
            }
        }

        public bool EdicaoEnderecoRetiradaAtivado => EnderecoRetiradaAtivado && ManipulacaoAtivada;

        TiposDocumento tipoDocumentoEnderecoEmitente;
        public int TipoDocumentoEnderecoEmitente
        {
            get => (int)(tipoDocumentoEnderecoEmitente = string.IsNullOrEmpty(Retirada?.CNPJ) ? TiposDocumento.CPF : TiposDocumento.CNPJ);
            set => tipoDocumentoEnderecoEmitente = (TiposDocumento)value;
        }

        public long DocumentoEnderecoEmitente
        {
            get
            {
                long.TryParse(tipoDocumentoEnderecoEmitente == TiposDocumento.CNPJ ? Retirada.CNPJ : Retirada?.CPF, out long retorno);
                return retorno;
            }
            set
            {
                if (tipoDocumentoEnderecoEmitente == TiposDocumento.CPF)
                {
                    Retirada.CNPJ = null;
                    Retirada.CPF = value.ToString();
                }
                else
                {
                    Retirada.CPF = null;
                    Retirada.CNPJ = value.ToString();
                }
            }
        }

        public string UFEscolhidaEnderecoEmitente
        {
            get => Retirada?.SiglaUF;
            set
            {
                NotaSalva.Informações.Retirada.SiglaUF = value;
                OnPropertyChanged(nameof(UFEscolhidaEnderecoEmitente));
            }
        }

        public Municipio ConjuntoMunicipioEnderecoEmitente
        {
            get => Municipios.Get(UFEscolhidaEnderecoEmitente).FirstOrDefault(x => x.Codigo == NotaSalva.Informações.Retirada.CodigoMunicipio);
            set
            {
                if (value != null)
                {
                    NotaSalva.Informações.Retirada.CodigoMunicipio = value.Codigo;
                    NotaSalva.Informações.Retirada.NomeMunicipio = value.Nome;
                }
            }
        }
        #endregion

        #region Endereço de entrega
        public RetiradaOuEntrega Entrega => NotaSalva.Informações.Entrega;

        public bool exteriorEnderecoEntrega;
        public bool ExteriorEnderecoEntrega
        {
            get => Entrega != null ? Entrega.SiglaUF == "EX" : false;
            set
            {
                exteriorEnderecoEntrega = value;
                if (value)
                {
                    Entrega.CodigoMunicipio = 9999999;
                    Entrega.NomeMunicipio = "EXTERIOR";
                    Entrega.SiglaUF = "EX";
                }
                else
                {
                    Entrega.CodigoMunicipio = 0;
                    Entrega.NomeMunicipio = null;
                    Entrega.SiglaUF = null;
                }
                OnPropertyChanged(nameof(ExteriorEnderecoEntrega));
            }
        }

        public bool EnderecoEntregaAtivado
        {
            get => Entrega != null;
            set
            {
                NotaSalva.Informações.Entrega = value ? new RetiradaOuEntrega() : null;
                OnPropertyChanged(nameof(Entrega), nameof(EdicaoEnderecoEntregaAtivado), nameof(TipoDocumentoEnderecoEmitente),
                    nameof(DocumentoEnderecoEmitente), nameof(UFEscolhidaEnderecoEmitente), nameof(ConjuntoMunicipioEnderecoEmitente));
            }
        }

        public bool EdicaoEnderecoEntregaAtivado => EnderecoEntregaAtivado && ManipulacaoAtivada;

        TiposDocumento tipoDocumentoEnderecoCliente;
        public int TipoDocumentoEnderecoCliente
        {
            get => (int)(tipoDocumentoEnderecoCliente = string.IsNullOrEmpty(Entrega?.CNPJ) ? TiposDocumento.CPF : TiposDocumento.CNPJ);
            set => tipoDocumentoEnderecoCliente = (TiposDocumento)value;
        }

        public long DocumentoEnderecoCliente
        {
            get
            {
                long.TryParse(tipoDocumentoEnderecoCliente == TiposDocumento.CNPJ ? Entrega.CNPJ : Entrega?.CPF, out long retorno);
                return retorno;
            }
            set
            {
                if (tipoDocumentoEnderecoCliente == TiposDocumento.CPF)
                {
                    Entrega.CNPJ = null;
                    Entrega.CPF = value.ToString();
                }
                else
                {
                    Entrega.CPF = null;
                    Entrega.CNPJ = value.ToString();
                }
            }
        }

        public string UFEscolhidaEnderecoCliente
        {
            get => Entrega?.SiglaUF;
            set
            {
                NotaSalva.Informações.Entrega.SiglaUF = value;
                OnPropertyChanged(nameof(UFEscolhidaEnderecoCliente));
            }
        }

        public Municipio ConjuntoMunicipioEnderecoCliente
        {
            get => Municipios.Get(UFEscolhidaEnderecoCliente).FirstOrDefault(x => x.Codigo == NotaSalva.Informações.Entrega.CodigoMunicipio);
            set
            {
                if (value != null)
                {
                    NotaSalva.Informações.Entrega.CodigoMunicipio = value.Codigo;
                    NotaSalva.Informações.Entrega.NomeMunicipio = value.Nome;
                }
            }
        }
        #endregion

        #endregion

        #region Comandos

        public ICommand AdicionarProdutoCommand => new Comando(AdicionarProduto, true);
        public ICommand EditarProdutoCommand => new Comando<DetalhesProdutos>(EditarProduto);
        public ICommand RemoverProdutoCommand => new Comando<DetalhesProdutos>(RemoverProduto);

        public ICommand ObterNovoNumeroCommand => new Comando(ObterNovoNumero);
        public ICommand LiberarEdicaoCommand => new Comando(LiberarEdicao);
        public ICommand ConfirmarCommand => new Comando(Confirmar);
        public ICommand SalvarCommand => new Comando(Salvar);
        public ICommand AssinarCommand => new Comando(Assinar);
        public ICommand TransmitirCommand => new Comando(Transmitir);
        public ICommand GerarDANFECommand => new Comando(GerarDANFE);
        public ICommand ExportarXMLCommand => new Comando(ExportarXML);

        public ICommand AdicionarNFeReferenciadaCommand => new Comando(AdicionarNFeReferenciada, true);
        public ICommand AdicionarNFReferenciadaCommand => new Comando(AdicionarNFReferenciada, true);
        public ICommand RemoverDocReferenciadoCommand => new Comando<DocumentoFiscalReferenciado>(RemoverDocReferenciado);

        public ICommand AdicionarReboqueCommand => new Comando(AdicionarReboque, true);
        public ICommand RemoverReboqueCommand => new Comando<Reboque>(RemoverReboque);
        public ICommand AdicionarVolumeCommand => new Comando(AdicionarVolume, true);
        public ICommand RemoverVolumeCommand => new Comando<Volume>(RemoverVolume);

        public ICommand AdicionarDuplicataCommand => new Comando(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new Comando<Duplicata>(RemoverDuplicata);

        public ICommand AdicionarFornecimentoCommand => new Comando(AdicionarFornecimento, true);
        public ICommand RemoverFornecimentoCommand => new Comando<FornecimentoDiario>(RemoverFornecimento);
        public ICommand AdicionarDeducaoCommand => new Comando(AdicionarDeducao, true);
        public ICommand RemoverDeducaoCommand => new Comando<Deducoes>(RemoverDeducao);

        public ICommand AdicionarObsContribuinteCommand => new Comando(AdicionarObsContribuinte, true);
        public ICommand RemoverObsContribuinteCommand => new Comando<Observacao>(RemoverObsContribuinte);
        public ICommand AdicionarProcReferenciadoCommand => new Comando(AdicionarProcReferenciado, true);
        public ICommand RemoverProcReferenciadoCommand => new Comando<ProcessoReferenciado>(RemoverProcReferenciado);

        public ICommand ExibirEmitenteCommand => new Comando(ExibirEmitente);
        public ICommand ExibirClienteCommand => new Comando(ExibirCliente);
        public ICommand EditarClienteCommand => new Comando(EditarCliente);
        public ICommand ExibirMotoristaCommand => new Comando(ExibirMotorista);
        public ICommand EditarMotoristaCommand => new Comando(EditarMotorista);

        #endregion

        Popup Log = Popup.Current;
        Processo NotaEmitida;
        AnalisadorNFe Analisador { get; }
        OperacoesNotaSalva OperacoesNota { get; }
        ConjuntoManipuladorNFe Conjunto { get; }

        StatusNFe StatusAtual
        {
            get => Conjunto.StatusAtual;
            set
            {
                Conjunto.StatusAtual = value;
                OnPropertyChanged(nameof(ManipulacaoAtivada),
                    nameof(EdicaoEnderecoRetiradaAtivado),
                    nameof(EdicaoEnderecoEntregaAtivado),
                    nameof(BotaoEditarVisivel),
                    nameof(BotaoConfirmarVisivel),
                    nameof(BotaoSalvarAtivado),
                    nameof(BotaoAssinarAtivado),
                    nameof(BotaoTransmitirAtivado),
                    nameof(BotaoGerarDANFEAtivado));
            }
        }

        internal NotaFiscalDataContext(ref ConjuntoManipuladorNFe Dados)
        {
            using (var db = new AplicativoContext())
            {
                ClientesDisponiveis = db.Clientes.ToList();
                EmitentesDisponiveis = db.Emitentes.ToList();
                MotoristasDisponiveis = db.Motoristas.ToList();
                ProdutosDisponiveis = db.Produtos.ToList();
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

            Conjunto = Dados;
            Analisador = new AnalisadorNFe(NotaSalva);
            OperacoesNota = new OperacoesNotaSalva(Log, Analisador);
        }

        void ObterNovoNumero()
        {
            if (NotaSalva.Informações.emitente.CNPJ == 0)
            {
                Log.Escrever(TitulosComuns.Erro, "Primeiro escolha o emitente da nota fiscal.");
            }
            else
            {
                var cnpj = NotaSalva.Informações.emitente.CNPJ;
                var serie = NotaSalva.Informações.identificação.Serie;
                NotaSalva.Informações.identificação.Numero = NotasFiscais.ObterNovoNumero(cnpj, serie);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void LiberarEdicao()
        {
            if (StatusAtual == StatusNFe.Assinada)
            {
                NotaSalva.Signature = null;
            }
            StatusAtual = StatusNFe.Edição;
            Analisador.Desnormalizar();
            Log.Escrever(TitulosComuns.Sucesso, "As alterações só terão efeito quando a nota fiscal for novamente salva.");
        }

        void Confirmar()
        {
            try
            {
                if (new ValidarDados(new ValidadorEmitente(NotaSalva.Informações.emitente),
                    new ValidadorDestinatario(NotaSalva.Informações.destinatário)).ValidarTudo(Log))
                {
                    NotaSalva.Informações.AtualizarChave();
                    Analisador.Normalizar();
                    Log.Escrever(TitulosComuns.ValidaçãoConcluída, "A nota fiscal foi validada. Aparentemente, não há irregularidades");
                    StatusAtual = StatusNFe.Validada;
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        void Salvar()
        {
            Analisador.Normalizar();
            StatusAtual = StatusNFe.Salva;
            AtualizarDI();
            Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso. Agora podes sair da aplicação sem perder esta NFe.");
        }

        async void Assinar()
        {
            if (await OperacoesNota.Assinar(NotaSalva))
            {
                StatusAtual = StatusNFe.Assinada;
                AtualizarDI();
            }
        }

        async void Transmitir()
        {
            var resposta = await OperacoesNota.Transmitir(NotaSalva, AmbienteTestes);
            if (resposta.sucesso)
            {
                NotaEmitida = new Processo()
                {
                    NFe = NotaSalva,
                    ProtNFe = resposta.protocolo
                };
                Log.Escrever(TitulosComuns.Sucesso, resposta.motivo);
                StatusAtual = StatusNFe.Emitida;
                AtualizarDI();
            }
        }

        async void ExportarXML()
        {
            var xml = ObterXML();
            if (await OperacoesNota.Exportar(xml, NotaSalva.Informações.Id))
            {
                Log.Escrever(TitulosComuns.Sucesso, $"Nota fiscal exportada com sucesso.");
                Conjunto.Exportada = true;
                AtualizarDI();
            }
        }

        void GerarDANFE()
        {
            MainPage.Current.AbrirFunçao(typeof(ViewDANFE), NotaEmitida);
            Conjunto.Impressa = true;
            AtualizarDI();
        }

        #region Adição e remoção básica

        void AdicionarProduto()
        {
            var detCompleto = new DetalhesProdutos
            {
                Produto = ProdutoSelecionado != null ? ProdutoSelecionado.ToProdutoOuServico() : new ProdutoOuServico()
            };
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoProdutoCompleto), detCompleto);
        }

        void EditarProduto(DetalhesProdutos produto)
        {
            var detCompleto = produto;
            MainPage.Current.AbrirFunçao(typeof(ManipulacaoProdutoCompleto), detCompleto);
        }

        void RemoverProduto(DetalhesProdutos produto)
        {
            NotaSalva.Informações.produtos.Remove(produto);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarNFeReferenciada()
        {
            var caixa = new View.CaixasDialogo.AdicionarReferenciaEletronica();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    RefNFe = caixa.Chave
                });
                OnPropertyChanged(nameof(NFesReferenciadas));
            }
        }

        async void AdicionarNFReferenciada()
        {
            var caixa = new View.CaixasDialogo.AdicionarNF1AReferenciada();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var contexto = (NF1AReferenciada)caixa.DataContext;
                NotaSalva.Informações.identificação.DocumentosReferenciados.Add(new DocumentoFiscalReferenciado
                {
                    RefNF = contexto
                });
                OnPropertyChanged(nameof(NFsReferenciadas));
            }
        }

        void RemoverDocReferenciado(DocumentoFiscalReferenciado doc)
        {
            NotaSalva.Informações.identificação.DocumentosReferenciados.Remove(doc);
            OnPropertyChanged(nameof(NFesReferenciadas), nameof(NFsReferenciadas));
        }

        async void AdicionarReboque()
        {
            var add = new View.CaixasDialogo.AdicionarReboque();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.transp.Reboque.Add(add.DataContext as Reboque);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            }
        }

        void RemoverReboque(Reboque reboque)
        {
            NotaSalva.Informações.transp.Reboque.Remove(reboque);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        async void AdicionarVolume()
        {
            var add = new View.CaixasDialogo.AdicionarVolume();
            if (await add.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.transp.Vol.Add(add.DataContext as Volume);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            }
        }

        void RemoverVolume(Volume volume)
        {
            NotaSalva.Informações.transp.Vol.Remove(volume);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        async void AdicionarDuplicata()
        {
            var caixa = new View.CaixasDialogo.AdicionarDuplicata();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cobr.Dup.Add(caixa.DataContext as Duplicata);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
            }
        }

        void RemoverDuplicata(Duplicata duplicata)
        {
            NotaSalva.Informações.cobr.Dup.Remove(duplicata);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
        }

        async void AdicionarFornecimento()
        {
            var caixa = new View.CaixasDialogo.AdicionarFornecimentoDiario();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cana.ForDia.Add(caixa.DataContext as FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            }
        }

        void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            NotaSalva.Informações.cana.ForDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        async void AdicionarDeducao()
        {
            var caixa = new View.CaixasDialogo.AdicionarDeducao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.cana.Deduc.Add(caixa.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
            }
        }

        void RemoverDeducao(Deducoes deducao)
        {
            NotaSalva.Informações.cana.Deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotaSalva)));
        }

        async void AdicionarObsContribuinte()
        {
            var caixa = new View.CaixasDialogo.AdicionarObservacaoContribuinte();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.infAdic.ObsCont.Add((Observacao)caixa.DataContext);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverObsContribuinte(Observacao obs)
        {
            NotaSalva.Informações.infAdic.ObsCont.Remove(obs);
            OnPropertyChanged(nameof(NotaSalva));
        }

        async void AdicionarProcReferenciado()
        {
            var caixa = new View.CaixasDialogo.AdicionarProcessoReferenciado();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                NotaSalva.Informações.infAdic.ProcRef.Add((ProcessoReferenciado)caixa.DataContext);
                OnPropertyChanged(nameof(NotaSalva));
            }
        }

        void RemoverProcReferenciado(ProcessoReferenciado proc)
        {
            NotaSalva.Informações.infAdic.ProcRef.Remove(proc);
            OnPropertyChanged(nameof(NotaSalva));
        }

        #endregion

        #region Exibição e edição básica

        async void ExibirEmitente()
        {
            var emit = new EmitenteDI(NotaSalva.Informações.emitente);
            var caixa = new View.CaixasDialogo.DetalheEmitenteAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new EmitenteDataContext(ref emit)
            };
            await caixa.ShowAsync();
        }

        async void ExibirCliente()
        {
            var emit = new ClienteDI(NotaSalva.Informações.destinatário);
            var caixa = new View.CaixasDialogo.DetalheClienteAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new ClienteDataContext(ref emit)
            };
            await caixa.ShowAsync();
        }

        async void EditarCliente()
        {
            var emit = new ClienteDI(NotaSalva.Informações.destinatário);
            var caixa = new View.CaixasDialogo.DetalheClienteAtual()
            {
                ManipulacaoAtivada = true,
                DataContext = new ClienteDataContext(ref emit)
            };
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ClienteSelecionado = ((ClienteDataContext)caixa.DataContext).Cliente;
            }
        }

        async void ExibirMotorista()
        {
            var emit = new MotoristaDI(NotaSalva.Informações.transp.Transporta);
            var caixa = new View.CaixasDialogo.DetalheMotoristaAtual()
            {
                ManipulacaoAtivada = false,
                DataContext = new MotoristaDataContext(ref emit)
            };
            await caixa.ShowAsync();
        }

        async void EditarMotorista()
        {
            var emit = new MotoristaDI(NotaSalva.Informações.transp.Transporta);
            var caixa = new View.CaixasDialogo.DetalheMotoristaAtual()
            {
                ManipulacaoAtivada = true,
                DataContext = new MotoristaDataContext(ref emit)
            };
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                MotoristaSelecionado = ((MotoristaDataContext)caixa.DataContext).Motorista;
            }
        }

        #endregion

        private void AtualizarDI()
        {
            try
            {
                var di = ObterDI();
                using (var db = new AplicativoContext())
                {
                    di.UltimaData = DateTime.Now;
                    if (db.NotasFiscais.Count(x => x.Id == di.Id) == 0)
                    {
                        db.Add(di);
                    }
                    else if (db.NotasFiscais.First(x => x.Id == di.Id) != di)
                    {
                        db.Update(di);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
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

        XElement ObterXML()
        {
            bool UsarNotaSalva = StatusAtual != StatusNFe.Emitida;
            return UsarNotaSalva ? NotaSalva.ToXElement<NFe>() : NotaEmitida.ToXElement<Processo>();
        }

        NFeDI ObterDI()
        {
            bool UsarNotaSalva = StatusAtual != StatusNFe.Emitida && StatusAtual != StatusNFe.Cancelada;
            var xml = UsarNotaSalva ? NotaSalva.ToXElement<NFe>() : NotaEmitida.ToXElement<Processo>();
            var di = UsarNotaSalva ? new NFeDI(NotaSalva, xml.ToString()) : new NFeDI(NotaEmitida, xml.ToString());
            di.Status = (int)StatusAtual;
            di.Exportada = Conjunto.Exportada;
            di.Impressa = Conjunto.Impressa;
            return di;
        }
    }
}
