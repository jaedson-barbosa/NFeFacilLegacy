using NFeFacil.ViewNFe.CaixasImpostos;
using System;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public static class AssociacoesSimples
    {
        public static readonly Dictionary<int, Type> COFINS = new Dictionary<int, Type>
        {
            { 1, typeof(DetalhamentoCOFINS.DetalharAliquota) },
            { 2, typeof(DetalhamentoCOFINS.DetalharAliquota) },
            { 3, typeof(DetalhamentoCOFINS.DetalharQtde) },
            { 4, null },
            { 5, COFINSPadrao },
            { 6, null },
            { 7, null },
            { 8, null },
            { 9, null }
        };

        public static readonly Dictionary<int, Type> PIS = new Dictionary<int, Type>
        {
            { 1, typeof(DetalhamentoPIS.DetalharAliquota) },
            { 2, typeof(DetalhamentoPIS.DetalharAliquota) },
            { 3, typeof(DetalhamentoPIS.DetalharQtde) },
            { 4, null },
            { 5, PISPadrao },
            { 6, null },
            { 7, null },
            { 8, null },
            { 9, null }
        };

        public static readonly Dictionary<TiposCalculo, Type> IPI = new Dictionary<TiposCalculo, Type>
        {
            { TiposCalculo.Inexistente, typeof(DetalhamentoIPI.DetalharSimples) },
            { TiposCalculo.PorAliquota, typeof(DetalhamentoIPI.DetalharAliquota) },
            { TiposCalculo.PorValor, typeof(DetalhamentoIPI.DetalharQtde) }
        };

        public static readonly Type COFINSPadrao = typeof(DetalhamentoCOFINS.DetalharAmbos);
        public static readonly Type PISPadrao = typeof(DetalhamentoPIS.DetalharAmbos);
    }
}
