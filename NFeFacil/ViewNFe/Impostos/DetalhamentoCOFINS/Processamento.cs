using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewNFe.CaixasImpostos;
using System.Globalization;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    public struct Processamento : IProcessamentoImposto
    {
        static CultureInfo CulturaPadrao = CultureInfo.InvariantCulture;
        public object Tela { private get; set; }
        public IDetalhamentoImposto Detalhamento { private get; set; }
        IDadosCOFINS dados;

        public Imposto[] Processar(ProdutoOuServico prod)
        {
            var resultado = dados.Processar(prod);
            if (resultado is Imposto[] list) return list;
            else return new Imposto[1] { (COFINS)resultado };
        }

        public bool ValidarDados(ILog log) => dados.Validar(log);

        public bool ValidarEntradaDados(ILog log)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var valida = (AssociacoesSimples.COFINS.ContainsKey(detalhamento.CST)
                    && AssociacoesSimples.COFINS[detalhamento.CST] == Tela.GetType())
                    || AssociacoesSimples.COFINSPadrao == Tela.GetType();
                if (valida)
                {
                    var cst = detalhamento.CST.ToString("00");
                    if (Tela is DetalharAliquota aliq)
                    {
                        dados = new DadosAliq()
                        {
                            CST = cst,
                            Aliquota = aliq.Aliquota
                        };
                    }
                    else if (Tela is DetalharQtde valor)
                    {
                        dados = new DadosQtde()
                        {
                            CST = cst,
                            Valor = valor.Valor
                        };
                    }
                    else if (Tela is DetalharAmbos outr)
                    {
                        if (detalhamento.CST == 5) dados = new DadosST()
                        {
                            CST = cst,
                            Aliquota = outr.Aliquota,
                            Valor = outr.Valor,
                            TipoCalculo = outr.TipoCalculo
                        };
                        else dados = new DadosOutr()
                        {
                            CST = cst,
                            Aliquota = outr.Aliquota,
                            Valor = outr.Valor,
                            TipoCalculo = outr.TipoCalculo
                        };
                    }
                    else
                    {
                        dados = new DadosNT();
                    }
                }
            }
            return false;
        }

        interface IDadosCOFINS
        {
            string CST { set; }
            bool Validar(ILog log);
            object Processar(ProdutoOuServico prod);
        }

        struct DadosAliq : IDadosCOFINS
        {
            public double Aliquota { private get; set; }
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                var vBC = prod.ValorTotalDouble;
                var pCOFINS = Aliquota;
                return new COFINS
                {
                    Corpo = new COFINSAliq
                    {
                        CST = CST,
                        vBC = vBC.ToString("F2", CulturaPadrao),
                        pCOFINS = pCOFINS.ToString("F4", CulturaPadrao),
                        vCOFINS = (vBC * pCOFINS / 100).ToString("F2", CulturaPadrao)
                    }
                };
            }

            public bool Validar(ILog log) => true;
        }

        struct DadosNT : IDadosCOFINS
        {
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                return new COFINS
                {
                    Corpo = new COFINSNT()
                    {
                        CST = CST
                    }
                };
            }

            public bool Validar(ILog log) => true;
        }

        struct DadosQtde : IDadosCOFINS
        {
            public double Valor { private get; set; }
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                var qBCProd = prod.QuantidadeComercializada;
                var vAliqProd = Valor;
                return new COFINS
                {
                    Corpo = new COFINSQtde
                    {
                        CST = CST,
                        qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                        vAliqProd = vAliqProd.ToString("F4", CulturaPadrao),
                        vCOFINS = (qBCProd * vAliqProd).ToString("F2", CulturaPadrao)
                    }
                };
            }

            public bool Validar(ILog log) => true;
        }

        sealed class DadosST : DadosDuplos
        {
            public override object Processar(ProdutoOuServico prod)
            {
                if (TipoCalculo == TiposCalculo.PorAliquota)
                {
                    var vBC = prod.ValorTotalDouble;
                    return new Imposto[2]
                    {
                        new COFINS
                        {
                            Corpo = new COFINSNT()
                            {
                                CST = CST
                            }
                        },
                        new COFINSST
                        {
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pCOFINS = Aliquota.ToString("F4", CulturaPadrao),
                            vCOFINS = (vBC * Aliquota/ 100).ToString("F2", CulturaPadrao)
                        }
                    };
                }
                else
                {
                    var qBCProd = prod.QuantidadeComercializada;
                    return new Imposto[2]
                    {
                        new COFINS
                        {
                            Corpo = new COFINSNT()
                            {
                                CST = CST
                            }
                        },
                        new COFINSST
                        {
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vCOFINS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                        }
                    };
                }
            }
        }

        sealed class DadosOutr : DadosDuplos
        {
            public override object Processar(ProdutoOuServico prod)
            {
                if (TipoCalculo == TiposCalculo.PorAliquota)
                {
                    var vBC = prod.ValorTotalDouble;
                    return new COFINS
                    {
                        Corpo = new COFINSOutr
                        {
                            CST = CST,
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pCOFINS = Aliquota.ToString("F4", CulturaPadrao),
                            vCOFINS = (vBC * Aliquota / 100).ToString("F2", CulturaPadrao)
                        }
                    };
                }
                else
                {
                    var qBCProd = prod.QuantidadeComercializada;
                    return new COFINS
                    {
                        Corpo = new COFINSOutr
                        {
                            CST = CST,
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vCOFINS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                        }
                    };
                }
            }
        }

        abstract class DadosDuplos : IDadosCOFINS
        {
            public double Aliquota { protected get; set; }
            public double Valor { protected get; set; }
            public TiposCalculo TipoCalculo { protected get; set; }
            public string CST { protected get; set; }

            public abstract object Processar(ProdutoOuServico prod);

            public bool Validar(ILog log) => true;
        }
    }
}
