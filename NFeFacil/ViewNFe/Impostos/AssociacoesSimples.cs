using NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS;
using System;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public static class AssociacoesSimples
    {
        public static readonly Dictionary<int, Type> COFINS = new Dictionary<int, Type>
        {
            { 1, typeof(DetalharCOFINSAliquota) },
            { 2, typeof(DetalharCOFINSAliquota) },
            { 3, typeof(DetalharCOFINSQtde) },
            { 4, null },
            { 5, COFINSPadrao },
            { 6, null },
            { 7, null },
            { 8, null },
            { 9, null }
        };
        public static readonly Type COFINSPadrao = typeof(DetalharCOFINSAmbos);
    }
}
