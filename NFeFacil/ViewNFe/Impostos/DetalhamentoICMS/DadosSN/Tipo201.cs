using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using static NFeFacil.ExtensoesPrincipal;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    class Tipo201 : BaseSN
    {
        public int modBCST { get; private set; }
        public string pMVAST { get; private set; }
        public string pRedBCST { get; private set; }
        public double pICMSST { get; private set; }

        public string pCredSN { get; private set; }
        public string vCredICMSSN { get; private set; }

        public Tipo201(TelasSN.Tipo201 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;

            pCredSN = tela.pCredSN;
            vCredICMSSN = tela.vCredICMSSN;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            double pMVASTd;
            bool usarpMVAST = TryParse(pMVAST, out pMVASTd);
            double pRedBCSTd;
            bool usarpRedBCST = TryParse(pRedBCST, out pRedBCSTd);
            var vBCST = CalcularBC(prod) * (100 + pMVASTd) / 100;
            vBCST *= 1 - (pRedBCSTd / 100);
            var vICMSST = vBCST * pICMSST / 100;

            return new ICMSSN201()
            {
                CSOSN = CSOSN,
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pCredSN = pCredSN,
                pICMSST = ToStr(pICMSST, "F4"),
                pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null,
                pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null,
                vBCST = ToStr(vBCST),
                vCredICMSSN = vCredICMSSN,
                vICMSST = ToStr(vICMSST)
            };
        }
    }
}
