using NFeFacil.ModeloXML.PartesProcesso;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewNFe.CaixasImpostos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ImpostosProduto : Page, IValida
    {
        public ImpostosProduto()
        {
            InitializeComponent();
        }

        string ValorTotalTributos { get; set; }
        ObservableCollection<VisualizacaoImposto> ImpostosAdicionados { get; set; } = new ObservableCollection<VisualizacaoImposto>();
        List<PrincipaisImpostos> ImpostosAdicionaveis;
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;
        DetalhesProdutos ProdutoCompleto { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProdutoCompleto = (DetalhesProdutos)e.Parameter;
            DefinirImpostosAdicionaveis();
        }

        async void DefinirImpostosAdicionaveis()
        {
            var caixa = new MessageDialog("Qual o tipo de imposto que é usado neste dado?", "Entrada");
            caixa.Commands.Add(new UICommand("ICMS"));
            caixa.Commands.Add(new UICommand("ISSQN"));
            if ((await caixa.ShowAsync()).Label == "ICMS")
            {
                ImpostosAdicionaveis = new List<PrincipaisImpostos>
                {
                    PrincipaisImpostos.ICMS,
                    PrincipaisImpostos.IPI,
                    PrincipaisImpostos.II,
                    PrincipaisImpostos.PIS,
                    PrincipaisImpostos.COFINS,
                    PrincipaisImpostos.ICMSUFDest
                };
            }
            else
            {
                ImpostosAdicionaveis = new List<PrincipaisImpostos>
                {
                    PrincipaisImpostos.IPI,
                    PrincipaisImpostos.ISSQN,
                    PrincipaisImpostos.PIS,
                    PrincipaisImpostos.COFINS,
                    PrincipaisImpostos.ICMSUFDest
                };
            }
        }

        async void AdicionarImposto(object sender, RoutedEventArgs e)
        {
            var caixa = new EscolhaImposto(ImpostosAdicionaveis);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                List<VisualizacaoImposto> adicionar = new List<VisualizacaoImposto>();
                switch (caixa.Escolhido)
                {
                    case PrincipaisImpostos.ICMS:
                        var icms = await AdicionarICMS();
                        if (icms != null)
                        {
                            var completo = new ICMS
                            {
                                Corpo = icms
                            };
                            adicionar.Add(new VisualizacaoImposto(completo));
                        }
                        break;
                    case PrincipaisImpostos.IPI:
                        var ipi = await AdicionarIPI();
                        if (ipi != null)
                        {
                            adicionar.Add(new VisualizacaoImposto(ipi));
                        }
                        break;
                    case PrincipaisImpostos.II:
                        var ii = await AdicionarII();
                        if (ii != null)
                        {
                            adicionar.Add(new VisualizacaoImposto(ii));
                        }
                        break;
                    case PrincipaisImpostos.ISSQN:
                        var issqn = await AdicionarISSQN();
                        if (issqn != null)
                        {
                            adicionar.Add(new VisualizacaoImposto(issqn));
                        }
                        break;
                    case PrincipaisImpostos.PIS:
                        var pis = await AdicionarPIS();
                        if (pis != null)
                        {
                            for (int i = 0; i < pis.Length; i++)
                            {
                                adicionar.Add(new VisualizacaoImposto(pis[i]));
                            }
                        }
                        break;
                    case PrincipaisImpostos.COFINS:
                        var cofins = await AdicionarCOFINS();
                        if (cofins != null)
                        {
                            for (int i = 0; i < cofins.Length; i++)
                            {
                                adicionar.Add(new VisualizacaoImposto(cofins[i]));
                            }
                        }
                        break;
                    case PrincipaisImpostos.ICMSUFDest:
                        var icmsufdest = await AdicionarICMSDestino();
                        if (icmsufdest != null)
                        {
                            adicionar.Add(new VisualizacaoImposto(icmsufdest));
                        }
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < adicionar.Count; i++)
                {
                    ImpostosAdicionaveis.Remove(adicionar[i].Primitivo);
                    ImpostosAdicionados.Add(adicionar[i]);
                }
            }
        }

        private void RemoverImposto(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var imp = (VisualizacaoImposto)contexto;
            ImpostosAdicionaveis.Add(imp.Primitivo);
            ImpostosAdicionados.Remove(imp);
        }

        async Task<ComumICMS> AdicionarICMS()
        {
            var caixa = new EscolherTipoICMS();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                if (caixa.Regime == EscolherTipoICMS.Regimes.Normal)
                {
                    var caixa2 = new AdicionarICMSRN(int.Parse(caixa.TipoICMSRN));
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        var cst = caixa.TipoICMSRN;
                        var origem = caixa.Origem;
                        switch (int.Parse(caixa.TipoICMSRN))
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
                                    CST = "10",
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
                                    CST = "41",
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
                                    CST = "90",
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
                    var csosn = caixa.TipoICMSSN;
                    var origem = caixa.Origem;
                    var tipoICMSSN = int.Parse(caixa.TipoICMSSN);
                    switch (tipoICMSSN)
                    {
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
                        default:
                            var caixa2 = new AdicionarICMSSN(int.Parse(caixa.TipoICMSSN));
                            if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                            {
                                switch (tipoICMSSN)
                                {
                                    case 101:
                                        return new ICMSSN101()
                                        {
                                            CSOSN = csosn,
                                            Orig = origem,
                                            pCredSN = caixa2.pCredSN,
                                            vCredICMSSN = caixa2.vCredICMSSN
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
                            break;
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

        async Task<II> AdicionarII()
        {
            var caixa = new AdicionarII();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                return caixa.Imposto;
            }
            return null;
        }

        async Task<ICMSUFDest> AdicionarICMSDestino()
        {
            var caixa = new AdicionarICMSDestino();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                return caixa.Imposto;
            }
            return null;
        }

        void Concluir(object sender, RoutedEventArgs e)
        {
            ProdutoCompleto.Impostos.impostos.Clear();
            List<VisualizacaoImposto> impostos = new List<VisualizacaoImposto>(ImpostosAdicionados);
            impostos.Sort((x, y) => x.Primitivo.CompareTo(y.Primitivo));
            for (int i = 0; i < impostos.Count; i++)
            {
                ProdutoCompleto.Impostos.impostos.Add(impostos[i].Oficial);
            }

            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter as NFe;
            var info = parametro.Informacoes;

            var detalhes = ProdutoCompleto;
            if (detalhes.Número == 0)
            {
                detalhes.Número = info.produtos.Count + 1;
                info.produtos.Add(detalhes);
            }
            else
            {
                info.produtos[detalhes.Número - 1] = detalhes;
            }
            info.total = new Total(info.produtos);

            MainPage.Current.Retornar(true);
        }

        void Cancelar(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        async Task<bool> IValida.Verificar()
        {
            var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, escolha Cancelar.\r\n" +
                "Mas lembre-se que o produto não será salvo se escolher sair.", "Atenção");
            mensagem.Commands.Add(new UICommand("Sair"));
            mensagem.Commands.Add(new UICommand("Cancelar"));
            var resultado = await mensagem.ShowAsync();
            return resultado.Label == "Sair";
        }

        struct VisualizacaoImposto
        {
            public string Titulo { get; set; }
            public string InformacaoAdicional { get; set; }
            internal PrincipaisImpostos Primitivo { get; set; }
            internal bool Secundario { get; set; }
            internal Imposto Oficial { get; set; }

            public VisualizacaoImposto(Imposto original)
            {
                if (original is ICMS icms)
                {
                    string valor = "0";
                    var corpo = icms.Corpo;
                    foreach (var prop in corpo.GetType().GetProperties().Where(x => x.CanWrite))
                    {
                        if (prop.Name == "vICMS")
                        {
                            valor = prop.GetValue(corpo).ToString();
                        }
                    }

                    Titulo = "ICMS";
                    if (icms.Corpo is IRegimeNormal normal)
                    {
                        InformacaoAdicional = $"CST: {normal.CST}; Valor = {valor}";
                    }
                    else
                    {
                        var simples = icms.Corpo as ISimplesNacional;
                        InformacaoAdicional = $"CSOSN: {simples.CSOSN}; Valor = {valor}";
                    }
                    Primitivo = PrincipaisImpostos.ICMS;
                    Secundario = false;
                }
                else if (original is IPI ipi)
                {
                    string valor;
                    if (ipi.Corpo is IPITrib trib)
                    {
                        valor = trib.vIPI;
                    }
                    else
                    {
                        valor = "0";
                    }
                    Titulo = "IPI";
                    InformacaoAdicional = $"CST: {ipi.Corpo.CST}; Valor: {valor}";
                    Primitivo = PrincipaisImpostos.IPI;
                    Secundario = false;
                }
                else if (original is II ii)
                {
                    Titulo = "Imposto de importação";
                    InformacaoAdicional = $"Valor: {ii.vII}";
                    Primitivo = PrincipaisImpostos.II;
                    Secundario = false;
                }
                else if (original is ISSQN issqn)
                {
                    Titulo = "ISSQN";
                    InformacaoAdicional = $"Valor: {issqn.vISSQN}";
                    Primitivo = PrincipaisImpostos.ISSQN;
                    Secundario = false;
                }
                else if (original is PIS pis)
                {
                    string valor;
                    if (pis.Corpo is PISAliq aliq)
                    {
                        valor = aliq.vPIS;
                    }
                    else if (pis.Corpo is PISOutr outr)
                    {
                        valor = outr.vPIS;
                    }
                    else if (pis.Corpo is PISQtde qtde)
                    {
                        valor = qtde.vPIS;
                    }
                    else
                    {
                        valor = "0";
                    }
                    Titulo = "PIS";
                    InformacaoAdicional = $"CST: {pis.Corpo.CST}; Valor: {valor}";
                    Primitivo = PrincipaisImpostos.PIS;
                    Secundario = false;
                }
                else if (original is PISST pisst)
                {
                    Titulo = "PISST";
                    InformacaoAdicional = $"Valor: {pisst.vPIS}";
                    Primitivo = PrincipaisImpostos.PIS;
                    Secundario = true;
                }
                else if (original is COFINS cofins)
                {
                    string valor;
                    if (cofins.Corpo is COFINSAliq aliq)
                    {
                        valor = aliq.vCOFINS;
                    }
                    else if (cofins.Corpo is COFINSOutr outr)
                    {
                        valor = outr.vCOFINS;
                    }
                    else if (cofins.Corpo is COFINSQtde qtde)
                    {
                        valor = qtde.vCOFINS;
                    }
                    else
                    {
                        valor = "0";
                    }
                    Titulo = "COFINS";
                    InformacaoAdicional = $"CST: {cofins.Corpo.CST}; Valor: {valor}";
                    Primitivo = PrincipaisImpostos.COFINS;
                    Secundario = false;
                }
                else if (original is COFINSST cofinsst)
                {
                    Titulo = "COFINSST";
                    InformacaoAdicional = $"Valor: {cofinsst.vCOFINS}";
                    Primitivo = PrincipaisImpostos.COFINS;
                    Secundario = true;
                }
                else if (original is ICMSUFDest icmsufdest)
                {
                    Titulo = "ICMS da UF destinatário";
                    InformacaoAdicional = $"Valor para o remetente: {icmsufdest.VICMSUFRemet}; Valor para o destinatário: {icmsufdest.VICMSUFDest}";
                    Primitivo = PrincipaisImpostos.ICMSUFDest;
                    Secundario = false;
                }
                else
                {
                    throw new Exception();
                }
                Oficial = original;
            }
        }
    }

    enum PrincipaisImpostos
    {
        ICMS,
        IPI,
        II,
        ISSQN,
        PIS,
        COFINS,
        ICMSUFDest
    }
}
