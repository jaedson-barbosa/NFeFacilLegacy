using System.Linq;
using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;


namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        public override Imposto[] Processar(ProdutoOuServico prod)
        {
            var detalhamento = (Detalhamento)Detalhamento;
            var origem = detalhamento.Origem;
            ComumICMS returno = null;
            var normal = Propriedades.EmitenteAtivo.RegimeTributario == 3;
            if (!normal)
            {
                var tipoICMSSN = int.Parse(detalhamento.TipoICMSSN);
                var csosn = detalhamento.TipoICMSSN;
                switch (tipoICMSSN)
                {
                    case 102:
                        returno = new ICMSSN102()
                        {
                            CSOSN = csosn,
                            Orig = origem
                        };
                        break;
                    case 103:
                        returno = new ICMSSN102()
                        {
                            CSOSN = csosn,
                            Orig = origem
                        };
                        break;
                    case 300:
                        returno = new ICMSSN102()
                        {
                            CSOSN = csosn,
                            Orig = origem
                        };
                        break;
                    case 400:
                        returno = new ICMSSN102()
                        {
                            CSOSN = csosn,
                            Orig = origem
                        };
                        break;
                    case 101:
                        var tipo101 = (SimplesNacional.Tipo101)Tela;
                        returno = new ICMSSN101()
                        {
                            CSOSN = csosn,
                            Orig = origem,
                            pCredSN = tipo101.pCredSN,
                            vCredICMSSN = tipo101.vCredICMSSN
                        };
                        break;
                    case 201:
                        var tipo201 = (SimplesNacional.Tipo201)Tela;
                        returno = new ICMSSN201()
                        {
                            CSOSN = csosn,
                            modBCST = tipo201.modBCST.ToString(),
                            Orig = origem,
                            pCredSN = tipo201.pCredSN,
                            pICMSST = tipo201.pICMSST,
                            pMVAST = tipo201.pMVAST,
                            pRedBCST = tipo201.pRedBCST,
                            vBCST = tipo201.vBCST,
                            vCredICMSSN = tipo201.vCredICMSSN,
                            vICMSST = tipo201.vICMSST
                        };
                        break;
                    case 202:
                        var tipo202 = (SimplesNacional.Tipo202)Tela;
                        returno = new ICMSSN202()
                        {
                            CSOSN = csosn,
                            modBCST = tipo202.modBCST.ToString(),
                            Orig = origem,
                            pICMSST = tipo202.pICMSST,
                            pMVAST = tipo202.pMVAST,
                            pRedBCST = tipo202.pRedBCST,
                            vBCST = tipo202.vBCST,
                            vICMSST = tipo202.vICMSST
                        };
                        break;
                    case 203:
                        tipo202 = (SimplesNacional.Tipo202)Tela;
                        returno = new ICMSSN202()
                        {
                            CSOSN = csosn,
                            modBCST = tipo202.modBCST.ToString(),
                            Orig = origem,
                            pICMSST = tipo202.pICMSST,
                            pMVAST = tipo202.pMVAST,
                            pRedBCST = tipo202.pRedBCST,
                            vBCST = tipo202.vBCST,
                            vICMSST = tipo202.vICMSST
                        };
                        break;
                    case 500:
                        var tipo500 = (SimplesNacional.Tipo500)Tela;
                        returno = new ICMSSN500()
                        {
                            CSOSN = csosn,
                            Orig = origem,
                            vBCSTRet = tipo500.vBCSTRet,
                            vICMSSTRet = tipo500.vICMSSTRet
                        };
                        break;
                    case 900:
                        var tipo900 = (SimplesNacional.Tipo900)Tela;
                        returno = new ICMSSN900()
                        {
                            CSOSN = csosn,
                            modBC = tipo900.modBC.ToString(),
                            modBCST = tipo900.modBCST.ToString(),
                            Orig = origem,
                            pCredSN = tipo900.pCredSN,
                            pICMS = tipo900.pICMS,
                            pICMSST = tipo900.pICMSST,
                            pMVAST = tipo900.pMVAST,
                            pRedBC = tipo900.pRedBC,
                            pRedBCST = tipo900.pRedBCST,
                            vBC = tipo900.vBC,
                            vBCST = tipo900.vBCST,
                            vCredICMSSN = tipo900.vCredICMSSN,
                            vICMS = tipo900.vICMS,
                            vICMSST = tipo900.vICMSST
                        };
                        break;
                }
            }
            else
            {
                var cst = detalhamento.TipoICMSRN;
                switch (int.Parse(detalhamento.TipoICMSRN))
                {
                    case 0:
                        var tipo00 = (RegimeNormal.Tipo0)Tela;
                        returno = new ICMS00()
                        {
                            CST = cst,
                            modBC = tipo00.modBC.ToString(),
                            Orig = origem,
                            pICMS = tipo00.pICMS,
                            vBC = tipo00.vBC,
                            vICMS = tipo00.vICMS
                        };
                        break;
                    case 10:
                        var tipo10 = (RegimeNormal.Tipo10)Tela;
                        returno = new ICMS10()
                        {
                            CST = cst,
                            modBC = tipo10.modBC.ToString(),
                            modBCST = tipo10.modBCST.ToString(),
                            Orig = origem,
                            pICMS = tipo10.pICMS,
                            pICMSST = tipo10.pICMSST,
                            pMVAST = tipo10.pMVAST,
                            pRedBCST = tipo10.pRedBCST,
                            vBC = tipo10.vBC,
                            vBCST = tipo10.vBCST,
                            vICMS = tipo10.vICMS,
                            vICMSST = tipo10.vICMSST
                        };
                        break;
                    case 1010:
                        var tipoPart = (RegimeNormal.TipoPart)Tela;
                        returno = new ICMSPart()
                        {
                            CST = "10",
                            modBC = tipoPart.modBC.ToString(),
                            modBCST = tipoPart.modBCST.ToString(),
                            Orig = origem,
                            pICMS = tipoPart.pICMS,
                            pICMSST = tipoPart.pICMSST,
                            pMVAST = tipoPart.pMVAST,
                            pRedBC = tipoPart.pRedBC,
                            pRedBCST = tipoPart.pRedBCST,
                            vBC = tipoPart.vBC,
                            vBCST = tipoPart.vBCST,
                            vICMS = tipoPart.vICMS,
                            vICMSST = tipoPart.vICMSST,
                            pBCOp = tipoPart.pBCOp,
                            UFST = tipoPart.UFST
                        };
                        break;
                    case 20:
                        var tipo20 = (RegimeNormal.Tipo20)Tela;
                        returno = new ICMS20()
                        {
                            CST = cst,
                            modBC = tipo20.modBC.ToString(),
                            motDesICMS = tipo20.motDesICMS,
                            Orig = origem,
                            pICMS = tipo20.pICMS,
                            vBC = tipo20.vBC,
                            vICMS = tipo20.vICMS,
                            vICMSDeson = tipo20.vICMSDeson,
                            pRedBC = tipo20.pRedBC
                        };
                        break;
                    case 30:
                        var tipo30 = (RegimeNormal.Tipo30)Tela;
                        returno = new ICMS30()
                        {
                            CST = cst,
                            modBCST = tipo30.modBCST.ToString(),
                            motDesICMS = tipo30.motDesICMS,
                            Orig = origem,
                            pICMSST = tipo30.pICMSST,
                            pMVAST = tipo30.pMVAST,
                            pRedBCST = tipo30.pRedBCST,
                            vBCST = tipo30.vBCST,
                            vICMSDeson = tipo30.vICMSDeson,
                            vICMSST = tipo30.vICMSST
                        };
                        break;
                    case 40:
                        var tipo40 = (RegimeNormal.Tipo40_41_50)Tela;
                        returno = new ICMS40()
                        {
                            CST = cst,
                            motDesICMS = tipo40.motDesICMS,
                            Orig = origem,
                            vICMSDeson = tipo40.vICMSDeson
                        };
                        break;
                    case 41:
                        tipo40 = (RegimeNormal.Tipo40_41_50)Tela;
                        returno = new ICMS41()
                        {
                            CST = cst,
                            motDesICMS = tipo40.motDesICMS,
                            Orig = origem,
                            vICMSDeson = tipo40.vICMSDeson
                        };
                        break;
                    case 4141:
                        var tipoST = (RegimeNormal.TipoICMSST)Tela;
                        returno = new ICMSST()
                        {
                            CST = "41",
                            Orig = origem,
                            vBCSTDest = tipoST.vBCSTDest,
                            vBCSTRet = tipoST.vBCSTRet,
                            vICMSSTDest = tipoST.vICMSSTDest,
                            vICMSSTRet = tipoST.vICMSSTRet
                        };
                        break;
                    case 50:
                        tipo40 = (RegimeNormal.Tipo40_41_50)Tela;
                        returno = new ICMS50()
                        {
                            CST = cst,
                            motDesICMS = tipo40.motDesICMS,
                            Orig = origem,
                            vICMSDeson = tipo40.vICMSDeson
                        };
                        break;
                    case 51:
                        var tipo51 = (RegimeNormal.Tipo51)Tela;
                        returno = new ICMS51()
                        {
                            CST = cst,
                            modBC = tipo51.modBC.ToString(),
                            Orig = origem,
                            pICMS = tipo51.pICMS,
                            pRedBC = tipo51.pRedBC,
                            vBC = tipo51.vBC,
                            vICMS = tipo51.vICMS,
                            pDif = tipo51.pDif,
                            vICMSDif = tipo51.vICMSDif,
                            vICMSOp = tipo51.vICMSOp
                        };
                        break;
                    case 60:
                        var tipo60 = (RegimeNormal.Tipo60)Tela;
                        returno = new ICMS60()
                        {
                            CST = cst,
                            Orig = origem,
                            vBCSTRet = tipo60.vBCSTRet,
                            vICMSSTRet = tipo60.vICMSSTRet
                        };
                        break;
                    case 70:
                        var tipo70 = (RegimeNormal.Tipo70)Tela;
                        returno = new ICMS70()
                        {
                            CST = cst,
                            modBC = tipo70.modBC.ToString(),
                            modBCST = tipo70.modBCST.ToString(),
                            motDesICMS = tipo70.motDesICMS,
                            Orig = origem,
                            pICMS = tipo70.pICMS,
                            pICMSST = tipo70.pICMSST,
                            pMVAST = tipo70.pMVAST,
                            pRedBC = tipo70.pRedBC,
                            pRedBCST = tipo70.pRedBCST,
                            vBC = tipo70.vBC,
                            vBCST = tipo70.vBCST,
                            vICMS = tipo70.vICMS,
                            vICMSDeson = tipo70.vICMSDeson,
                            vICMSST = tipo70.vICMSST
                        };
                        break;
                    case 90:
                        var tipo90 = (RegimeNormal.Tipo90)Tela;
                        returno = new ICMS90()
                        {
                            CST = cst,
                            modBC = tipo90.modBC.ToString(),
                            modBCST = tipo90.modBCST.ToString(),
                            motDesICMS = tipo90.motDesICMS,
                            Orig = origem,
                            pICMS = tipo90.pICMS,
                            pICMSST = tipo90.pICMSST,
                            pMVAST = tipo90.pMVAST,
                            pRedBC = tipo90.pRedBC,
                            pRedBCST = tipo90.pRedBCST,
                            vBC = tipo90.vBC,
                            vBCST = tipo90.vBCST,
                            vICMS = tipo90.vICMS,
                            vICMSDeson = tipo90.vICMSDeson,
                            vICMSST = tipo90.vICMSST
                        };
                        break;
                    case 9090:
                        tipoPart = (RegimeNormal.TipoPart)Tela;
                        returno = new ICMSPart()
                        {
                            CST = "90",
                            modBC = tipoPart.modBC.ToString(),
                            modBCST = tipoPart.modBCST.ToString(),
                            Orig = origem,
                            pICMS = tipoPart.pICMS,
                            pICMSST = tipoPart.pICMSST,
                            pMVAST = tipoPart.pMVAST,
                            pRedBC = tipoPart.pRedBC,
                            pRedBCST = tipoPart.pRedBCST,
                            vBC = tipoPart.vBC,
                            vBCST = tipoPart.vBCST,
                            vICMS = tipoPart.vICMS,
                            vICMSST = tipoPart.vICMSST,
                            pBCOp = tipoPart.pBCOp,
                            UFST = tipoPart.UFST
                        };
                        break;
                }
            }
            var imposto = new ICMS { Corpo = returno };
            return new Imposto[1] { imposto };
        }

        public override bool ValidarDados(ILog log) => true;

        public override bool ValidarEntradaDados(ILog log)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var normal = Propriedades.EmitenteAtivo.RegimeTributario == 3;
                if (!normal)
                {
                    var csosn = int.Parse(detalhamento.TipoICMSSN);
                    return Tela.GetType() == AssociacoesSimples.ICMSSN[csosn];
                }
                else
                {
                    var cst = int.Parse(detalhamento.TipoICMSRN);
                    return Tela.GetType() == AssociacoesSimples.ICMSRN[cst];
                }
            }
            return false;
        }
    }

    sealed class CalculoICMS
    {
        double BaseCalculoSimples { get; }

        public CalculoICMS(ProdutoOuServico prod)
        {
            var totalBruto = prod.ValorTotal;
            var frete = string.IsNullOrEmpty(prod.Frete) ? 0 : Parse(prod.Frete);
            var seguro = string.IsNullOrEmpty(prod.Seguro) ? 0 : Parse(prod.Seguro);
            var despesas = string.IsNullOrEmpty(prod.DespesasAcessorias) ? 0 : Parse(prod.DespesasAcessorias);
            var desconto = string.IsNullOrEmpty(prod.Desconto) ? 0 : Parse(prod.Desconto);

            BaseCalculoSimples = totalBruto + frete + seguro + despesas - desconto;
        }

        void CalcularICMS(ref ICMS00 icms)
        {
            var vBC = BaseCalculoSimples;
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);
        }

        void CalcularICMS(ref ICMS10 icms, double vIPI, double valorTabela)
        {
            var vBC = BaseCalculoSimples;
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = double.IsNaN(valorTabela) ? (vBC + vIPI) * (100 + pMVAST) / 100 : valorTabela;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = (vBCST * pICMSST / 100) - vICMS;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMS20 icms)
        {
            var vBC = BaseCalculoSimples;
            var pICMS = Parse(icms.pICMS);
            var valorSemReducao = vBC * pICMS / 100;
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var vICMSDeson = valorSemReducao - vICMS;
            if (vICMSDeson == 0)
            {
                icms.vICMSDeson = null;
                icms.motDesICMS = null;
            }
            else
            {
                icms.vICMSDeson = ToStr(vICMSDeson);
            }
        }

        void CalcularICMS(ref ICMS30 icms, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (BaseCalculoSimples + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMS51 icms)
        {
            var vBC = BaseCalculoSimples;
            var pICMS = Parse(icms.pICMS);
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMSOp = vBC * pICMS / 100;
            icms.vICMSOp = ToStr(vICMSOp);

            var pDif = Parse(icms.pDif);
            var vICMSDif = vBC * pDif;
            icms.vICMSDif = ToStr(vICMSDif);

            var vICMS = vICMSOp - vICMSDif;
            icms.vICMS = ToStr(vICMS);
        }

        void CalcularICMS(ref ICMS70 icms, double vIPI)
        {
            var vBC = BaseCalculoSimples;
            var pICMS = Parse(icms.pICMS);
            var bcSemReducao = vBC * pICMS / 100;
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var pICMSST = Parse(icms.pICMSST);
            var vBCST = (vBC + vIPI) * (100 + pMVAST) / 100;
            var bcstSemReducao = (vBCST * pICMSST / 100) - vICMS;

            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;
            icms.vICMSST = ToStr(vICMSST);

            var vICMSDeson = (bcSemReducao - vICMS) + (bcstSemReducao - vICMSST);
            if (vICMSDeson == 0)
            {
                icms.vICMSDeson = null;
                icms.motDesICMS = null;
            }
            else
            {
                icms.vICMSDeson = ToStr(vICMSDeson);
            }
        }

        void CalcularICMS(ref ICMSSN201 icms, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (BaseCalculoSimples + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMSSN202 icms, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (BaseCalculoSimples + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }
    }
}
