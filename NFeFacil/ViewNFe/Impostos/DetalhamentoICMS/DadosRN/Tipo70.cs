using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo70 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        public Tipo70(TelasRN.Tipo70 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            vICMSDeson = tela.vICMSDeson;
            motDesICMS = tela.motDesICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            vBCST = tela.vBCST;
            pICMSST = tela.pICMSST;
            vICMSST = tela.vICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS70()
            {
                CST = CST,
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                motDesICMS = motDesICMS,
                Orig = Origem,
                pICMS = pICMS,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBC = pRedBC,
                pRedBCST = pRedBCST,
                vBC = vBC,
                vBCST = vBCST,
                vICMS = vICMS,
                vICMSDeson = vICMSDeson,
                vICMSST = vICMSST
            };
        }

        void CalcularICMS(ref ICMS70 icms, DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
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
            var vBCST = (vBC + ObterIPI(prod)) * (100 + pMVAST) / 100;
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
    }
}
