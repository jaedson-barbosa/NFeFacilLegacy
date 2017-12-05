using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using System.Globalization;

namespace NFeFacil.Calculos
{
    sealed class CalculoICMS
    {
        CultureInfo culturaPadrao = CultureInfo.InvariantCulture;

        void CalcularICMS(ref ICMS00 icms)
        {
            var vBC = Parse(icms.vBC);
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);
        }

        void CalcularICMS(ref ICMS10 icms, double vIPI, double valorTabela)
        {
            var vBC = Parse(icms.vBC);
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = double.IsNaN(valorTabela) ? (vBC + vIPI) * (100 + pMVAST) / 100 : valorTabela;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = (vBCST * pICMSST / 100) - vICMS;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMS20 icms)
        {
            var vBC = Parse(icms.vBC);
            var pICMS = Parse(icms.pICMS);
            var valorSemReducao = vBC * pICMS / 100;
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var vICMSDeson = valorSemReducao - vICMS;
            if (vICMSDeson == 0)
            {
                icms.vICMSDeson = null;
                icms.motDesICMS = null;
            }
            else
            {
                icms.vICMSDeson = ToStr(vICMSDeson);
            }
        }

        void CalcularICMS(ref ICMS30 icms, double total, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (total + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMS51 icms)
        {
            var vBC = Parse(icms.vBC);
            var pICMS = Parse(icms.pICMS);
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMSOp = vBC * pICMS / 100;
            icms.vICMSOp = ToStr(vICMSOp);

            var pDif = Parse(icms.pDif);
            var vICMSDif = vBC * pDif;
            icms.vICMSDif = ToStr(vICMSDif);

            var vICMS = vICMSOp - vICMSDif;
            icms.vICMS = ToStr(vICMS);
        }

        void CalcularICMS(ref ICMS70 icms, double vIPI)
        {
            var vBC = Parse(icms.vBC);
            var pICMS = Parse(icms.pICMS);
            var bcSemReducao = vBC * pICMS / 100;
            var pRedBC = Parse(icms.pRedBC);
            vBC *= 1 - (pRedBC / 100);
            icms.vBC = ToStr(vBC);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var pICMSST = Parse(icms.pICMSST);
            var vBCST = (vBC + vIPI) * (100 + pMVAST) / 100;
            var bcstSemReducao = (vBCST * pICMSST / 100) - vICMS;

            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;
            icms.vICMSST = ToStr(vICMSST);

            var vICMSDeson = (bcSemReducao - vICMS) + (bcstSemReducao - vICMSST);
            if (vICMSDeson == 0)
            {
                icms.vICMSDeson = null;
                icms.motDesICMS = null;
            }
            else
            {
                icms.vICMSDeson = ToStr(vICMSDeson);
            }
        }

        void CalcularICMS(ref ICMSSN201 icms, double total, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (total + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }

        void CalcularICMS(ref ICMSSN202 icms, double total, double vIPI)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (total + vIPI) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }

        string ToStr(double valor) => valor.ToString("F2", culturaPadrao);
        double Parse(string str) => double.Parse(str, NumberStyles.Number, culturaPadrao);
    }
}
