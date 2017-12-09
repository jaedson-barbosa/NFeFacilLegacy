using System;
using System.Linq;
using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    public sealed class Processamento : ProcessamentoImposto
    {
        public override Imposto[] Processar(ProdutoOuServico prod)
        {
            var detalhamento = (Detalhamento)Detalhamento;
            var origem = detalhamento.Origem;
            ComumICMS returno = null;
            if (detalhamento.Regime == CaixasImpostos.EscolherTipoICMS.Regimes.Simples)
            {
                var tipoICMSSN = int.Parse(detalhamento.TipoICMSSN);
                var simp = AssociacoesSimples.ICMSSimples;
                var csosn = detalhamento.TipoICMSSN;
                if (simp.Contains(tipoICMSSN))
                {
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
                        case 500:
                            returno = new ICMSSN500()
                            {
                                CSOSN = csosn,
                                Orig = origem
                            };
                            break;
                    }
                }
                else
                {
                    var caixa2 = (DetalharSN)Tela;
                    switch (tipoICMSSN)
                    {
                        case 101:
                            returno = new ICMSSN101()
                            {
                                CSOSN = csosn,
                                Orig = origem,
                                pCredSN = caixa2.pCredSN,
                                vCredICMSSN = caixa2.vCredICMSSN
                            };
                            break;
                        case 201:
                            returno = new ICMSSN201()
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
                            break;
                        case 202:
                            returno = new ICMSSN202()
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
                            break;
                        case 203:
                            returno = new ICMSSN202()
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
                            break;
                        case 900:
                            returno = new ICMSSN900()
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
                            break;
                    }
                }
            }
            else
            {
                var cst = detalhamento.TipoICMSRN;
                var caixa2 = (DetalharRN)Tela;
                switch (int.Parse(detalhamento.TipoICMSRN))
                {
                    case 0:
                        returno = new ICMS00()
                        {
                            CST = cst,
                            modBC = caixa2.modBC.ToString(),
                            Orig = origem,
                            pICMS = caixa2.pICMS,
                            vBC = caixa2.vBC,
                            vICMS = caixa2.vICMS
                        };
                        break;
                    case 10:
                        returno = new ICMS10()
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
                        break;
                    case 1010:
                        returno = new ICMSPart()
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
                        break;
                    case 20:
                        returno = new ICMS20()
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
                        break;
                    case 30:
                        returno = new ICMS30()
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
                        break;
                    case 40:
                        returno = new ICMS40()
                        {
                            CST = cst,
                            motDesICMS = caixa2.motDesICMS,
                            Orig = origem,
                            vICMSDeson = caixa2.vICMSDeson
                        };
                        break;
                    case 41:
                        returno = new ICMS41()
                        {
                            CST = cst,
                            motDesICMS = caixa2.motDesICMS,
                            Orig = origem,
                            vICMSDeson = caixa2.vICMSDeson
                        };
                        break;
                    case 4141:
                        returno = new ICMSST()
                        {
                            CST = "41",
                            Orig = origem,
                            vBCSTDest = caixa2.vBCSTDest,
                            vBCSTRet = caixa2.vBCSTRet,
                            vICMSSTDest = caixa2.vICMSSTDest,
                            vICMSSTRet = caixa2.vICMSSTRet
                        };
                        break;
                    case 50:
                        returno = new ICMS50()
                        {
                            CST = cst,
                            motDesICMS = caixa2.motDesICMS,
                            Orig = origem,
                            vICMSDeson = caixa2.vICMSDeson
                        };
                        break;
                    case 51:
                        returno = new ICMS51()
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
                        break;
                    case 60:
                        returno = new ICMS60()
                        {
                            CST = cst,
                            Orig = origem,
                            vBCSTRet = caixa2.vBCSTRet,
                            vICMSSTRet = caixa2.vICMSSTRet
                        };
                        break;
                    case 70:
                        returno = new ICMS70()
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
                        break;
                    case 90:
                        returno = new ICMS90()
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
                        break;
                    case 9090:
                        returno = new ICMSPart()
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
                if (detalhamento.Regime == CaixasImpostos.EscolherTipoICMS.Regimes.Simples)
                {
                    var simp = AssociacoesSimples.ICMSSimples;
                    if (simp.Contains(int.Parse(detalhamento.TipoICMSSN)))
                    {
                        return Tela == null;
                    }
                    else
                    {
                        return Tela.GetType() == typeof(DetalharSN);
                    }
                }
                else
                {
                    return Tela.GetType() == typeof(DetalharRN);
                }
            }
            return false;
        }
    }
}
