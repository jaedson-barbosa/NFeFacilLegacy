using System;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    public static class AssociacoesSimples
    {
        public static readonly Dictionary<TiposCalculo, Type> COFINS = new Dictionary<TiposCalculo, Type>
        {
            { TiposCalculo.Inexistente, null },
            { TiposCalculo.PorAliquota, typeof(DetalhamentoCOFINS.DetalharAliquota) },
            { TiposCalculo.PorValor, typeof(DetalhamentoCOFINS.DetalharQtde) }
        };

        public static readonly Dictionary<TiposCalculo, Type> PIS = new Dictionary<TiposCalculo, Type>
        {
            { TiposCalculo.Inexistente, null },
            { TiposCalculo.PorAliquota, typeof(DetalhamentoPIS.DetalharAliquota) },
            { TiposCalculo.PorValor, typeof(DetalhamentoPIS.DetalharQtde) }
        };

        public static readonly Dictionary<TiposCalculo, Type> IPI = new Dictionary<TiposCalculo, Type>
        {
            { TiposCalculo.Inexistente, typeof(DetalhamentoIPI.DetalharSimples) },
            { TiposCalculo.PorAliquota, typeof(DetalhamentoIPI.DetalharAliquota) },
            { TiposCalculo.PorValor, typeof(DetalhamentoIPI.DetalharQtde) }
        };

        public static readonly Dictionary<bool, Type> ISSQN = new Dictionary<bool, Type>
        {
            { true, typeof(DetalhamentoISSQN.DetalharExterior) },
            { false, typeof(DetalhamentoISSQN.DetalharNacional) }
        };

        public static readonly Dictionary<int, Type> ICMSSN = new Dictionary<int, Type>
        {
            { 101, typeof(DetalhamentoICMS.TelasSN.Tipo101) },
            { 102, null },
            { 103, null },
            { 201, typeof(DetalhamentoICMS.TelasSN.Tipo201) },
            { 202, typeof(DetalhamentoICMS.TelasSN.Tipo202) },
            { 203, typeof(DetalhamentoICMS.TelasSN.Tipo202) },
            { 300, null },
            { 400, null },
            { 500, typeof(DetalhamentoICMS.TelasSN.Tipo500) },
            { 900, typeof(DetalhamentoICMS.TelasSN.Tipo900) }
        };

        public static readonly Dictionary<int, Type> ICMSRN = new Dictionary<int, Type>
        {
            { 0, typeof(DetalhamentoICMS.TelasRN.Tipo0) },
            { 10, typeof(DetalhamentoICMS.TelasRN.Tipo10) },
            { 1010, typeof(DetalhamentoICMS.TelasRN.TipoPart) },
            { 20, typeof(DetalhamentoICMS.TelasRN.Tipo20) },
            { 30, typeof(DetalhamentoICMS.TelasRN.Tipo30) },
            { 40, typeof(DetalhamentoICMS.TelasRN.Tipo40_41_50) },
            { 41, typeof(DetalhamentoICMS.TelasRN.Tipo40_41_50) },
            { 4141, typeof(DetalhamentoICMS.TelasRN.TipoICMSST) },
            { 50, typeof(DetalhamentoICMS.TelasRN.Tipo40_41_50) },
            { 51, typeof(DetalhamentoICMS.TelasRN.Tipo51) },
            { 60, typeof(DetalhamentoICMS.TelasRN.Tipo60) },
            { 70, typeof(DetalhamentoICMS.TelasRN.Tipo70) },
            { 90, typeof(DetalhamentoICMS.TelasRN.Tipo90) },
            { 9090, typeof(DetalhamentoICMS.TelasRN.TipoPart) },
        };
    }
}
