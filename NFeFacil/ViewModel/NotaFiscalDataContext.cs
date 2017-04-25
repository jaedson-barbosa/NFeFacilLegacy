﻿using BibliotecaCentral.IBGE;
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

        public Emitente Emitente { get; set; }
        public Destinatario Destinatario { get; set; }
        public List<DetalhesProdutos> Produtos { get; set; }
        public InformacoesAdicionais InformacoesAdicionais { get; set; }
        public Exportacao Exportacao { get; set; }
        public Compra CompraNota { get; set; }
        public Total Totais { get; set; }
        public Identificacao Ident { get; }
        public Transporte Transp { get; }
        public Cobranca Cobranca { get; }
        public RegistroAquisicaoCana Cana { get; }

        private Popup Log = new Popup();
        private NFe notaSalva;
        private Processo notaEmitida;

        public bool BotaoConfirmarAtivado { get; private set; }
        public bool BotaoSalvarAtivado { get; private set; }
        public bool BotaoAssinarAtivado { get; private set; }
        public bool BotaoTransmitirAtivado { get; private set; }
        public bool BotaoGerarDANFEAtivado { get; private set; }

        private StatusNFe status = StatusNFe.EdiçãoCriação;
        internal StatusNFe StatusAtual
        {
            get { return status; }
            set
            {
                BotaoConfirmarAtivado = value == StatusNFe.EdiçãoCriação;
                BotaoSalvarAtivado = value == StatusNFe.Validado;
                BotaoAssinarAtivado = value == StatusNFe.Salvo;
                BotaoTransmitirAtivado = value == StatusNFe.Assinado;
                BotaoGerarDANFEAtivado = value == StatusNFe.Emitido;
                OnPropertyChanged(nameof(BotaoConfirmarAtivado),
                    nameof(BotaoSalvarAtivado),
                    nameof(BotaoAssinarAtivado),
                    nameof(BotaoTransmitirAtivado),
                    nameof(BotaoGerarDANFEAtivado));
                status = value;
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
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.Documento == Destinatario.Documento);
                }
                return clienteSelecionado;
            }
            set
            {
                Destinatario = clienteSelecionado = value;
                OnPropertyChanged(nameof(Destinatario));
            }
        }

        private Emitente emitenteSelecionado;
        public Emitente EmitenteSelecionado
        {
            get
            {
                if (emitenteSelecionado == null)
                {
                    emitenteSelecionado = EmitentesDisponiveis.FirstOrDefault(x => x.CNPJ == Emitente.CNPJ);
                }
                return emitenteSelecionado;
            }
            set
            {
                Emitente = emitenteSelecionado = value;
                OnPropertyChanged(nameof(Emitente));
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
                    motoristaSelecionado = MotoristasDisponiveis.FirstOrDefault(x => x.Documento == Transp?.transporta?.Documento);
                }
                return motoristaSelecionado;
            }
            set
            {
                Transp.transporta = motoristaSelecionado = value;
                OnPropertyChanged(nameof(Transp));
            }
        }

        public int IndexPivotSelecionado { get; set; } = 0;
        private TipoOperacao OperacaoRequirida { get; }

        internal NotaFiscalDataContext(object param, StatusNFe status, TipoOperacao operacaoRequirida)
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
            OperacaoRequirida = operacaoRequirida;

            Detalhes nfe;
            if (param is Processo proc)
            {
                nfe = proc.NFe.Informações;
                notaEmitida = proc;
            }
            else if (param is NFe nota)
            {
                nfe = nota.Informações;
                notaSalva = nota;
            }
            else
            {
                throw new ArgumentException();
            }

            StatusAtual = status;
            Ident = nfe.identificação;
            Emitente = nfe.emitente;
            Destinatario = nfe.destinatário;
            Produtos = nfe.produtos;
            Transp = nfe.transp ?? new Transporte();
            Cobranca = nfe.cobr ?? new Cobranca();
            InformacoesAdicionais = nfe.infAdic ?? new InformacoesAdicionais();
            Exportacao = nfe.exporta ?? new Exportacao();
            CompraNota = nfe.compra ?? new Compra();
            Cana = nfe.cana ?? new RegistroAquisicaoCana();

            if (CoreApplication.Properties.TryGetValue("ProdutoPendente", out object temp))
            {
                var detalhes = (DetalhesProdutos)temp;
                detalhes.número = Produtos.Count + 1;
                Produtos.Add(detalhes);
                IndexPivotSelecionado = 3;
                CoreApplication.Properties.Remove("ProdutoPendente");
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
            Produtos.Remove(produto);
            OnPropertyChanged(nameof(Produtos));
        }

        public ICommand ConfirmarCommand => new ComandoSimples(Confirmar, true);
        public ICommand SalvarCommand => new ComandoSimples(Salvar, true);
        public ICommand AssinarCommand => new ComandoSimples(Assinar, true);
        public ICommand TransmitirCommand => new ComandoSimples(Transmitir, true);
        public ICommand GerarDANFECommand => new ComandoSimples(GerarDANFE, true);

        private void Confirmar()
        {
            if (notaEmitida != null)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, "Comando não faz neste contexto.");
            }
            else
            {
                if (new ValidarDados(new ValidadorEmitente(Emitente),
                    new ValidadorDestinatario(Destinatario)).ValidarTudo(Log))
                {
                    notaSalva = new NFe
                    {
                        Informações = new Detalhes
                        {
                            identificação = Ident,
                            emitente = Emitente,
                            destinatário = Destinatario,
                            transp = ObterTranporteNormalizado(),
                            produtos = Produtos,
                            total = Totais,
                            cobr = NaoEDefault(Cobranca),
                            infAdic = NaoEDefault(InformacoesAdicionais),
                            exporta = NaoEDefault(Exportacao),
                            compra = NaoEDefault(CompraNota),
                            cana = NaoEDefault(Cana)
                        }
                    };
                    Log.Escrever(TitulosComuns.ValidaçãoConcluída, "A nota fiscal foi validada. Aparentemente, não há irregularidades");
                    StatusAtual = StatusNFe.Validado;
                }
            }
        }

        private Transporte ObterTranporteNormalizado()
        {
            var transp = Transp;
            transp.transporta = NaoEDefault(Transp.transporta);
            transp.veicTransp = NaoEDefault(Transp.veicTransp);
            transp.retTransp = NaoEDefault(Transp.retTransp);
            return transp;
        }

        private static T NaoEDefault<T>(T valor) where T : class => valor != null && valor != default(T) ? valor : null;

        private void Salvar()
        {
            try
            {
                StatusAtual = StatusNFe.Salvo;
                var xml = notaSalva.ToXElement<NFe>();
                var di = NFeDI.Converter(xml);
                di.Status = (int)StatusAtual;
                using (var db = new NotasFiscais())
                {
                    if (OperacaoRequirida == TipoOperacao.Adicao)
                    {
                        db.Adicionar((di, xml));
                    }
                    else
                    {
                        db.Atualizar((di, xml));
                    }
                }
                Log.Escrever(TitulosComuns.Sucesso, "Nota fiscal salva com sucesso. Agora podes sair da aplicação sem perder esta NFe.");
            }
            catch (Exception erro)
            {
                Log.Escrever(TitulosComuns.ErroCatastrófico, erro.Message);
                StatusAtual = StatusNFe.EdiçãoCriação;
            }
        }

        private void Assinar()
        {
            Log.Escrever(TitulosComuns.ErroCatastrófico, "Função ainda não implementada.");
            StatusAtual = StatusNFe.Assinado;
        }

        private void Transmitir()
        {
            Log.Escrever(TitulosComuns.ErroCatastrófico, "Função ainda não implementada.");
            StatusAtual = StatusNFe.Emitido;
        }

        private async void GerarDANFE()
        {
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(ViewDANFE), notaEmitida);
            StatusAtual = StatusNFe.Impresso;
        }

        #region Identificação

        public DateTimeOffset DataEmissao
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraEmissão))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraEmissão = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraEmissão);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraEmissao
        {
            get => DataEmissao.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraEmissão);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraEmissão = novo.ToStringPersonalizado();
            }
        }

        public DateTimeOffset DataSaidaEntrada
        {
            get
            {
                if (string.IsNullOrEmpty(Ident.DataHoraSaídaEntrada))
                {
                    var agora = DateTime.Now;
                    Ident.DataHoraSaídaEntrada = agora.ToStringPersonalizado();
                    return agora;
                }
                return DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
            }
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(value.Year, value.Month, value.Day, anterior.Hour, anterior.Minute, anterior.Second);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public TimeSpan HoraSaidaEntrada
        {
            get => DataSaidaEntrada.TimeOfDay;
            set
            {
                var anterior = DateTimeOffset.Parse(Ident.DataHoraSaídaEntrada);
                var novo = new DateTime(anterior.Year, anterior.Month, anterior.Day, value.Hours, value.Minutes, value.Seconds);
                Ident.DataHoraSaídaEntrada = novo.ToStringPersonalizado();
            }
        }

        public ushort EstadoIdentificacao
        {
            get => Ident.CódigoUF;
            set
            {
                Ident.CódigoUF = value;
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
                if (!string.IsNullOrEmpty(Transp.retTransp.cMunFG) && ufEscolhida == null)
                {
                    foreach (var item in Estados.EstadosCache)
                    {
                        var lista = Municipios.Get(item);
                        if (lista.Count(x => x.Codigo == int.Parse(Transp.retTransp.cMunFG)) > 0)
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
            get => (ModalidadesTransporte)Transp.modFrete;
            set => Transp.modFrete = (int)value;
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
                Transp.reboque.Add(x.DataContext as Reboque);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
            };
            await add.ShowAsync();
        }

        private void RemoverReboque(Reboque reboque)
        {
            Transp.reboque.Remove(reboque);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
        }

        private async void AdicionarVolume()
        {
            var add = new View.CaixasDialogo.AdicionarVolume();
            add.PrimaryButtonClick += (x, y) =>
            {
                Transp.vol.Add(x.DataContext as Volume);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
            };
            await add.ShowAsync();
        }

        private void RemoverVolume(Volume volume)
        {
            Transp.vol.Remove(volume);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Transp)));
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
                Cobranca.Dup.Add(sender.DataContext as Duplicata);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cobranca"));
            };
            await caixa.ShowAsync();
        }

        private void RemoverDuplicata(Duplicata duplicata)
        {
            Cobranca.Dup.Remove(duplicata);
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
                Cana.forDia.Add(x.DataContext as FornecimentoDiario);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverFornecimento(FornecimentoDiario fornecimento)
        {
            Cana.forDia.Remove(fornecimento);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        public async void AdicionarDeducao()
        {
            var caixa = new View.CaixasDialogo.AdicionarDeducao();
            caixa.PrimaryButtonClick += (x, y) =>
            {
                Cana.deduc.Add(x.DataContext as Deducoes);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
            };
            await caixa.ShowAsync();
        }

        public void RemoverDeducao(Deducoes deducao)
        {
            Cana.deduc.Remove(deducao);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Cana)));
        }

        #endregion
    }
}
