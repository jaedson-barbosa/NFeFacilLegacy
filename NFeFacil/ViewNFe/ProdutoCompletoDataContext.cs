using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ViewModel;
using NFeFacil.ViewModel.ImpostosProduto;
using NFeFacil.ViewNFe.CaixasImpostos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewNFe
{
    public sealed class ProdutoCompletoDataContext : INotifyPropertyChanged
    {
        public DetalhesProdutos ProdutoCompleto { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] parametros)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(parametros[i]));
            }
        }

        public ObservableCollection<DeclaracaoImportacao> ListaDI => ProdutoCompleto.Produto.DI.GerarObs();
        public ObservableCollection<GrupoExportacao> ListaGE => ProdutoCompleto.Produto.GrupoExportação.GerarObs();

        public bool AtivadopvtICMS { get; private set; } = true;
        public bool AtivadopvtISSQN { get; private set; } = false;
        public bool AtivadopvtICMSInterestadual { get; private set; } = true;

        private bool tipoICMSSelecionado = true;
        public bool TipoICMSSelecionado
        {
            get => tipoICMSSelecionado;
            set
            {
                tipoICMSSelecionado = value;
                AtivadopvtICMS = AtivadopvtICMSInterestadual = value;
                AtivadopvtISSQN = !value;
                OnPropertyChanged(nameof(AtivadopvtICMS), nameof(AtivadopvtICMSInterestadual), nameof(AtivadopvtISSQN));
            }
        }

        public VeiculoNovo Veiculo
        {
            get { return ProdutoCompleto.Produto.veicProd ?? (ProdutoCompleto.Produto.veicProd = new VeiculoNovo()); }
        }

        public Combustivel Comb
        {
            get { return ProdutoCompleto.Produto.comb ?? (ProdutoCompleto.Produto.comb = new Combustivel()); }
        }

        public bool UsarCIDE
        {
            get => Comb.CIDE != null;
            set
            {
                Comb.CIDE = value ? new CIDE() : null;
                OnPropertyChanged(nameof(UsarCIDE), nameof(Comb));
            }
        }

        public ObservableCollection<TiposEspeciaisProduto> ListaTiposEspeciaisProduto => ExtensoesPrincipal.ObterItens<TiposEspeciaisProduto>();
        private TiposEspeciaisProduto tipoEspecialProdutoSelecionado;
        public TiposEspeciaisProduto TipoEspecialProdutoSelecionado
        {
            get { return tipoEspecialProdutoSelecionado; }
            set
            {
                tipoEspecialProdutoSelecionado = value;
                VisibilidadeVeiculoNovo = TiposEspeciaisProduto.Veículo == value;
                VisibilidadeMedicamento = TiposEspeciaisProduto.Medicamento == value;
                VisibilidadeArmamento = TiposEspeciaisProduto.Armamento == value;
                VisibilidadeCombustivel = TiposEspeciaisProduto.Combustível == value;
                VisibilidadePapel = TiposEspeciaisProduto.Papel == value;
                OnPropertyChanged(nameof(VisibilidadeVeiculoNovo),
                    nameof(VisibilidadeMedicamento),
                    nameof(VisibilidadeArmamento),
                    nameof(VisibilidadeCombustivel),
                    nameof(VisibilidadePapel));
                if (value == TiposEspeciaisProduto.Veículo)
                {
                    ProdutoCompleto.Produto.veicProd = new VeiculoNovo();
                    ProdutoCompleto.Produto.comb = null;
                }
                else if (value == TiposEspeciaisProduto.Combustível)
                {
                    ProdutoCompleto.Produto.veicProd = null;
                    ProdutoCompleto.Produto.comb = new Combustivel();
                }
                ProdutoCompleto.Produto.medicamentos = new List<Medicamento>();
                ProdutoCompleto.Produto.armas = new List<Arma>();
                ProdutoCompleto.Produto.NRECOPI = null;
                OnPropertyChanged("Produto.veicProd", "Produto.comb", "Produto.nRECOPI", "ListaMedicamentos", "ListaArmamento");
            }
        }

        public bool VisibilidadeVeiculoNovo { get; private set; }
        public bool VisibilidadeMedicamento { get; private set; }
        public bool VisibilidadeArmamento { get; private set; }
        public bool VisibilidadeCombustivel { get; private set; }
        public bool VisibilidadePapel { get; private set; }

        public ObservableCollection<Medicamento> ListaMedicamentos
        {
            get { return ProdutoCompleto.Produto.medicamentos.GerarObs(); }
        }

        public ObservableCollection<Arma> ListaArmamento
        {
            get { return ProdutoCompleto.Produto.armas.GerarObs(); }
        }

        ICMSDataContext contextoICMS;
        public ICMSDataContext ContextoICMS
        {
            get
            {
                if (contextoICMS == null)
                {
                    contextoICMS = new ICMSDataContext();
                }
                return contextoICMS;
            }
        }

        PISDataContext contextoPIS;
        public PISDataContext ContextoPIS
        {
            get
            {
                if (contextoPIS == null)
                {
                    contextoPIS = new PISDataContext();
                }
                contextoPIS.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoPIS;
            }
        }

        COFINSDataContext contextoCOFINS;
        public COFINSDataContext ContextoCOFINS
        {
            get
            {
                if (contextoCOFINS == null)
                {
                    contextoCOFINS = new COFINSDataContext();
                }
                contextoCOFINS.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoCOFINS;
            }
        }

        IPIDataContext contextoIPI;
        public IPIDataContext ContextoIPI
        {
            get
            {
                if (contextoIPI == null)
                {
                    contextoIPI = new IPIDataContext();
                }
                contextoIPI.ProdutoReferente = ProdutoCompleto.Produto;
                return contextoIPI;
            }
        }

        ISSQNDataContext contextoISSQN;
        public ISSQNDataContext ContextoISSQN
        {
            get
            {
                if (contextoISSQN == null)
                {
                    contextoISSQN = new ISSQNDataContext();
                }
                return contextoISSQN;
            }
        }

        II contextoII;
        public II ContextoII
        {
            get
            {
                if (contextoII == null)
                {
                    contextoII = new II();
                }
                return contextoII;
            }
        }

        ImpostoDevol contextoImpostoDevol;
        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (contextoImpostoDevol == null)
                {
                    contextoImpostoDevol = new ImpostoDevol();
                }
                return contextoImpostoDevol;
            }
        }

        ICMSUFDest contextoIcmsUFDest;
        public ICMSUFDest ContextoIcmsUFDest
        {
            get
            {
                if (contextoIcmsUFDest == null)
                {
                    contextoIcmsUFDest = new ICMSUFDest();
                }
                return contextoIcmsUFDest;
            }
        }

        public ProdutoCompletoDataContext(DetalhesProdutos produtoCompleto)
        {
            ProdutoCompleto = produtoCompleto;
            ProdutoCompleto.Produto.DadoImpostoChanged += (x, y) =>
            {
                OnPropertyChanged(nameof(ContextoPIS), nameof(ContextoCOFINS), nameof(ContextoIPI));
            };
            var imps = produtoCompleto.Impostos.impostos;
            ConjuntoPIS conjPis = new ConjuntoPIS();
            ConjuntoCOFINS conjCofins = new ConjuntoCOFINS();
            for (int i = 0; i < imps.Count; i++)
            {
                var imp = imps[i];
                if (imp is ICMS icms)
                {
                    contextoICMS = new ICMSDataContext(icms);
                }
                else if (imp is PIS pis)
                {
                    conjPis.PIS = pis;
                }
                else if (imp is PISST pisst)
                {
                    conjPis.PISST = pisst;
                }
                else if (imp is COFINS cofins)
                {
                    conjCofins.COFINS = cofins;
                }
                else if (imp is COFINSST cofinsst)
                {
                    conjCofins.COFINSST = cofinsst;
                }
                else if (imp is IPI ipi)
                {
                    contextoIPI = new IPIDataContext(ipi, ProdutoCompleto.Produto);
                }
                else if (imp is ISSQN issqn)
                {
                    contextoISSQN = new ISSQNDataContext(issqn);
                }
                else if (imp is II ii)
                {
                    contextoII = ii;
                }
                else if (imp is ImpostoDevol devol)
                {
                    contextoImpostoDevol = devol;
                }
                else if (imp is ICMSUFDest dest)
                {
                    contextoIcmsUFDest = dest;
                }
                else
                {
                    throw new InvalidOperationException("O formato do imposto não é reconhecido.");
                }
            }
            contextoPIS = new PISDataContext(conjPis, ProdutoCompleto.Produto);
            contextoCOFINS = new COFINSDataContext(conjCofins, ProdutoCompleto.Produto);

            AdicionarDeclaracaoImportacaoCommand = new Comando(AdicionarDeclaracaoImportacao);
            AdicionarDeclaracaoExportacaoCommand = new Comando(AdicionarDeclaracaoExportacao);
            RemoverDeclaracaoImportacaoCommand = new Comando<DeclaracaoImportacao>(RemoverDeclaracaoImportacao);
            RemoverDeclaracaoExportacaoCommand = new Comando<GrupoExportacao>(RemoverDeclaracaoExportacao);
            AdicionarMedicamentoCommand = new Comando(AdicionarMedicamento);
            RemoverMedicamentoCommand = new Comando<Medicamento>(RemoverMedicamento);
            AdicionarArmamentoCommand = new Comando(AdicionarArmamento);
            RemoverArmamentoCommand = new Comando<Arma>(RemoverArmamento);
        }

        public ICommand AdicionarDeclaracaoImportacaoCommand { get; }
        public ICommand AdicionarDeclaracaoExportacaoCommand { get; }
        public ICommand RemoverDeclaracaoImportacaoCommand { get; }
        public ICommand RemoverDeclaracaoExportacaoCommand { get; }
        public ICommand AdicionarMedicamentoCommand { get; }
        public ICommand RemoverMedicamentoCommand { get; }
        public ICommand AdicionarArmamentoCommand { get; }
        public ICommand RemoverArmamentoCommand { get; }

        private async void AdicionarDeclaracaoImportacao()
        {
            var caixa = new CaixasDialogoProduto.AdicionarDeclaracaoImportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ProdutoCompleto.Produto.DI.Add(caixa.Declaracao);
                OnPropertyChanged(nameof(ListaDI));
            }
        }

        private async void AdicionarDeclaracaoExportacao()
        {
            var caixa = new CaixasDialogoProduto.EscolherTipoDeclaracaoExportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var tipo = caixa.TipoEscolhido;
                if (tipo == CaixasDialogoProduto.TiposDeclaracaoExportacao.Direta)
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoDireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ProdutoCompleto.Produto.GrupoExportação.Add(caixa2.Declaracao);
                    }
                }
                else
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoIndireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ProdutoCompleto.Produto.GrupoExportação.Add(caixa2.Declaracao);
                    }
                }
                OnPropertyChanged(nameof(ListaGE));
            }
        }

        private void RemoverDeclaracaoImportacao(DeclaracaoImportacao declaracao)
        {
            ProdutoCompleto.Produto.DI.Remove(declaracao);
            OnPropertyChanged(nameof(ListaDI));
        }

        private void RemoverDeclaracaoExportacao(GrupoExportacao declaracao)
        {
            ProdutoCompleto.Produto.GrupoExportação.Remove(declaracao);
            OnPropertyChanged(nameof(ListaGE));
        }

        async void AdicionarMedicamento()
        {
            var caixa = new CaixasDialogoProduto.AdicionarMedicamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoMedicamento = (Medicamento)caixa.DataContext;
                ProdutoCompleto.Produto.medicamentos.Add(novoMedicamento);
                OnPropertyChanged(nameof(ListaMedicamentos));
            }
        }

        private void RemoverMedicamento(Medicamento med)
        {
            ProdutoCompleto.Produto.medicamentos.Remove(med);
            OnPropertyChanged(nameof(ListaMedicamentos));
        }

        async void AdicionarArmamento()
        {
            var caixa = new CaixasDialogoProduto.AdicionarArmamento();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var novoArmamento = (Arma)caixa.DataContext;
                ProdutoCompleto.Produto.armas.Add(novoArmamento);
                OnPropertyChanged(nameof(ListaArmamento));
            }
        }

        public void RemoverArmamento(Arma arma)
        {
            ProdutoCompleto.Produto.armas.Remove(arma);
            OnPropertyChanged(nameof(ListaArmamento));
        }

        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        async void AdicionarImposto()
        {

        }

        async Task<ComumICMS> AdicionarICMS()
        {
            var caixa = new EscolherTipoICMS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                if (caixa.Regime == EscolherTipoICMS.Regimes.Normal)
                {
                    var caixa2 = new AdicionarICMSRN(caixa.TipoICMSRN);
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var cst = caixa.TipoICMSRN.ToString("00");
                        var origem = caixa.Origem;
                        switch (caixa.TipoICMSRN)
                        {
                            case 0:
                                return new ICMS00()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    vBC = caixa2.vBC,
                                    vICMS = caixa2.vICMS
                                };
                            case 10:
                                return new ICMS10()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vICMS = caixa2.vICMS,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 1010:
                                return new ICMSPart()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBC = caixa2.pRedBC,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vICMS = caixa2.vICMS,
                                    vICMSST = caixa2.vICMSST,
                                    pBCOp = caixa2.pBCOp,
                                    UFST = caixa2.UFST
                                };
                            case 20:
                                return new ICMS20()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    vBC = caixa2.vBC,
                                    vICMS = caixa2.vICMS,
                                    vICMSDeson = caixa2.vICMSDeson,
                                    pRedBC = caixa2.pRedBC
                                };
                            case 30:
                                return new ICMS30()
                                {
                                    CST = cst,
                                    modBCST = caixa2.modBCST.ToString(),
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBCST = caixa2.vBCST,
                                    vICMSDeson = caixa2.vICMSDeson,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 40:
                                return new ICMS40()
                                {
                                    CST = cst,
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    vICMSDeson = caixa2.vICMSDeson
                                };
                            case 41:
                                return new ICMS41()
                                {
                                    CST = cst,
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    vICMSDeson = caixa2.vICMSDeson
                                };
                            case 4141:
                                return new ICMSST()
                                {
                                    CST = cst,
                                    Orig = origem,
                                    vBCSTDest = caixa2.vBCSTDest,
                                    vBCSTRet = caixa2.vBCSTRet,
                                    vICMSSTDest = caixa2.vICMSSTDest,
                                    vICMSSTRet = caixa2.vICMSSTRet
                                };
                            case 50:
                                return new ICMS50()
                                {
                                    CST = cst,
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    vICMSDeson = caixa2.vICMSDeson
                                };
                            case 51:
                                return new ICMS51()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pRedBC = caixa2.pRedBC,
                                    vBC = caixa2.vBC,
                                    vICMS = caixa2.vICMS,
                                    pDif = caixa2.pDif,
                                    vICMSDif = caixa2.vICMSDif,
                                    vICMSOp = caixa2.vICMSOp
                                };
                            case 60:
                                return new ICMS60()
                                {
                                    CST = cst,
                                    Orig = origem,
                                    vBCSTRet = caixa2.vBCSTRet,
                                    vICMSSTRet = caixa2.vICMSSTRet
                                };
                            case 70:
                                return new ICMS70()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBC = caixa2.pRedBC,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vICMS = caixa2.vICMS,
                                    vICMSDeson = caixa2.vICMSDeson,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 90:
                                return new ICMS90()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    motDesICMS = caixa2.motDesICMS,
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBC = caixa2.pRedBC,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vICMS = caixa2.vICMS,
                                    vICMSDeson = caixa2.vICMSDeson,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 9090:
                                return new ICMSPart()
                                {
                                    CST = cst,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBC = caixa2.pRedBC,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vICMS = caixa2.vICMS,
                                    vICMSST = caixa2.vICMSST,
                                    pBCOp = caixa2.pBCOp,
                                    UFST = caixa2.UFST
                                };
                        }
                    }
                }
                else
                {
                    var caixa2 = new AdicionarICMSSN(caixa.TipoICMSSN);
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var csosn = caixa.TipoICMSSN.ToString("000");
                        var origem = caixa.Origem;
                        switch (caixa.TipoICMSSN)
                        {
                            case 101:
                                return new ICMSSN101()
                                {
                                    CSOSN = csosn,
                                    Orig = origem,
                                    pCredSN = caixa2.pCredSN,
                                    vCredICMSSN = caixa2.vCredICMSSN
                                };
                            case 102:
                                return new ICMSSN102()
                                {
                                    CSOSN = csosn,
                                    Orig = origem
                                };
                            case 103:
                                return new ICMSSN102()
                                {
                                    CSOSN = csosn,
                                    Orig = origem
                                };
                            case 201:
                                return new ICMSSN201()
                                {
                                    CSOSN = csosn,
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pCredSN = caixa2.pCredSN,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBCST = caixa2.vBCST,
                                    vCredICMSSN = caixa2.vCredICMSSN,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 202:
                                return new ICMSSN202()
                                {
                                    CSOSN = csosn,
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBCST = caixa2.vBCST,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 203:
                                return new ICMSSN202()
                                {
                                    CSOSN = csosn,
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBCST = caixa2.vBCST,
                                    vICMSST = caixa2.vICMSST
                                };
                            case 300:
                                return new ICMSSN102()
                                {
                                    CSOSN = csosn,
                                    Orig = origem
                                };
                            case 400:
                                return new ICMSSN102()
                                {
                                    CSOSN = csosn,
                                    Orig = origem
                                };
                            case 500:
                                return new ICMSSN500()
                                {
                                    CSOSN = csosn,
                                    Orig = origem
                                };
                            case 900:
                                return new ICMSSN900()
                                {
                                    CSOSN = csosn,
                                    modBC = caixa2.modBC.ToString(),
                                    modBCST = caixa2.modBCST.ToString(),
                                    Orig = origem,
                                    pCredSN = caixa2.pCredSN,
                                    pICMS = caixa2.pICMS,
                                    pICMSST = caixa2.pICMSST,
                                    pMVAST = caixa2.pMVAST,
                                    pRedBC = caixa2.pRedBC,
                                    pRedBCST = caixa2.pRedBCST,
                                    vBC = caixa2.vBC,
                                    vBCST = caixa2.vBCST,
                                    vCredICMSSN = caixa2.vCredICMSSN,
                                    vICMS = caixa2.vICMS,
                                    vICMSST = caixa2.vICMSST
                                };
                        }
                    }
                }
            }
            return null;
        }

        async Task<IPI> AdicionarIPI()
        {
            var caixa = new EscolherTipoIPI();
            IPI retorno = null;
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                if (caixa.TipoCalculo == TiposCalculo.Inexistente)
                {
                    var caixa2 = new AdicionarIPISimples();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        retorno = caixa2.Conjunto;
                    }
                }
                else if (caixa.TipoCalculo == TiposCalculo.PorAliquota)
                {
                    var caixa2 = new AdicionarIPIAliquota();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var vBC = ProdutoCompleto.Produto.ValorTotal;
                        var pIPI = double.Parse(caixa2.Aliquota, culturaPadrao);
                        caixa2.Conjunto.Corpo = new IPITrib
                        {
                            vBC = vBC.ToString("F2", culturaPadrao),
                            pIPI = pIPI.ToString("F4", culturaPadrao),
                            vIPI = (vBC * pIPI / 100).ToString("F2", culturaPadrao)
                        };
                        retorno = caixa2.Conjunto;
                    }
                }
                else
                {
                    var caixa2 = new AdicionarIPIValor();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var qUnid = ProdutoCompleto.Produto.QuantidadeComercializada;
                        var vUnid = double.Parse(caixa2.ValorUnitario, culturaPadrao);
                        caixa2.Conjunto.Corpo = new IPITrib
                        {
                            qUnid = qUnid.ToString("F4", culturaPadrao),
                            vUnid = vUnid.ToString("F4", culturaPadrao),
                            vIPI = (qUnid * vUnid).ToString("F2", culturaPadrao)
                        };
                        retorno = caixa2.Conjunto;
                    }
                }
            }

            if (retorno != null)
            {
                retorno.Corpo.CST = caixa.CST;
            }
            return retorno;
        }

        async Task<Imposto[]> AdicionarPIS()
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var cst = caixa.CST;
                var valor = int.Parse(cst);

                if (valor == 1 || valor == 2)
                {
                    var caixa2 = new AddPISouCOFINSAliquota();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var vBC = ProdutoCompleto.Produto.ValorTotal;
                        var pPIS = caixa2.Aliquota;
                        return new PIS[1]
                        {
                            new PIS
                            {
                                Corpo = new PISAliq
                                {
                                    CST = cst,
                                    vBC = vBC.ToString("F2", culturaPadrao),
                                    pPIS = pPIS.ToString("F4", culturaPadrao),
                                    vPIS = (vBC * pPIS / 100).ToString("F2", culturaPadrao)
                                }
                            }
                        };
                    }
                }
                else if (valor == 3)
                {
                    var caixa2 = new AddPISouCOFINSValor();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                        var vAliqProd = caixa2.Valor;
                        return new PIS[1]
                        {
                            new PIS
                            {
                                Corpo = new PISQtde
                                {
                                    CST = cst,
                                    qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                    vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                    vPIS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                }
                            }
                        };
                    }
                }
                else if (valor >= 4 && valor <= 9)
                {
                    if (valor == 5)
                    {
                        if (caixa.TipoCalculoST == TiposCalculo.PorAliquota)
                        {
                            var caixa2 = new AddPISouCOFINSAliquota();
                            if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                            {
                                var vBC = ProdutoCompleto.Produto.ValorTotal;
                                var pPIS = caixa2.Aliquota;
                                return new Imposto[2]
                                {
                                    new PIS
                                    {
                                        Corpo = new PISNT()
                                        {
                                            CST = cst
                                        }
                                    },
                                    new PISST
                                    {
                                        vBC = vBC.ToString("F2", culturaPadrao),
                                        pPIS = pPIS.ToString("F4", culturaPadrao),
                                        vPIS = (vBC * pPIS / 100).ToString("F2", culturaPadrao)
                                    }
                                };
                            }
                        }
                        else
                        {
                            var caixa2 = new AddPISouCOFINSValor();
                            if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                            {
                                var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                                var vAliqProd = caixa2.Valor;
                                return new Imposto[2]
                                {
                                    new PIS
                                    {
                                        Corpo = new PISNT()
                                        {
                                            CST = cst
                                        }
                                    },
                                    new PISST
                                    {
                                        qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                        vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                        vPIS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                    }
                                };
                            }
                        }
                    }
                    else
                    {
                        return new PIS[1]
                        {
                            new PIS
                            {
                                Corpo = new PISNT()
                                {
                                    CST = cst
                                }
                            }
                        };
                    }
                }
                else
                {
                    if (caixa.TipoCalculo == TiposCalculo.PorAliquota)
                    {
                        var caixa2 = new AddPISouCOFINSAliquota();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            var vBC = ProdutoCompleto.Produto.ValorTotal;
                            var pPIS = caixa2.Aliquota;
                            return new PIS[1]
                            {
                                new PIS
                                {
                                    Corpo = new PISOutr
                                    {
                                        CST = cst,
                                        vBC = vBC.ToString("F2", culturaPadrao),
                                        pPIS = pPIS.ToString("F4", culturaPadrao),
                                        vPIS = (vBC * pPIS / 100).ToString("F2", culturaPadrao)
                                    }
                                }
                            };
                        }
                    }
                    else
                    {
                        var caixa2 = new AddPISouCOFINSValor();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                            var vAliqProd = caixa2.Valor;
                            return new PIS[1]
                            {
                                new PIS
                                {
                                    Corpo = new PISOutr
                                    {
                                        CST = cst,
                                        qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                        vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                        vPIS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                    }
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        async Task<Imposto[]> AdicionarCOFINS()
        {
            var caixa = new EscolherTipoPISouCOFINS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var cst = caixa.CST;
                var valor = int.Parse(cst);

                if (valor == 1 || valor == 2)
                {
                    var caixa2 = new AddPISouCOFINSAliquota();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var vBC = ProdutoCompleto.Produto.ValorTotal;
                        var pCOFINS = caixa2.Aliquota;
                        return new COFINS[1]
                        {
                            new COFINS
                            {
                                Corpo = new COFINSAliq
                                {
                                    CST = cst,
                                    vBC = vBC.ToString("F2", culturaPadrao),
                                    pCOFINS = pCOFINS.ToString("F4", culturaPadrao),
                                    vCOFINS = (vBC * pCOFINS / 100).ToString("F2", culturaPadrao)
                                }
                            }
                        };
                    }
                }
                else if (valor == 3)
                {
                    var caixa2 = new AddPISouCOFINSValor();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                        var vAliqProd = caixa2.Valor;
                        return new COFINS[1]
                        {
                            new COFINS
                            {
                                Corpo = new COFINSQtde
                                {
                                    CST = cst,
                                    qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                    vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                    vCOFINS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                }
                            }
                        };
                    }
                }
                else if (valor >= 4 && valor <= 9)
                {
                    if (valor == 5)
                    {
                        if (caixa.TipoCalculoST == TiposCalculo.PorAliquota)
                        {
                            var caixa2 = new AddPISouCOFINSAliquota();
                            if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                            {
                                var vBC = ProdutoCompleto.Produto.ValorTotal;
                                var pCOFINS = caixa2.Aliquota;
                                return new Imposto[2]
                                {
                                    new COFINS
                                    {
                                        Corpo = new COFINSNT()
                                        {
                                            CST = cst
                                        }
                                    },
                                    new COFINSST
                                    {
                                        vBC = vBC.ToString("F2", culturaPadrao),
                                        pCOFINS = pCOFINS.ToString("F4", culturaPadrao),
                                        vCOFINS = (vBC * pCOFINS / 100).ToString("F2", culturaPadrao)
                                    }
                                };
                            }
                        }
                        else
                        {
                            var caixa2 = new AddPISouCOFINSValor();
                            if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                            {
                                var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                                var vAliqProd = caixa2.Valor;
                                return new Imposto[2]
                                {
                                    new COFINS
                                    {
                                        Corpo = new COFINSNT()
                                        {
                                            CST = cst
                                        }
                                    },
                                    new COFINSST
                                    {
                                        qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                        vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                        vCOFINS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                    }
                                };
                            }
                        }
                    }
                    else
                    {
                        return new COFINS[1]
                        {
                            new COFINS
                            {
                                Corpo = new COFINSNT()
                                {
                                    CST = cst
                                }
                            }
                        };
                    }
                }
                else
                {
                    if (caixa.TipoCalculo == TiposCalculo.PorAliquota)
                    {
                        var caixa2 = new AddPISouCOFINSAliquota();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            var vBC = ProdutoCompleto.Produto.ValorTotal;
                            var pCOFINS = caixa2.Aliquota;
                            return new COFINS[1]
                            {
                                new COFINS
                                {
                                    Corpo = new COFINSOutr
                                    {
                                        CST = cst,
                                        vBC = vBC.ToString("F2", culturaPadrao),
                                        pCOFINS = pCOFINS.ToString("F4", culturaPadrao),
                                        vCOFINS = (vBC * pCOFINS / 100).ToString("F2", culturaPadrao)
                                    }
                                }
                            };
                        }
                    }
                    else
                    {
                        var caixa2 = new AddPISouCOFINSValor();
                        if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                        {
                            var qBCProd = ProdutoCompleto.Produto.QuantidadeComercializada;
                            var vAliqProd = caixa2.Valor;
                            return new COFINS[1]
                            {
                                new COFINS
                                {
                                    Corpo = new COFINSOutr
                                    {
                                        CST = cst,
                                        qBCProd = qBCProd.ToString("F4", culturaPadrao),
                                        vAliqProd = vAliqProd.ToString("F4", culturaPadrao),
                                        vCOFINS = (qBCProd * vAliqProd).ToString("F2", culturaPadrao)
                                    }
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        async Task<ISSQN> AdicionarISSQN()
        {
            var caixa = new MessageDialog("Qual o tipo de ISSQN desejado?", "Entrada");
            caixa.Commands.Add(new UICommand("Nacional"));
            caixa.Commands.Add(new UICommand("Exterior"));
            if ((await caixa.ShowAsync()).Label == "Exterior")
            {
                var caixa2 = new AdicionarISSQNExterior();
                if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                {
                    return caixa2.Imposto;
                }
            }
            else
            {
                var caixa2 = new AdicionarISSQNNacional();
                if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                {
                    return caixa2.Imposto;
                }
            }
            return null;
        }

        async Task<II> AidicionarII()
        {
            var caixa = new AdicionarII();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                return caixa.Imposto;
            }
            return null;
        }

        async Task<ImpostoDevol> AdicionarImpostoDevolvido()
        {
            var caixa = new AdicionarIPIDevolvido();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                return caixa.Imposto;
            }
            return null;
        }

        public enum TiposEspeciaisProduto
        {
            Simples,
            Veículo,
            Medicamento,
            Armamento,
            Combustível,
            Papel
        }
    }
}
