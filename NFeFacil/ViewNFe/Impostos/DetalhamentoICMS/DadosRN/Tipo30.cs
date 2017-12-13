using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo30 : BaseRN
    {
        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        public Tipo30(TelasRN.Tipo30 tela)
        {
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
            return new ICMS30()
            {
                CST = CST,
                modBCST = modBCST.ToString(),
                motDesICMS = motDesICMS,
                Orig = Origem,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBCST = pRedBCST,
                vBCST = vBCST,
                vICMSDeson = vICMSDeson,
                vICMSST = vICMSST
            };
        }

        void CalcularICMS(ref ICMS30 icms, DetalhesProdutos prod)
        {
            var pMVAST = string.IsNullOrEmpty(icms.pMVAST) ? 0 : Parse(icms.pMVAST);
            var pRedBCST = string.IsNullOrEmpty(icms.pRedBCST) ? 0 : Parse(icms.pRedBCST);
            var vBCST = (CalcularBC(prod) + ObterIPI(prod)) * (100 + pMVAST) / 100;
            vBCST *= 1 - (pRedBCST / 100);
            icms.vBCST = ToStr(vBCST);

            var pICMSST = Parse(icms.pICMSST);
            var vICMSST = vBCST * pICMSST / 100;
            icms.vICMSST = ToStr(vICMSST);
        }
    }
}
