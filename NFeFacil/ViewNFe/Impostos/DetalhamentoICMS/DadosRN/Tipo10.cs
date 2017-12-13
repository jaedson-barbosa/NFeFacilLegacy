using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo10 : BaseRN
    {
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        public Tipo10(TelasRN.Tipo10 tela)
        {
            modBC = tela.modBC;
            vBC = tela.vBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            vBCST = tela.vBCST;
            pICMSST = tela.pICMSST;
            vICMSST = tela.vICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS10()
            {
                CST = CST,
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pICMS = pICMS,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBCST = pRedBCST,
                vBC = vBC,
                vBCST = vBCST,
                vICMS = vICMS,
                vICMSST = vICMSST
            };
        }

        void CalcularICMS(ref ICMS10 icms, DetalhesProdutos prod, double valorTabela)
        {
            var vBC = CalcularBC(prod);
            var pICMS = Parse(icms.pICMS);
            var vICMS = vBC * pICMS / 100;
            icms.vICMS = ToStr(vICMS);

            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = double.IsNaN(valorTabela) ? (vBC + ObterIPI(prod)) * (100 + pMVAST) / 100 : valorTabela;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = (vBCST * pICMSST / 100) - vICMS;
            icms.vICMSST = ToStr(vICMSST);
        }
    }
}
