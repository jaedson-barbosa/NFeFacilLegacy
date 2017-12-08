using System;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public sealed class RoteiroAdicaoImpostos
    {
        Type Current { get; }
        Type[] Telas { get; }
        IProcessamentoImposto[] Processamentos { get; }

        public RoteiroAdicaoImpostos(List<IDetalhamentoImposto> impostos)
        {
            Telas = new Type[impostos.Count];
            Processamentos = new IProcessamentoImposto[impostos.Count];
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
