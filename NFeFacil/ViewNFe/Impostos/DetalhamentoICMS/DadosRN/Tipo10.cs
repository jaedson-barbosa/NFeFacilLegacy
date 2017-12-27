using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo10 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public Tipo10() { }
        public Tipo10(TelasRN.Tipo10 tela)
        {
            modBC = tela.modBC;
            pICMS = tela.pICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            var vBC = CalcularBC(prod);
            var vICMS = vBC * pICMS / 100;

            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = (vBC + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            vBCST *= 1 - (pRedBCSTd / 100);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;

            return new ICMS10()
            {
                CST = CST,
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pICMS = ToStr(pICMS, "F4"),
                pICMSST = ToStr(pICMSST, "F4"),
                pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null,
                pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null,
                vBC = ToStr(vBC),
                vBCST = ToStr(vBCST),
                vICMS = ToStr(vICMS),
                vICMSST = ToStr(vICMSST)
            };
        }
    }
}
