using System;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    public static class AssociacoesICMS
    {
        public static UserControl GetSimplesNacional(int csosn, DetalhamentoICMS.Detalhamento detalhamento)
        {
            switch (csosn)
            {
                case 101: return new DetalhamentoICMS.TelasSN.Tipo101(detalhamento);
                case 201: return new DetalhamentoICMS.TelasSN.Tipo201(detalhamento);
                case 202: return new DetalhamentoICMS.TelasSN.Tipo202(detalhamento);
                case 203: return new DetalhamentoICMS.TelasSN.Tipo202(detalhamento);
                case 500: return new DetalhamentoICMS.TelasSN.Tipo500(detalhamento);
                case 900: return new DetalhamentoICMS.TelasSN.Tipo900(detalhamento);
                default: return null;
            }
        }

        public static UserControl GetRegimeNormal(int cst, DetalhamentoICMS.Detalhamento detalhamento)
        {
            switch (cst)
            {
                case 0: return new DetalhamentoICMS.TelasRN.Tipo0(detalhamento);
                case 10: return new DetalhamentoICMS.TelasRN.Tipo10(detalhamento);
                case 20: return new DetalhamentoICMS.TelasRN.Tipo20(detalhamento);
                case 30: return new DetalhamentoICMS.TelasRN.Tipo30(detalhamento);
                case 40: return new DetalhamentoICMS.TelasRN.Tipo40_41_50(detalhamento);
                case 41: return new DetalhamentoICMS.TelasRN.Tipo40_41_50(detalhamento);
                case 50: return new DetalhamentoICMS.TelasRN.Tipo40_41_50(detalhamento);
                case 51: return new DetalhamentoICMS.TelasRN.Tipo51(detalhamento);
                case 60: return new DetalhamentoICMS.TelasRN.Tipo60(detalhamento);
                case 70: return new DetalhamentoICMS.TelasRN.Tipo70(detalhamento);
                case 90: return new DetalhamentoICMS.TelasRN.Tipo90(detalhamento);
                case 1010: return new DetalhamentoICMS.TelasRN.TipoPart(detalhamento);
                case 4141: return new DetalhamentoICMS.TelasRN.TipoICMSST(detalhamento);
                case 9090: return new DetalhamentoICMS.TelasRN.TipoPart(detalhamento);
                default: throw new ArgumentException("Tipo CST desconhecido");
            }
        }
    }
}
