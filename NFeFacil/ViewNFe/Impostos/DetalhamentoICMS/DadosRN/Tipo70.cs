using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    class Tipo70 : BaseRN
    {
        public int modBC { get; set; }
        public double pRedBC { get; set; }
        public double pICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public string motDesICMS { get; set; }

        public Tipo70(TelasRN.Tipo70 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;

            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
            var bcSemReducao = vBC * pICMS / 100;
            vBC *= 1 - (pRedBC / 100);
            var vICMS = vBC * pICMS / 100;

            double pMVASTd;
            bool usarpMVAST = TryParse(pMVAST, out pMVASTd);
            double pRedBCSTd;
            bool usarpRedBCST = TryParse(pRedBCST, out pRedBCSTd);
            var vBCST = (vBC + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            var bcstSemReducao = (vBCST * pICMSST / 100) - vICMS;

            vBCST *= 1 - (pRedBCSTd / 100);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;

            var vICMSDeson = (bcSemReducao - vICMS) + (bcstSemReducao - vICMSST);
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

            return new ICMS70()
            {
                CST = CST,
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                motDesICMS = infDeson ? motDesICMS : null,
                Orig = Origem,
                pICMS = ToStr(pICMS, "F4"),
                pICMSST = ToStr(pICMSST, "F4"),
                pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null,
                pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null,
                vBC = ToStr(vBC),
                vBCST = ToStr(vBCST),
                vICMS = ToStr(vICMS),
                vICMSDeson = infDeson ? ToStr(vICMSDeson) : null,
                vICMSST = ToStr(vICMSST)
            };
        }
    }
}
