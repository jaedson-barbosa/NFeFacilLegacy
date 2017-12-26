using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public sealed class RoteiroAdicaoImpostos
    {
        int index = -1;
        public Type Current => index >= 0 ? Telas[index] : null;
        Type[] Telas { get; }
        ProcessamentoImposto[] Processamentos { get; }
        DetalhesProdutos Produto { get; }

        public RoteiroAdicaoImpostos(List<IDetalhamentoImposto> impostos, DetalhesProdutos prod)
        {
            Produto = prod;
            Telas = new Type[impostos.Count];
            Processamentos = new ProcessamentoImposto[impostos.Count];
            for (int i = 0; i < impostos.Count; i++)
            {
                var atual = impostos[i];
                if (atual is DetalhamentoCOFINS.Detalhamento cofins)
                {
                    Telas[i] = AssociacoesSimples.COFINS[cofins.TipoCalculo];
                    Processamentos[i] = new DetalhamentoCOFINS.Processamento()
                    {
                        Detalhamento = cofins,
                    };
                }
                else if (atual is DetalhamentoPIS.Detalhamento pis)
                {
                    Telas[i] = AssociacoesSimples.PIS[pis.TipoCalculo];
                    Processamentos[i] = new DetalhamentoPIS.Processamento()
                    {
                        Detalhamento = pis,
                    };
                }
                else if (atual is DetalhamentoIPI.Detalhamento ipi)
                {
                    Telas[i] = AssociacoesSimples.IPI[ipi.TipoCalculo];
                    Processamentos[i] = new DetalhamentoIPI.Processamento()
                    {
                        Detalhamento = ipi,
                    };
                }
                else if (atual is DetalhamentoISSQN.Detalhamento issqn)
                {
                    Telas[i] = AssociacoesSimples.ISSQN[issqn.Exterior];
                    Processamentos[i] = new DetalhamentoISSQN.Processamento()
                    {
                        Detalhamento = issqn,
                    };
                }
                else if (atual is DetalhamentoII.Detalhamento ii)
                {
                    Telas[i] = typeof(DetalhamentoII.Detalhar);
                    Processamentos[i] = new DetalhamentoII.Processamento()
                    {
                        Detalhamento = ii,
                    };
                }
                else if (atual is DetalhamentoICMSUFDest.Detalhamento icmsUFDest)
                {
                    Telas[i] = typeof(DetalhamentoICMSUFDest.Detalhar);
                    Processamentos[i] = new DetalhamentoICMSUFDest.Processamento()
                    {
                        Detalhamento = icmsUFDest,
                    };
                }
                else if (atual is DetalhamentoICMS.Detalhamento icms)
                {
                    var normal = DefinicoesTemporarias.EmitenteAtivo.RegimeTributario == 3;
                    if (!normal)
                    {
                        var csosn = int.Parse(icms.TipoICMSSN);
                        Telas[i] = AssociacoesSimples.ICMSSN[csosn];
                    }
                    else
                    {
                        var cst = int.Parse(icms.TipoICMSRN);
                        Telas[i] = AssociacoesSimples.ICMSRN[cst];
                    }
                    Processamentos[i] = new DetalhamentoICMS.Processamento()
                    {
                        Detalhamento = icms,
                    };
                }
            }
        }

        public bool Avancar()
        {
            index++;
            return index != Processamentos.Length;
        }

        public bool Voltar()
        {
            if (index >= 0 && !finalizado)
            {
                index--;
                return index > 0;
            }
            return false;
        }

        public bool Validar(Page pagina)
        {
            if (index >= 0)
            {
                var proc = Processamentos[index];
                proc.ProcessarEntradaDados(pagina);
                return proc.ValidarDados();
            }
            return true;
        }

        bool finalizado;
        public DetalhesProdutos Finalizar()
        {
            Produto.Impostos.impostos.Clear();
            var impostos = Produto.Impostos.impostos;
            foreach (var item in Processamentos.OrderBy(x => (int)x.Tipo))
            {
                impostos.AddRange(item.Processar(Produto));
            }
            finalizado = true;
            return Produto;
        }
    }
}
