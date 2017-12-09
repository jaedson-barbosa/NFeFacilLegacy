using System;
using System.Collections.Generic;
using System.Linq;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public sealed class RoteiroAdicaoImpostos
    {
        Type Current { get; }
        Type[] Telas { get; }
        ProcessamentoImposto[] Processamentos { get; }

        public RoteiroAdicaoImpostos(List<IDetalhamentoImposto> impostos)
        {
            Telas = new Type[impostos.Count];
            Processamentos = new ProcessamentoImposto[impostos.Count];
            for (int i = 0; i < impostos.Count; i++)
            {
                var atual = impostos[i];
                if (atual is DetalhamentoCOFINS.Detalhamento cofins)
                {
                    if (AssociacoesSimples.COFINS.ContainsKey(cofins.CST))
                    {
                        Telas[i] = AssociacoesSimples.COFINS[cofins.CST];
                    }
                    else
                    {
                        Telas[i] = AssociacoesSimples.COFINSPadrao;
                    }
                }
                else if (atual is DetalhamentoPIS.Detalhamento pis)
                {
                    if (AssociacoesSimples.PIS.ContainsKey(pis.CST))
                    {
                        Telas[i] = AssociacoesSimples.PIS[pis.CST];
                    }
                    else
                    {
                        Telas[i] = AssociacoesSimples.PISPadrao;
                    }
                }
                else if (atual is DetalhamentoIPI.Detalhamento ipi)
                {
                    Telas[i] = AssociacoesSimples.IPI[ipi.TipoCalculo];
                }
                else if (atual is DetalhamentoISSQN.Detalhamento issqn)
                {
                    Telas[i] = AssociacoesSimples.ISSQN[issqn.Exterior];
                }
                else if (atual is DetalhamentoII.Detalhamento ii)
                {
                    Telas[i] = typeof(DetalhamentoII.Detalhar);
                }
                else if (atual is DetalhamentoICMSUFDest.Detalhamento icmsUFDest)
                {
                    Telas[i] = typeof(DetalhamentoICMSUFDest.Detalhar);
                }
                else if (atual is DetalhamentoICMS.Detalhamento icms)
                {
                    if (icms.Regime == CaixasImpostos.EscolherTipoICMS.Regimes.Simples)
                    {
                        if (AssociacoesSimples.ICMSSimples.Contains(int.Parse(icms.TipoICMSSN)))
                        {
                            Telas[i] = null;
                        }
                        else
                        {
                            Telas[i] = typeof(DetalhamentoICMS.DetalharSN);
                        }
                    }
                    else
                    {
                        Telas[i] = typeof(DetalhamentoICMS.DetalharRN);
                    }
                }
            }
        }

        public void Avancar()
        {

        }

        public void Voltar()
        {

        }
    }
}
