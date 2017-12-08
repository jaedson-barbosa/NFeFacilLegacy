using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewNFe.CaixasImpostos;
using System.Globalization;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    public struct Processamento : IProcessamentoImposto
    {
        static CultureInfo CulturaPadrao = CultureInfo.InvariantCulture;
        public object Tela { private get; set; }
        public IDetalhamentoImposto Detalhamento { private get; set; }
        IDadosPIS dados;

        public Imposto[] Processar(ProdutoOuServico prod)
        {
            var resultado = dados.Processar(prod);
            if (resultado is Imposto[] list) return list;
            else return new Imposto[1] { (PIS)resultado };
        }

        public bool ValidarDados(ILog log) => dados.Validar(log);

        public bool ValidarEntradaDados(ILog log)
        {
            if (Detalhamento is Detalhamento detalhamento)
            {
                var valida = (AssociacoesSimples.PIS.ContainsKey(detalhamento.CST)
                    && AssociacoesSimples.PIS[detalhamento.CST] == Tela.GetType())
                    || AssociacoesSimples.PISPadrao == Tela.GetType();
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

        interface IDadosPIS
        {
            string CST { set; }
            bool Validar(ILog log);
            object Processar(ProdutoOuServico prod);
        }

        struct DadosAliq : IDadosPIS
        {
            public double Aliquota { private get; set; }
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                var vBC = prod.ValorTotalDouble;
                var pPIS = Aliquota;
                return new PIS
                {
                    Corpo = new PISAliq
                    {
                        CST = CST,
                        vBC = vBC.ToString("F2", CulturaPadrao),
                        pPIS = pPIS.ToString("F4", CulturaPadrao),
                        vPIS = (vBC * pPIS / 100).ToString("F2", CulturaPadrao)
                    }
                };
            }

            public bool Validar(ILog log) => true;
        }

        struct DadosNT : IDadosPIS
        {
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                return new PIS
                {
                    Corpo = new PISNT()
                    {
                        CST = CST
                    }
                };
            }

            public bool Validar(ILog log) => true;
        }

        struct DadosQtde : IDadosPIS
        {
            public double Valor { private get; set; }
            public string CST { private get; set; }

            public object Processar(ProdutoOuServico prod)
            {
                var qBCProd = prod.QuantidadeComercializada;
                var vAliqProd = Valor;
                return new PIS
                {
                    Corpo = new PISQtde
                    {
                        CST = CST,
                        qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                        vAliqProd = vAliqProd.ToString("F4", CulturaPadrao),
                        vPIS = (qBCProd * vAliqProd).ToString("F2", CulturaPadrao)
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
                        new PIS
                        {
                            Corpo = new PISNT()
                            {
                                CST = CST
                            }
                        },
                        new PISST
                        {
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pPIS = Aliquota.ToString("F4", CulturaPadrao),
                            vPIS = (vBC * Aliquota/ 100).ToString("F2", CulturaPadrao)
                        }
                    };
                }
                else
                {
                    var qBCProd = prod.QuantidadeComercializada;
                    return new Imposto[2]
                    {
                        new PIS
                        {
                            Corpo = new PISNT()
                            {
                                CST = CST
                            }
                        },
                        new PISST
                        {
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vPIS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
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
                    return new PIS
                    {
                        Corpo = new PISOutr
                        {
                            CST = CST,
                            vBC = vBC.ToString("F2", CulturaPadrao),
                            pPIS = Aliquota.ToString("F4", CulturaPadrao),
                            vPIS = (vBC * Aliquota / 100).ToString("F2", CulturaPadrao)
                        }
                    };
                }
                else
                {
                    var qBCProd = prod.QuantidadeComercializada;
                    return new PIS
                    {
                        Corpo = new PISOutr
                        {
                            CST = CST,
                            qBCProd = qBCProd.ToString("F4", CulturaPadrao),
                            vAliqProd = Valor.ToString("F4", CulturaPadrao),
                            vPIS = (qBCProd * Valor).ToString("F2", CulturaPadrao)
                        }
                    };
                }
            }
        }

        abstract class DadosDuplos : IDadosPIS
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
