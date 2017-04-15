using Microsoft.EntityFrameworkCore;
using NFeFacil.IBGE;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.ModeloXML;
using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class NotaFiscalDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        private Popup Log = new Popup();
        private NFe notaSalva;
        private Processo notaEmitida;

        public bool BotaoConfirmarAtivado { get; private set; }
        public bool BotaoSalvarAtivado { get; private set; }
        public bool BotaoAssinarAtivado { get; private set; }
        public bool BotaoTransmitirAtivado { get; private set; }
        public bool BotaoGerarDANFEAtivado { get; private set; }

        private StatusNFe status = StatusNFe.EdiçãoCriação;
        private StatusNFe StatusAtual
        {
            get { return status; }
            set
            {
                bool[] botoes =
                {
                    BotaoConfirmarAtivado,
                    BotaoSalvarAtivado,
                    BotaoAssinarAtivado,
                    BotaoTransmitirAtivado,
                    BotaoGerarDANFEAtivado
                };
                for (int i = 0; i < botoes.Length; i++)
                {
                    var valor = (int)value;
                    botoes[i] = i == (valor >= botoes.Length ? botoes.Length - 1 : valor);
                }
                status = value;
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
                if (clienteSelecionado == null)
                {
                    clienteSelecionado = ClientesDisponiveis.FirstOrDefault(x => x.obterDocumento == Destinatario.obterDocumento);
                }
                return clienteSelecionado;
            }
            set
            {
                Destinatario = clienteSelecionado = value;
                OnPropertyChanged(nameof(Destinatario));
            }
        }

        private EmitenteDI emitenteSelecionado;
        public EmitenteDI EmitenteSelecionado
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

        private MotoristaDI motoristaSelecionado;
        public MotoristaDI MotoristaSelecionado
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

        internal NotaFiscalDataContext(NotaComDados param)
        {
            using (var db = new AplicativoContext())
            {
                ClientesDisponiveis = db.Clientes
                    .Include(x => x.endereco)
                    .ToList();
                EmitentesDisponiveis = db.Emitentes
                    .Include(x => x.endereco)
                    .ToList();
                ProdutosDisponiveis = db.Produtos
                    .ToList();
                MotoristasDisponiveis = db.Motoristas
                    .ToList();
            }

            switch (param.tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Add, "Criar nota fiscal");
                    break;
                case TipoOperacao.Edicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.ManipularNota, Symbol.Edit, "Editar nota fiscal");
                    break;
            }
            Detalhes nfe;
            if (param.proc?.NFe != null)
            {
                nfe = param.proc.NFe.Informações;
                notaEmitida = param.proc;
            }
            else
            {
                nfe = param.nota.Informações;
                notaSalva = param.nota;
            }
            StatusAtual = (StatusNFe)param.dados.Status;
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

        #region Propriedades de contexto
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

        #endregion

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

        public ICommand AdicionarReboqueCommand => new ComandoSemParametros(AdicionarReboque, true);
        public ICommand RemoverReboqueCommand => new ComandoComParametros<Reboque, ObterDataContext<Reboque>>(RemoverReboque);
        public ICommand AdicionarVolumeCommand => new ComandoSemParametros(AdicionarVolume, true);
        public ICommand RemoverVolumeCommand => new ComandoComParametros<Volume, ObterDataContext<Volume>>(RemoverVolume);

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

        public ICommand AdicionarDuplicataCommand => new ComandoSemParametros(AdicionarDuplicata, true);
        public ICommand RemoverDuplicataCommand => new ComandoComParametros<Duplicata, ObterDataContext<Duplicata>>(RemoverDuplicata);

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

        public ICommand AdicionarFornecimentoCommand => new ComandoSemParametros(AdicionarFornecimento, true);
        public ICommand RemoverFornecimentoCommand => new ComandoComParametros<FornecimentoDiario, ObterDataContext<FornecimentoDiario>>(RemoverFornecimento);
        public ICommand AdicionarDeducaoCommand => new ComandoSemParametros(AdicionarDeducao, true);
        public ICommand RemoverDeducaoCommand => new ComandoComParametros<Deducoes, ObterDataContext<Deducoes>>(RemoverDeducao);

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
